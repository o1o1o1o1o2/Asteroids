using Asteroids.Client.Db.ConfigsScriptableObjects;
using Asteroids.Client.Db.EnemyTypes;
using Asteroids.Client.Ecs.Components;
using Asteroids.Client.Ecs.Components.Visual;
using Asteroids.Client.Ecs.Contracts;
using Asteroids.Client.SceneViews;
using FlyLib.Core.GameConfigs.Contracts;
using FlyLib.Core.Pool;
using FlyLib.Core.SimpleStateMachine.Contracts;
using SimpleEcs.Contracts;
using SimpleEcs.Public;
using UnityEngine;

namespace Asteroids.Client.Ecs.Systems.Logic
{
	public class CollideSystem : IExecuteSystem
	{
		private readonly IEnemyEcsFactory _enemyEcsFactory;
		private readonly IEnemyViewFactory _enemyViewFactory;
		private readonly ISimpleStateMachine _gameStateMachine;
		private readonly IFilteredGroup _collidedGroup;
		private readonly GamePlayConfig _gamePlayConfig;

		public CollideSystem(GameContext gameContext, IEnemyEcsFactory enemyEcsFactory, IEnemyViewFactory enemyViewFactory, IGameConfigs gameConfigs)
		{
			_enemyEcsFactory = enemyEcsFactory;
			_enemyViewFactory = enemyViewFactory;
			_collidedGroup = gameContext.AllOf<CCollidedTag>();
			_gamePlayConfig = gameConfigs.GetConfig<GamePlayConfig>();
		}

		public void Execute(float deltaTime)
		{
			foreach (var entity in _collidedGroup)
			{
				TrySpawnAsteroidFragments(entity);

				var viewComp = entity.GetValue<VMovableView>()?.Component;
				if (viewComp != null)
				{
					if (viewComp is EnemyViewObject comp)
					{
						comp.DestroyEnemy();
					}
					else
					{
						viewComp.Recycle();
					}
				}

				entity.Destroy();
			}
		}

		private void TrySpawnAsteroidFragments(Entity entity)
		{
			var enemyType = entity.GetValue<CEnemyDef>() as AsteroidEnemy;
			if (enemyType == null || enemyType.SpawnOnDestroy == null)
			{
				return;
			}

			var originVelocity = entity.GetValue<CVelocity>();
			var spawnPos = entity.GetValue<CPosition>();
			for (var i = 0; i < 2; i++)
			{
				var velocity = Quaternion.AngleAxis(_gamePlayConfig.AsteroidFragmentVelocityDeviation * (i == 0 ? 1 : -1), Vector3.forward) *
					originVelocity * _gamePlayConfig.AsteroidFragmentIncreaseSpeedMult;
				var newEnemyE = _enemyEcsFactory.CreateEnemyLogicEntity(enemyType.SpawnOnDestroy, spawnPos, velocity: velocity);
				_enemyViewFactory.CreateEnemyView(newEnemyE);
			}
		}
	}
}