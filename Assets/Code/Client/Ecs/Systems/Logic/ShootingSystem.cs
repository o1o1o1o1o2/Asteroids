using Asteroids.Client.Ecs.Components;
using Asteroids.Client.Ecs.Contracts;
using FlyLib.Core.Pool;
using SimpleEcs.Contracts;
using SimpleEcs.Public;

namespace Asteroids.Client.Ecs.Systems.Logic
{
	public class ShootingSystem : IExecuteSystem
	{
		private readonly GameContext _gameContext;
		private readonly IEventsController _eventsController;
		private readonly IFilteredGroup _shootGroup;


		public ShootingSystem(GameContext gameContext,IEventsController eventsController)
		{
			_gameContext = gameContext;
			_eventsController = eventsController;
			_shootGroup = gameContext.AllOf<CShootInfo>();
		}

		public void Execute(float deltaTime)
		{
			foreach (var entity in _shootGroup)
			{
				var shootInfo = entity.GetValue<CShootInfo>();
				var newEntity = _gameContext.CreateEntity();
				newEntity.SetTag<CDeSpawnableTag>(true);
				newEntity.Set<CPosition>(shootInfo.SpawnPoint);
				newEntity.Set<CVelocity>(shootInfo.Velocity);

				var view = shootInfo.ProjectileDefinition.ProjectilePrefab.Spawn();
				view.AddToEntity(newEntity);
				view.TriggerEnter = () => _eventsController.OnTriggerEnter(newEntity);
				view.SetPosition(shootInfo.SpawnPoint);
				
				entity.Remove<CShootInfo>();
			}
		}
	}
}