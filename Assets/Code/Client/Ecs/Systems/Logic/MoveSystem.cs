using Asteroids.Client.Ecs.Components;
using SimpleEcs.Contracts;
using SimpleEcs.Public;

namespace Asteroids.Client.Ecs.Systems.Logic
{
	public class MoveSystem : IExecuteSystem
	{
		private readonly IFilteredGroup _movables;

		public MoveSystem(GameContext gameContext)
		{
			_movables = gameContext.AllOf<CPosition, CVelocity>();
		}

		public void Execute(float deltaTime)
		{
			foreach (var entity in _movables)
			{
				entity.Set<CPosition>(entity.GetValue<CPosition>() + entity.GetValue<CVelocity>() * deltaTime);
			}
		}
	}
}