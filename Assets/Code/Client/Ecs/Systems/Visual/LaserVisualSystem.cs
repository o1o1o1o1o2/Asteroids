using Asteroids.Client.Ecs.Components;
using Asteroids.Client.Ecs.Components.Visual;
using Asteroids.Client.Types;
using SimpleEcs.Contracts;
using UnityEngine;

namespace Asteroids.Client.Ecs.Systems.Visual
{
	public class LaserVisualSystem : IExecuteSystem
	{
		private readonly IFilteredGroup _playerGroup;
		private readonly IFilteredGroup _laserGroup;
		private readonly Vector2 _offset = new Vector2(30, 30);
		private Rect _camRect;

		public LaserVisualSystem(IGameContext gameContext)
		{
			_playerGroup = gameContext.AllOf<CPlayerTag, VPlayerView>();
			_laserGroup = gameContext.AllOf<CLaserShootLeftSec>();
			_camRect = Camera.main.GetCamWorldRect();
		}

		public void Execute(float deltaTime)
		{
			foreach (var entity in _playerGroup)
			{
				var view = entity.GetValue<VPlayerView>();
				var showLaser = _laserGroup.Count > 0 && _camRect.Overlaps(new Rect((Vector2)view.Position - _offset, _offset * 2));
				view.SetLaserActive(showLaser);
			}
		}
	}
}