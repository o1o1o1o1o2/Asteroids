using Asteroids.Client.Db.EnemyTypes;
using FlyLib.Core.GameConfigs.Controls;
using UnityEngine;

namespace Asteroids.Client.Db.ConfigsScriptableObjects
{
	[CreateAssetMenu(fileName = "GamePlayConfig", menuName = "GameConfigs/New GamePlayConfig", order = 51)]
	public class GamePlayConfig : GameConfigUniq
	{
		[field: SerializeField] public EnemyDefinition[] AsteroidEnemies { get; private set; }
		[field: SerializeField] public EnemyDefinition Ufo { get; private set; }

		[field: SerializeField, Range(0, 2)] public float AsteroidSpawnMinRangeSec { get; private set; } = 1.0f;
		[field: SerializeField, Range(2, 5)] public float AsteroidSpawnMaxRangeSec { get; private set; } = 2.0f;
		[field: SerializeField, Range(100, 300)] public float AsteroidMinSpeed { get; private set; } = 100.0f;
		[field: SerializeField, Range(300, 500)] public float AsteroidMaxSpeed { get; private set; } = 500.0f;
		[field: SerializeField, Range(10, 150)] public float AsteroidAngularSpeedMinRange { get; private set; } = 10.0f;
		[field: SerializeField, Range(150, 300)] public float AsteroidAngularSpeedMaxRange { get; private set; } = 300f;
		[field: SerializeField, Range(0, 5)] public float AsteroidFragmentIncreaseSpeedMult { get; private set; } = 2;
		[field: SerializeField, Range(0, 90)] public float AsteroidFragmentVelocityDeviation { get; private set; } = 30;

		[field: Space, SerializeField, Range(0, 5)] public float UfoSpawnMinRangeSec { get; private set; } = 5.0f;
		[field: SerializeField, Range(5, 10)] public float UfoSpawnMaxRangeSec { get; private set; } = 10.0f;
		[field: SerializeField, Range(100, 500)] public float UfoSpeed { get; private set; } = 200.0f;
	}
}