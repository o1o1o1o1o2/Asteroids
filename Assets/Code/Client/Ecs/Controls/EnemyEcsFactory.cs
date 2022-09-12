using Asteroids.Client.Db.EnemyTypes;
using Asteroids.Client.Ecs.Components;
using Asteroids.Client.Ecs.Contracts;
using SimpleEcs.Public;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.Client.Ecs.Controls
{
	public class EnemyEcsFactory : IEnemyEcsFactory
	{
		private readonly GameContext _gameContext;
		private readonly EnemyDefinition[] _asteroidTypes;
		

		public EnemyEcsFactory(GameContext gameContext)
		{
			_gameContext = gameContext;
		}

		public Entity CreateEnemyLogicEntity(EnemyDefinition enemyDef, Vector3 spawnPos, Vector3 velocity, Quaternion? rotation = null,
			float? angularSpeed = null)
		{
			var newEntity = _gameContext.CreateEntity();
			newEntity.SetTag<CDeSpawnableTag>(true);

			SetPosition(spawnPos, newEntity);

			SetRotation(newEntity, rotation);

			SetAngularSpeed(newEntity, angularSpeed);

			SetVelocity(velocity, newEntity);

			SetEnemyDefinition(enemyDef, newEntity);

			return newEntity;
		}

		private void SetEnemyDefinition(EnemyDefinition enemyDef, Entity newEntity)
		{
			newEntity.Set<CEnemyDef>(enemyDef);
		}

		private void SetVelocity(Vector3 velocityOverride, Entity newEntity)
		{
			newEntity.Set<CVelocity>(velocityOverride);
		}

		private void SetAngularSpeed(Entity newEntity, float? angularSpeed)
		{
			if (!angularSpeed.HasValue)
			{
				return;
			}

			newEntity.Set<CAngularSpeed>(angularSpeed.Value);
			newEntity.Set<CAngularVelocity>(Random.Range(0.0f, 1.0f) > 0.5f ? Vector3.forward : Vector3.back);
		}

		private static void SetRotation(Entity newEntity, Quaternion? rotation)
		{
			if (!rotation.HasValue)
			{
				return;
			}
			
			newEntity.Set<CRotation>(rotation.Value);
		}

		private void SetPosition(Vector3 spawnPos, Entity newEntity)
		{
			newEntity.Set<CPosition>(spawnPos);
		}
	}
}