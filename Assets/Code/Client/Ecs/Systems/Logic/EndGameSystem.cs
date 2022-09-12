using Asteroids.Client.Ecs.Components;
using Asteroids.Client.GameStates;
using FlyLib.Core.SimpleStateMachine.Contracts;
using SimpleEcs.Contracts;
using SimpleEcs.Public;

namespace Asteroids.Client.Ecs.Systems.Logic
{
	public class EndGameSystem : IExecuteSystem
	{
		private readonly ISimpleStateMachine _gameStateMachine;
		private readonly IFilteredGroup _playerCollidedGroup;

		public EndGameSystem(GameContext gameContext, ISimpleStateMachine gameStateMachine)
		{
			_gameStateMachine = gameStateMachine;
			_playerCollidedGroup = gameContext.AllOf<CCollidedWithPlayerTag>();
		}

		public void Execute(float deltaTime)
		{
			if (_playerCollidedGroup.Count > 0)
			{
				_gameStateMachine.EnterAsync<MainMenuState>();
			}
		}
	}
}