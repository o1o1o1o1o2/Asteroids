using System.Linq;
using System.Threading.Tasks;
using Asteroids.Client.Db.ConfigsScriptableObjects;
using Asteroids.Client.Ecs.Components;
using Asteroids.Client.Ecs.Components.Visual;
using Asteroids.Client.Ecs.Contracts;
using Asteroids.Client.SceneViews;
using Asteroids.Client.Types;
using FlyLib.Core.AssetProvider.Contracts;
using FlyLib.Core.GameConfigs.Contracts;
using FlyLib.Core.Pool;
using FlyLib.Core.Utils;
using SimpleEcs.Contracts;
using SimpleEcs.Public;
using UnityEngine;

namespace Asteroids.Client.Ecs.Systems.Logic
{
	public class PlayerSpawnSystem : IInitializeSystem, IExecuteSystem
	{
		private struct PlayerCopyInfo
		{
			public Entity Entity { get; }
			public PlayerCopyType CopyType { get; }
			public bool IsInRect { get; }

			public PlayerCopyInfo(Entity entity, PlayerCopyType copyType, bool isInRect)
			{
				Entity = entity;
				CopyType = copyType;
				IsInRect = isInRect;
			}
		}

		private readonly Vector2 _spawnOffset = new Vector2(100, 100);

		private readonly GameContext _gameContext;
		private readonly IGameConfigs _gameConfigs;
		private readonly IAssetProvider _assetProvider;
		private readonly IEventsController _eventsController;
		private readonly IFilteredGroup _playerGroup;
		private PlayerViewObject _viewForSpawn;

		private Rect _camRect;
		private Rect _copyRect;
		private Rect _thresholdRect;
		private readonly Vector3 _horizontalOffset;
		private readonly Vector3 _verticalOffset;

		public PlayerSpawnSystem(GameContext gameContext, IGameConfigs gameConfigs, IAssetProvider assetProvider, IEventsController eventsController)
		{
			_gameContext = gameContext;
			_gameConfigs = gameConfigs;
			_assetProvider = assetProvider;
			_eventsController = eventsController;
			_playerGroup = gameContext.AllOf<CPlayerTag, CPosition>();

			_camRect = Camera.main.GetCamWorldRect();
			_copyRect = new Rect(_camRect.position + _spawnOffset, _camRect.size - _spawnOffset * 2);
			_thresholdRect = new Rect(_camRect.position - _camRect.size + _spawnOffset, _camRect.size + _camRect.size * 2 - _spawnOffset * 2);
			_horizontalOffset = new Vector3(_camRect.size.x, 0f, 0f);
			_verticalOffset = new Vector3(0f, _camRect.size.y, 0f);
		}

		public void Initialize()
		{
			InitializeAsync().Wait();
		}

		private async Task InitializeAsync()
		{
			if (_viewForSpawn == null)
			{
				var shipPrefabAddress = _gameConfigs.GetConfig<ViewsConfig>().ShipPrefabAddress;
				var assetGo = await _assetProvider.LoadAsync<GameObject>(shipPrefabAddress);
				assetGo.TryGetComponent(out _viewForSpawn);

				if (_viewForSpawn == null)
				{
					Debug.LogError($"Prefab with address:{shipPrefabAddress} does not have component{nameof(PlayerViewObject)}");
					return;
				}
			}

			SpawnPlayer(PlayerCopyType.Main, _camRect.center, Quaternion.identity, Vector3.zero);
		}

		private void SpawnPlayer(PlayerCopyType playerCopyType, Vector3 position, Quaternion rotation, Vector3 velocity)
		{
			var playerE = _gameContext.CreateEntity();
			playerE.SetTag<CPlayerTag>(true);
			playerE.Set<CPlayerCopyType>(playerCopyType);
			playerE.Set<CPosition>(position);
			playerE.Set<CRotation>(rotation);
			playerE.Set<CVelocity>(velocity);
			var view = _viewForSpawn.Spawn();
			view.AddToEntity(playerE);
		}

		public void Execute(float deltaTime)
		{
			var players = _playerGroup.Select(x => new PlayerCopyInfo(x, x.GetValue<CPlayerCopyType>(), _copyRect.Contains(x.GetValue<CPosition>())))
				.ToArray();

			var playerInRectIdx = players.IndexOf(x => x.IsInRect);
			if (playerInRectIdx >= 0)
			{
				RemovePlayerCopiesNotInRect(players[playerInRectIdx].Entity);
				return;
			}

			OffsetCopiesPosOnReachThreshold(players);

			if (players.Length == 4)
			{
				return;
			}

			TrySpawnCopy(players);
		}

		//this need when all copies are not in copyRect and move parallel to copyRect border
		private void OffsetCopiesPosOnReachThreshold(PlayerCopyInfo[] playerCopyInfos)
		{
			var reachThresholdPlayerIdx = playerCopyInfos.IndexOf(x => !_thresholdRect.Contains(x.Entity.GetValue<CPosition>()));
			if (reachThresholdPlayerIdx < 0)
			{
				return;
			}

			var pos = playerCopyInfos[reachThresholdPlayerIdx].Entity.GetValue<CPosition>();

			var offsetHorizontal = TryGetCopyPosOnAxis(pos, _thresholdRect, _horizontalOffset, 0);
			var offsetVertical = TryGetCopyPosOnAxis(pos, _thresholdRect, _verticalOffset, 1);

			if (!offsetHorizontal.HasValue && !offsetVertical.HasValue)
			{
				return;
			}

			var offset = new Vector3(offsetHorizontal.HasValue ? offsetHorizontal.Value.x - pos.x : 0,
				offsetVertical.HasValue ? offsetVertical.Value.y - pos.y : 0, 0);
			foreach (var entity in _playerGroup)
			{
				entity.Set<CPosition>(entity.GetValue<CPosition>() + offset);
			}
		}

		private void TrySpawnCopy(PlayerCopyInfo[] playerCopyInfos)
		{
			var mainPlayer = playerCopyInfos.First(x => x.CopyType == PlayerCopyType.Main);
			var mainPlayerPos = mainPlayer.Entity.GetValue<CPosition>();
			//horizontal copy
			var copyPosHorizontal = TryGetCopyPosWithSpawnIfNeeded(playerCopyInfos, mainPlayer, mainPlayerPos, PlayerCopyType.Horizontal, _horizontalOffset, 0);

			//vertical copy
			var copyPosVertical = TryGetCopyPosWithSpawnIfNeeded(playerCopyInfos, mainPlayer, mainPlayerPos, PlayerCopyType.Vertical, _verticalOffset, 1);

			//diagonal copy
			if (!copyPosHorizontal.HasValue || !copyPosVertical.HasValue)
			{
				return;
			}

			if (playerCopyInfos.Any(x => x.CopyType == PlayerCopyType.Diagonal))
			{
				return;
			}

			SpawnPlayer(PlayerCopyType.Diagonal, new Vector3(copyPosHorizontal.Value.x, copyPosVertical.Value.y, 0f),
				mainPlayer.Entity.GetValue<CRotation>(),
				mainPlayer.Entity.GetValue<CVelocity>());
		}

		private Vector3? TryGetCopyPosWithSpawnIfNeeded(PlayerCopyInfo[] playerCopyInfos, PlayerCopyInfo mainPlayer, Vector3 mainPlayerPos,
			PlayerCopyType playerCopyType, Vector3 offset, int axis)
		{
			var copyIdx = playerCopyInfos.IndexOf(x => x.CopyType == playerCopyType);
			if (copyIdx >= 0)
			{
				return playerCopyInfos[copyIdx].Entity.GetValue<CPosition>();
			}

			var copyPos = TryGetCopyPosOnAxis(mainPlayerPos, _copyRect, offset, axis);

			if (!copyPos.HasValue)
			{
				return null;
			}

			SpawnPlayer(playerCopyType, copyPos.Value, mainPlayer.Entity.GetValue<CRotation>(), mainPlayer.Entity.GetValue<CVelocity>());

			return copyPos;
		}

		private Vector3? TryGetCopyPosOnAxis(Vector3 pos, Rect rect, Vector3 offset, int axis)
		{
			if (pos[axis] <= rect.min[axis])
			{
				return pos + offset;
			}

			if (pos[axis] < rect.max[axis])
			{
				return null;
			}

			return pos - offset;
		}

		private void RemovePlayerCopiesNotInRect(Entity playerInRect)
		{
			playerInRect.Set<CPlayerCopyType>(PlayerCopyType.Main);

			foreach (var pE in _playerGroup)
			{
				if (pE == playerInRect)
				{
					continue;
				}

				var view = pE.GetValue<VMovableView>();
				view?.Component.Recycle();
				pE.Destroy();
			}
		}
	}
}