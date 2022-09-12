using SimpleEcs.Public;

namespace Asteroids.Client.Ecs.Contracts
{
	public interface IEventsController
	{
		void OnTriggerEnter(Entity entity);
		void OnPlayerTriggerEnter(Entity entity);
	}
}