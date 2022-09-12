using Asteroids.Client.Ecs.Components;
using Asteroids.Client.Ecs.Components.Visual;
using SimpleEcs.Contracts;

namespace Asteroids.Client.Ecs.Systems.Visual
{
	public class SyncMovementVisualSystem : IExecuteSystem
	{
		private readonly IFilteredGroup _movables;
		private readonly IFilteredGroup _rotatables;

		public SyncMovementVisualSystem(IGameContext gameContext)
		{
			_movables = gameContext.AllOf<CPosition, VMovableView>();
			_rotatables = gameContext.AllOf<CRotation, VMovableView>();
		}

		public void Execute(float deltaTime)
		{
			foreach (var entity in _movables)
			{
				var view = entity.GetValue<VMovableView>();
				view.SetPosition(entity.GetValue<CPosition>());
			}

			foreach (var entity in _rotatables)
			{
				var view = entity.GetValue<VMovableView>();
				view.SetRotation(entity.GetValue<CRotation>());
			}
		}
	}
}