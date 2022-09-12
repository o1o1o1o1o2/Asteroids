using Asteroids.Client.Db.ConfigsScriptableObjects;
using Asteroids.Client.Db.EnemyTypes;
using Asteroids.Client.Ecs.Contracts;
using Asteroids.Client.Types;
using FlyLib.Core.GameConfigs.Contracts;
using FlyLib.Core.Utils;
using SimpleEcs.Contracts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.Client.Ecs.Systems.Logic
{
	public class EnemySpawnSystem : IExecuteSystem
	{
		private readonly Vector2 _spawnRectOffset = new Vector2(200, 100);
		private readonly IEnemyEcsFactory _enemyEcsFactory;
		private readonly IEnemyViewFactory _enemyViewFactory;
		private readonly GamePlayConfig _gamePlayConfig;

		private readonly Rect _camRect;
		private readonly Rect _spawnRect;
		private float _asteroidSpawnTimer;
		private float _nextAsteroidSpawnTime;
		private float _ufoSpawnTimer;
		private float _nextUfoSpawnTime;
		private readonly EnemyDefinition[] _asteroidTypes;

		public EnemySpawnSystem(IEnemyEcsFactory enemyEcsFactory, IEnemyViewFactory enemyViewFactory, IGameConfigs gameConfigs)
		{
			_enemyEcsFactory = enemyEcsFactory;
			_enemyViewFactory = enemyViewFactory;
			_gamePlayConfig = gameConfigs.GetConfig<GamePlayConfig>();
			_asteroidTypes = _gamePlayConfig.AsteroidEnemies;

			_camRect = Camera.main.GetCamWorldRect();
			_spawnRect = new Rect(_camRect.position - _spawnRectOffset, _camRect.size + _spawnRectOffset * 2);
			_camRect = new Rect(_camRect.position + _spawnRectOffset, _camRect.size - _spawnRectOffset * 2);
			_nextUfoSpawnTime = Random.Range(_gamePlayConfig.UfoSpawnMinRangeSec, _gamePlayConfig.UfoSpawnMaxRangeSec);
		}

		public void Execute(float deltaTime)
		{
			_asteroidSpawnTimer += deltaTime;
			if (_asteroidSpawnTimer >= _nextAsteroidSpawnTime)
			{
				SpawnAsteroid();
				_asteroidSpawnTimer = 0f;
				_nextAsteroidSpawnTime = Random.Range(_gamePlayConfig.AsteroidSpawnMinRangeSec, _gamePlayConfig.AsteroidSpawnMaxRangeSec);
			}

			_ufoSpawnTimer += deltaTime;
			if (_ufoSpawnTimer < _nextUfoSpawnTime)
			{
				return;
			}

			SpawnUfo();
			_ufoSpawnTimer = 0f;
			_nextUfoSpawnTime = Random.Range(_gamePlayConfig.UfoSpawnMinRangeSec, _gamePlayConfig.UfoSpawnMaxRangeSec);
		}

		private void SpawnAsteroid()
		{
			var asteroidDef = (EnemyDefinition)_asteroidTypes.GetValue(Random.Range(0, _asteroidTypes.Length - 1));
			var position = RandomExt.GetRandomPositionAtTheEdgeOfRect(_spawnRect);
			var rotation = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward);
			var dirPos = RandomExt.GetRandomPositionAtTheEdgeOfRect(_camRect);
			var velocity = (dirPos - position).normalized * Random.Range(_gamePlayConfig.AsteroidMinSpeed, _gamePlayConfig.AsteroidMaxSpeed);
			var angularSpeed = Random.Range(_gamePlayConfig.AsteroidAngularSpeedMinRange, _gamePlayConfig.AsteroidAngularSpeedMaxRange);
			var newEnemyE = _enemyEcsFactory.CreateEnemyLogicEntity(asteroidDef, position, velocity, rotation: rotation, angularSpeed: angularSpeed);
			_enemyViewFactory.CreateEnemyView(newEnemyE);
		}

		private void SpawnUfo()
		{
			var spawnPos = RandomExt.GetRandomPositionAtTheEdgeOfRect(_spawnRect);
			var newEnemyE = _enemyEcsFactory.CreateEnemyLogicEntity(_gamePlayConfig.Ufo, spawnPos, Vector3.zero);
			_enemyViewFactory.CreateEnemyView(newEnemyE);
		}
	}
}