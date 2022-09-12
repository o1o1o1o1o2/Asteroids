using Asteroids.Client.Db.ProjectileTypes;
using UnityEngine;

namespace Asteroids.Client.Types
{
	public struct ShootInfo
	{
		public Vector3 SpawnPoint { get; set; }
		public Vector3 ShootDirection { get; set; }
		public Vector3 Velocity { get; set; }
		public ProjectileDefinition ProjectileDefinition { get; set; }
	}
}