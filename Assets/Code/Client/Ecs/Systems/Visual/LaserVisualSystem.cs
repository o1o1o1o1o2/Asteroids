using Asteroids.Client.Ecs.Components;
using Asteroids.Client.Ecs.Components.Visual;
using SimpleEcs.Contracts;

namespace Asteroids.Client.Ecs.Systems.Visual
{
	public class LaserVisualSystem : IExecuteSystem
	{
		private readonly IFilteredGroup _playerGroup;
		private readonly IFilteredGroup _laserGroup;

		public LaserVisualSystem(IGameContext gameContext)
		{
			_playerGroup = gameContext.AllOf<CPlayerTag, VPlayerView>();
			_laserGroup = gameContext.AllOf<CLaserShootLeftSec>();
		}

		public void Execute(float deltaTime)
		{
			foreach (var entity in _playerGroup)
			{
				var view = entity.GetValue<VPlayerView>();
				view.SetLaserActive(_laserGroup.Count > 0);
			}
		}
	}
}