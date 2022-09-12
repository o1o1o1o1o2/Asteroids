using System.Linq;
using Asteroids.Client.Ecs.Components;
using SimpleEcs.Contracts;
using SimpleEcs.Public;

namespace Asteroids.Client.Ecs.Systems.Logic
{
	public class HiScoreSystem : IExecuteSystem
	{
		private readonly IFilteredGroup _enemiesCollideGroup;
		private readonly IFilteredGroup _scoresGroup;

		public HiScoreSystem(GameContext gameContext)
		{
			_enemiesCollideGroup = gameContext.AllOf<CEnemyDef, CCollidedTag>();
			_scoresGroup = gameContext.AllOf<CScores>();
		}

		public void Execute(float deltaTime)
		{
			if (_enemiesCollideGroup.Count == 0)
			{
				return;
			}

			var scoresE = _scoresGroup.FirstOrDefault();
			foreach (var entity in _enemiesCollideGroup)
			{
				var score = entity.GetValue<CEnemyDef>().ScoresOnDestroy;
				scoresE?.Set<CScores>(scoresE.GetValue<CScores>() + score);
			}
		}
	}
}