using System.Threading;
using System.Threading.Tasks;
using Asteroids.Client.Ui;
using FlyLib.Core.SimpleStateMachine.Contracts;
using FlyLib.Core.Ui.ScreenManager.Contracts;
using FlyLib.Core.Ui.ScreenManager.Controlls;

namespace Asteroids.Client.GameStates
{
	public class MainMenuState : IGameState, IEnterState, IExitState
	{
		private readonly IScreenManager _screenManager;

		public MainMenuState(IScreenManager screenManager)
		{
			_screenManager = screenManager;
		}

		public async Task OnEnterAsync(CancellationToken ct)
		{
			if (!_screenManager.IsOnTop<StartScreen>())
			{
				await _screenManager.ShowAsync<StartScreen, StartScreen.Args>(null);
			}
		}

		public async Task OnExitAsync(CancellationToken ct)
		{
			await _screenManager.HideAsync<StartScreen>();
		}
	}
}