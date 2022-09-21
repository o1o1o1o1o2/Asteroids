using UnityEngine;

namespace Asteroids.Client.Ecs.Components.Interfaces
{
	public interface IPlayerView
	{
		Vector3 Position { get; }
		Vector3 BulletSpawnPoint { get; }
		void SetAccelerated(bool accelerated);
		void SetLaserActive(bool active);
	}
}