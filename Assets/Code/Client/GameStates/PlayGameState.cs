using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asteroids.Client.Ecs.Components;
using Asteroids.Client.Ecs.Components.Visual;
using Asteroids.Client.Ui;
using FlyLib.Core.Pool;
using FlyLib.Core.SimpleStateMachine.Contracts;
using FlyLib.Core.Ui.ScreenManager.Contracts;
using FlyLib.Core.Ui.ScreenManager.Controlls;
using SimpleEcs.Contracts;
using SimpleEcs.Public;

namespace Asteroids.Client.GameStates
{
	public class PlayGameState : IGameState, IEnterState, IExitState
	{
		private readonly GameContext _gameContext;
		private readonly IEcsRunner _ecsRunner;
		private readonly IScreenManager _screenManager;

		public PlayGameState(GameContext gameContext, IEcsRunner ecsRunner, IScreenManager screenManager)
		{
			_gameContext = gameContext;
			_ecsRunner = ecsRunner;
			_screenManager = screenManager;
		}

		public async Task OnEnterAsync(CancellationToken ct)
		{
			var scoresE = _gameContext.CreateEntity();
			scoresE.Set<CScores>(0);
			await _screenManager.ShowAsync<HudScreen>();
			_ecsRunner.Start();
		}

		public async Task OnExitAsync(CancellationToken ct)
		{
			_ecsRunner.Stop();
			await _screenManager.HideAsync<HudScreen>();
			await _screenManager.ShowAsync<StartScreen, StartScreen.Args>(
				new StartScreen.Args(_gameContext.AllOf<CScores>().FirstOrDefault()?.GetValue<CScores>() ?? 0));
			Clear();
		}

		private void Clear()
		{
			foreach (var entity in _gameContext.AllOf<VMovableView>())
			{
				entity.GetValue<VMovableView>().Component.Recycle();
			}

			_gameContext.Reset();
		}
	}
}