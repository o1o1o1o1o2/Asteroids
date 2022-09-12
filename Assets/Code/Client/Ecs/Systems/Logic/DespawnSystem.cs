using Asteroids.Client.Ecs.Components;
using Asteroids.Client.Ecs.Components.Visual;
using Asteroids.Client.Types;
using FlyLib.Core.Pool;
using SimpleEcs.Contracts;
using SimpleEcs.Public;
using UnityEngine;

namespace Asteroids.Client.Ecs.Systems.Logic
{
	public class DespawnSystem : IExecuteSystem
	{
		private readonly IFilteredGroup _enemyGroup;

		private readonly Vector2 _destroyOffset = new Vector2(200, 100);
		private Rect _aliveInsideRect;

		public DespawnSystem(GameContext gameContext)
		{
			_enemyGroup = gameContext.AllOf<CDeSpawnableTag, CPosition, VMovableView>();

			var camRect = Camera.main.GetCamWorldRect();
			_aliveInsideRect = new Rect(camRect.position - _destroyOffset, camRect.size + _destroyOffset * 2);
		}

		public void Execute(float deltaTime)
		{
			foreach (var entity in _enemyGroup)
			{
				var pos = entity.GetValue<CPosition>();
				if (_aliveInsideRect.Contains(pos))
				{
					continue;
				}

				var view = entity.GetValue<VMovableView>();
				view?.Component.Recycle();
				entity.Destroy();
			}
		}
	}
}