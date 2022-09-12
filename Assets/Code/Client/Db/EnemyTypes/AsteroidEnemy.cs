using UnityEngine;

namespace Asteroids.Client.Db.EnemyTypes
{
	[CreateAssetMenu(fileName = "Asteroid", menuName = "GameConfigs/Enemies/Asteroid", order = 51)]
	public class AsteroidEnemy : EnemyDefinition
	{
		[field: SerializeField] public EnemyDefinition SpawnOnDestroy { get; private set; }
	}
}