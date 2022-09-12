using System.Threading.Tasks;
using Asteroids.Client.Ecs.Controls;
using Asteroids.Client.Ecs.Systems.Logic;
using Asteroids.Client.Ecs.Systems.Visual;

namespace Asteroids.Client.Ecs
{
	public class EcsInstaller
	{
		private readonly RootInstaller _rootInstaller;

		public EcsInstaller(RootInstaller rootInstaller)
		{
			_rootInstaller = rootInstaller;
		}

		public async Task InitAsync()
		{
			var gameContext = _rootInstaller.GameContext;
			var gameConfigs = _rootInstaller.GameConfigs;
			var eventsController = new EnemyEventsController();
			var enemyEcsFactory = new EnemyEcsFactory(gameContext);
			var enemyViewFactory = new EnemyViewFactory(gameConfigs, _rootInstaller.AssetProvider, eventsController);
			await enemyViewFactory.Init();

			_rootInstaller.EcsRunner.Register(
				//logic
				new EnemySpawnSystem(enemyEcsFactory, enemyViewFactory, gameConfigs),
				new PlayerSpawnSystem(gameContext, gameConfigs, _rootInstaller.AssetProvider, eventsController),
				new UfoVelocitySystem(gameContext, gameConfigs),
				new PlayerInputSystem(gameContext, gameConfigs, _rootInstaller.InputActions),
				new ShootingSystem(gameContext, eventsController),
				new PlayerVelocitySystem(gameContext, gameConfigs),
				new MoveSystem(gameContext),
				new EndGameSystem(gameContext, _rootInstaller.GameStateMachine),
				new HiScoreSystem(gameContext),
				new CollideSystem(gameContext, enemyEcsFactory, enemyViewFactory, gameConfigs),
				new RotateSystem(gameContext),
				new DespawnSystem(gameContext),

				//visual
				new PlayerAcceleratedVisualSystem(gameContext),
				new SyncMovementVisualSystem(gameContext));
		}
	}
}