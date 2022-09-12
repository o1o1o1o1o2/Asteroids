using System.Linq;
using Asteroids.Client.Db.ConfigsScriptableObjects;
using Asteroids.Client.Ecs.Components;
using FlyLib.Core.GameConfigs.Contracts;
using SimpleEcs.Contracts;
using SimpleEcs.Public;
using UnityEngine;

namespace Asteroids.Client.Ecs.Systems.Logic
{
	public class PlayerVelocitySystem : IExecuteSystem
	{
		private readonly IFilteredGroup _playerGroup;
		private readonly float _maxSpeed;
		private readonly PlayerConfig _playerConfig;

		public PlayerVelocitySystem(GameContext gameContext, IGameConfigs gameConfigs)
		{
			_playerGroup = gameContext.AllOf<CPlayerTag, CVelocity>();
			_playerConfig = gameConfigs.GetConfig<PlayerConfig>();
		}

		public void Execute(float deltaTime)
		{
			var playerE = _playerGroup.FirstOrDefault();
			if (playerE == null)
			{
				return;
			}
			
			Vector3 newVelocity;
			if (playerE.Has<CAccelerateTag>())
			{
				var forceDir = playerE.GetValue<CRotation>() * Vector3.up;

				newVelocity = playerE.GetValue<CVelocity>() + forceDir.normalized * (_playerConfig.AccelerationRate * deltaTime);
			}
			else
			{
				newVelocity = playerE.GetValue<CVelocity>() * Mathf.Pow(_playerConfig.DeAccelerationRate, deltaTime);
			}

			newVelocity = new Vector3(Mathf.Clamp(newVelocity.x, -_playerConfig.MaxSpeed, _playerConfig.MaxSpeed),
				Mathf.Clamp(newVelocity.y, -_playerConfig.MaxSpeed, _playerConfig.MaxSpeed),
				0.0f);

			foreach (var entity in _playerGroup)
			{
				entity.Set<CVelocity>(newVelocity);
			}
		}
	}
}