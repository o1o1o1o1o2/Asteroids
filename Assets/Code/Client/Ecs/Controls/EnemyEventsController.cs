using Asteroids.Client.Ecs.Components;
using Asteroids.Client.Ecs.Contracts;
using SimpleEcs.Public;

namespace Asteroids.Client.Ecs.Controls
{
	public class EnemyEventsController : IEventsController
	{
		public void OnTriggerEnter(Entity entity)
		{
			entity.SetTag<CCollidedTag>(true);
		}
		
		public void OnPlayerTriggerEnter(Entity entity)
		{
			entity.SetTag<CCollidedWithPlayerTag>(true);
		}
	}
}