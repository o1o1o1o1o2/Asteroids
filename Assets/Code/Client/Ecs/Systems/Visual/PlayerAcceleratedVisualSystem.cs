using Asteroids.Client.Ecs.Components;
using Asteroids.Client.Ecs.Components.Visual;
using SimpleEcs.Contracts;

namespace Asteroids.Client.Ecs.Systems.Visual
{
	public class PlayerAcceleratedVisualSystem : IExecuteSystem
	{
		private readonly IFilteredGroup _playerGroup;

		public PlayerAcceleratedVisualSystem(IGameContext gameContext)
		{
			_playerGroup = gameContext.AllOf<CPlayerTag, VPlayerView>();
		}

		public void Execute(float deltaTime)
		{
			foreach (var entity in _playerGroup)
			{
				var view = entity.GetValue<VPlayerView>();
				view.SetAccelerated(entity.Has<CAccelerateTag>());
			}
		}
	}
}