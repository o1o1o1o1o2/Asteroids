using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asteroids.Client.Db.ConfigsScriptableObjects;
using Asteroids.Client.Ecs.Components;
using Asteroids.Client.Ecs.Components.Visual;
using Asteroids.Client.Ecs.Contracts;
using Asteroids.Client.SceneViews;
using FlyLib.Core.AssetProvider.Contracts;
using FlyLib.Core.GameConfigs.Contracts;
using FlyLib.Core.Pool;
using SimpleEcs.Public;
using UnityEngine;

namespace Asteroids.Client.Ecs.Controls
{
	public class EnemyViewFactory : IEnemyViewFactory
	{
		private readonly IGameConfigs _gameConfigs;
		private readonly IAssetProvider _assetProvider;
		private readonly IEventsController _eventsController;
		private Dictionary<string, EnemyViewObject> _asteroidPrefabs;

		public EnemyViewFactory(IGameConfigs gameConfigs, IAssetProvider assetProvider, IEventsController eventsController)
		{
			_gameConfigs = gameConfigs;
			_assetProvider = assetProvider;
			_eventsController = eventsController;
		}

		public async Task Init()
		{
			var taskHandler = await _assetProvider.LoadByLabelAsync<GameObject>(_gameConfigs.GetConfig<ViewsConfig>().EnemyAsteroidAddressLabel);
			_asteroidPrefabs = taskHandler.Select(x => x.GetComponent<EnemyViewObject>())
				.ToDictionary(x => x.name, x => x);
		}

		public void CreateEnemyView(Entity logicEntity)
		{
			var view = SpawnView(logicEntity);

			if (view == null)
			{
				Debug.LogError($"Error wile try to spawn view for entity: {logicEntity}");
				return;
			}

			view.AddToEntity(logicEntity);

			InitView(logicEntity);
		}

		private EnemyViewObject SpawnView(Entity logicEntity)
		{
			var enemyDef = logicEntity.GetValue<CEnemyDef>();
			var prefab = _asteroidPrefabs[enemyDef.name];
			if (prefab == null)
			{
				Debug.LogError($"There is no prefab with name {enemyDef.name}");
			}

			var view = prefab.Spawn();
			view.TriggerEnter = () => _eventsController.OnTriggerEnter(logicEntity);
			view.TriggerEnterPlayer = () => _eventsController.OnPlayerTriggerEnter(logicEntity);
			return view;
		}

		private static void InitView(Entity logicEntity)
		{
			if (!logicEntity.Has<VMovableView>())
			{
				return;
			}

			var movableView = logicEntity.GetValue<VMovableView>();
			if (logicEntity.Has<CPosition>())
			{
				movableView.SetPosition(logicEntity.GetValue<CPosition>());
			}

			if (logicEntity.Has<CRotation>())
			{
				movableView.SetRotation(logicEntity.GetValue<CRotation>());
			}
		}
	}
}