using System.Linq;
using Asteroids.Client.Db.ConfigsScriptableObjects;
using Asteroids.Client.Ecs.Components;
using Asteroids.Client.Ecs.Components.Visual;
using Asteroids.Client.Types;
using FlyLib.Core.GameConfigs.Contracts;
using InputManger;
using SimpleEcs.Contracts;
using SimpleEcs.Public;
using UnityEngine;

namespace Asteroids.Client.Ecs.Systems.Logic
{
	public class PlayerInputSystem : IExecuteSystem
	{
		private readonly InputActions _inputActions;
		private readonly IFilteredGroup _playerGroup;
		private readonly IFilteredGroup _laserGroup;

		private readonly PlayerConfig _playerConfig;
		private Rect _camRect;

		public PlayerInputSystem(GameContext gameContext, IGameConfigs gameConfigs, InputActions inputActions)
		{
			_inputActions = inputActions;
			_playerConfig = gameConfigs.GetConfig<PlayerConfig>();
			_playerGroup = gameContext.AllOf<CPlayerTag>();
			_laserGroup = gameContext.AllOf<CLaserTag, CLaserShotCount>();
			_inputActions.PlayerInput.Enable();

			_camRect = Camera.main.GetCamWorldRect();
		}

		public void Execute(float deltaTime)
		{
			var playerE = _playerGroup.FirstOrDefault();

			if (playerE == null)
			{
				return;
			}

			Move(playerE, _inputActions.PlayerInput.Movement.ReadValue<Vector2>(), deltaTime);

			if (_inputActions.PlayerInput.Shooting.WasPressedThisFrame())
			{
				ShootBullet();
			}

			if (_inputActions.PlayerInput.ShootLaser.WasPressedThisFrame())
			{
				ShootLaser();
			}
		}

		private void Move(Entity playerE, Vector2 movementVector, float deltaTime)
		{
			foreach (var entity in _playerGroup)
			{
				entity.SetTag<CAccelerateTag>(movementVector.y > 0);
			}

			switch (movementVector.x)
			{
				case < 0:
					Rotate(playerE, deltaTime, true);
					break;

				case > 0:
					Rotate(playerE, deltaTime, false);
					break;
			}
		}

		private void ShootBullet()
		{
			var pE = _playerGroup.FirstOrDefault(x => _camRect.Contains(x.GetValue<VPlayerView>().BulletSpawnPoint));
			if (pE == null)
			{
				return;
			}

			var shootDir = pE.GetValue<CRotation>() * Vector3.up;
			var shootInfo = new ShootInfo
			{
				ProjectileDefinition = _playerConfig.ProjectileDefinition,
				SpawnPoint = pE.GetValue<VPlayerView>().BulletSpawnPoint,
				ShootDirection = shootDir,
				Velocity = shootDir * _playerConfig.ProjectileSpeed + pE.GetValue<CVelocity>(),
			};
			pE.Set<CShootInfo>(shootInfo);
		}

		private void ShootLaser()
		{
			var laserE = _laserGroup.FirstOrDefault();

			if (laserE == null)
			{
				return;
			}

			if (laserE.Has<CLaserShootLeftSec>())
			{
				return;
			}

			var laserShootCount = laserE.GetValue<CLaserShotCount>();
			if (laserShootCount == 0)
			{
				return;
			}

			laserE.Set<CLaserShootLeftSec>(_playerConfig.LaserShootDurationSec);
			laserE.Set<CLaserShotCount>(laserShootCount - 1);
		}

		private void Rotate(Entity playerE, float deltaTime, bool isClockWise)
		{
			var prevRotZ = playerE.GetValue<CRotation>().eulerAngles.z;
			var newRotationZ = Quaternion.AngleAxis(prevRotZ + (isClockWise ? deltaTime : -deltaTime) * _playerConfig.RotationSpeed, Vector3.forward);
			foreach (var entity in _playerGroup)
			{
				entity.Set<CRotation>(newRotationZ);
			}
		}
	}
}