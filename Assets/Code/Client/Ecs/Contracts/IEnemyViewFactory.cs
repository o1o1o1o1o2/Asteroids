using SimpleEcs.Public;

namespace Asteroids.Client.Ecs.Contracts
{
	public interface IEnemyViewFactory
	{
		void CreateEnemyView(Entity logicEntity);
	}
}