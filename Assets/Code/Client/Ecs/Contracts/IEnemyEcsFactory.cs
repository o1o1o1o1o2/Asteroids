using Asteroids.Client.Db.EnemyTypes;
using SimpleEcs.Public;
using UnityEngine;

namespace Asteroids.Client.Ecs.Contracts
{
	public interface IEnemyEcsFactory
	{
		Entity CreateEnemyLogicEntity(EnemyDefinition enemyDef, Vector3 spawnPos, Vector3 velocity, Quaternion? rotation = null, float? angularSpeed = null);
	}
}