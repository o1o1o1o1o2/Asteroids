using System;
using System.Linq;
using Asteroids.Client.Db.ConfigsScriptableObjects;
using Asteroids.Client.Ecs.Components;
using FlyLib.Core.GameConfigs.Contracts;
using SimpleEcs.Contracts;
using SimpleEcs.Public;
using UnityEngine;

namespace Asteroids.Client.Ecs.Systems.Logic
{
	public class LaserSystem : IExecuteSystem, IInitializeSystem
	{
		private readonly GameContext _gameContext;
		private readonly IFilteredGroup _laserCooldownGroup;
		private readonly IFilteredGroup _laserShootingGroup;
		private readonly PlayerConfig _playerConfig;

		public LaserSystem(GameContext gameContext, IGameConfigs gameConfigs)
		{
			_gameContext = gameContext;
			_playerConfig = gameConfigs.GetConfig<PlayerConfig>();
			_laserCooldownGroup = gameContext.AllOf<CLaserTag, CLaserShotCount, CLaserCoolDownSec>();
			_laserShootingGroup = gameContext.AllOf<CLaserTag, CLaserShootLeftSec>();
		}

		public void Initialize()
		{
			var laserE = _gameContext.CreateEntity();
			laserE.SetTag<CLaserTag>(true);
			laserE.Set<CLaserCoolDownSec>(0);
			laserE.Set<CLaserShotCount>(_playerConfig.LaserMaxShots);
		}

		public void Execute(float deltaTime)
		{
			HandleShootingTime(deltaTime);
			HandleLaserCount(deltaTime);
		}

		private void HandleLaserCount(float deltaTime)
		{
			var laserE = _laserCooldownGroup.FirstOrDefault();

			if (laserE == null)
			{
				return;
			}

			var timer = laserE.GetValue<CLaserCoolDownSec>();
			if (timer < _playerConfig.LaserCooldownSec)
			{
				timer = Mathf.Clamp(timer + deltaTime, 0, _playerConfig.LaserCooldownSec);
				laserE.Set<CLaserCoolDownSec>(timer);
			}

			var currShots = laserE.GetValue<CLaserShotCount>();
			if (currShots == _playerConfig.LaserMaxShots)
			{
				return;
			}

			if (Math.Abs(timer - _playerConfig.LaserCooldownSec) > float.Epsilon)
			{
				return;
			}

			laserE.Set<CLaserShotCount>(currShots + 1);
			laserE.Set<CLaserCoolDownSec>(0f);
		}

		private void HandleShootingTime(float deltaTime)
		{
			var laserShootE = _laserShootingGroup.FirstOrDefault();

			if (laserShootE == null)
			{
				return;
			}

			var timeLeft = laserShootE.GetValue<CLaserShootLeftSec>();
			timeLeft -= deltaTime;

			if (timeLeft <= 0)
			{
				laserShootE.Remove<CLaserShootLeftSec>();
			}
			else
			{
				laserShootE.Set<CLaserShootLeftSec>(timeLeft);
			}
		}
	}
}