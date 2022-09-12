using System.Threading.Tasks;
using Asteroids.Client.GameStates;
using FlyLib.Core.SimpleStateMachine.Contracts;
using FlyLib.Core.Ui.ScreenManager.Contracts;
using FlyLib.Core.Ui.ScreenManager.Controlls;

namespace Asteroids.Client.Ui
{
	public class StartScreen : UIScreen<StartScreen.Args>
	{
		public class Args : IScreenArgs
		{
			public int Scores { get; }

			public Args(int scores)
			{
				Scores = scores;
			}
		}

		private readonly StartView _startView;
		private readonly ISimpleStateMachine _simpleStateMachine;

		public StartScreen(StartView startView, ISimpleStateMachine simpleStateMachine)
		{
			_startView = startView;
			_simpleStateMachine = simpleStateMachine;
			_startView.StartClick = () => { StartGame().Wait(); };
		}

		protected override Task ShowAsync(Args args)
		{
			if (args != null)
			{
				_startView.SetHiScoresText($"Scores : {args.Scores}");
				_startView.SetButtonText("Restart");
				_startView.SetHiScoresActive(true);
			}
			else
			{
				_startView.SetButtonText("Start");
				_startView.SetHiScoresActive(false);
			}

			return _startView.ShowAsync();
		}

		protected override Task HideAsync()
		{
			return _startView.HideAsync();
		}

		private async Task StartGame()
		{
			await _simpleStateMachine.EnterAsync<PlayGameState>();
		}
	}
}