using Asteroids.Client.Db.ProjectileTypes;
using FlyLib.Core.GameConfigs.Controls;
using UnityEngine;

namespace Asteroids.Client.Db.ConfigsScriptableObjects
{
	[CreateAssetMenu(fileName = "PlayerConfig", menuName = "GameConfigs/New PlayerConfig", order = 51)]
	public class PlayerConfig : GameConfigUniq
	{
		[field: Header("Movement")]
		[field: SerializeField, Range(0, 2000)]
		public float AccelerationRate { get; private set; } = 1000;

		[field: SerializeField, Range(0f, 1f)] public float DeAccelerationRate { get; private set; } = 0.3f;
		[field: SerializeField, Range(0, 2000)] public float MaxSpeed { get; private set; } = 1000;
		[field: SerializeField, Range(0, 1000)] public float RotationSpeed { get; private set; } = 300;

		[field: Header("Shooting")]
		[field: SerializeField]
		public ProjectileDefinition ProjectileDefinition { get; private set; }

		[field: SerializeField, Range(1500, 3000)] public float ProjectileSpeed { get; private set; } = 1000;
		
		[field: SerializeField, Range(5, 10)] public float LaserCooldownSec { get; private set; } = 5;
		[field: SerializeField, Range(1, 5)] public int LaserMaxShots { get; private set; } = 3;
		[field: SerializeField, Range(1, 5)] public float LaserShootDurationSec { get; private set; } = 2;
	}
}