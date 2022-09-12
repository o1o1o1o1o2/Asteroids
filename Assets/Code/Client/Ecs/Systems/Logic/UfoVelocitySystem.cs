using System.Linq;
using Asteroids.Client.Db.ConfigsScriptableObjects;
using Asteroids.Client.Ecs.Components;
using Asteroids.Client.Types;
using FlyLib.Core.GameConfigs.Contracts;
using SimpleEcs.Contracts;
using SimpleEcs.Public;
using UnityEngine;

namespace Asteroids.Client.Ecs.Systems.Logic
{
	public class UfoVelocitySystem : IExecuteSystem
	{
		private readonly IFilteredGroup _enemiesGroup;
		private readonly IFilteredGroup _playersGroup;
		private readonly GamePlayConfig _gamePlayConfig;
		private Rect _camRect;

		public UfoVelocitySystem(GameContext gameContext, IGameConfigs gameConfigs)
		{
			_enemiesGroup = gameContext.AllOf<CEnemyDef>();
			_playersGroup = gameContext.AllOf<CPlayerTag>();
			_gamePlayConfig = gameConfigs.GetConfig<GamePlayConfig>();
			_camRect = Camera.main.GetCamWorldRect();
		}

		public void Execute(float deltaTime)
		{
			foreach (var enemyE in _enemiesGroup)
			{
				if (enemyE.GetValue<CEnemyDef>() != _gamePlayConfig.Ufo)
				{
					continue;
				}

				var nearestPlayer = _playersGroup.FirstOrDefault(x => _camRect.Contains(x.GetValue<CPosition>()))?.GetValue<CPosition>();
				if (nearestPlayer == null)
				{
					return;
				}

				var ufoPos = enemyE.GetValue<CPosition>();
				enemyE.Set<CVelocity>((nearestPlayer.Value - ufoPos).normalized * _gamePlayConfig.UfoSpeed);
			}
		}
	}
}