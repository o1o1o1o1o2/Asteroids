using UnityEngine;

namespace Asteroids.Client.Ecs.Components.Interfaces
{
	public interface IPlayerView
	{
		Vector3 BulletSpawnPoint { get; }
		void SetAccelerated(bool accelerated);
	}
}