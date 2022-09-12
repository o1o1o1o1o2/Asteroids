using Asteroids.Client.SceneViews;
using Asteroids.Client.Ui;
using UnityEngine;

namespace Asteroids.Client
{
	public class ClientInstaller : SceneInstaller
	{
		[SerializeField] private HudView _hudView;
		[SerializeField] private StartView _startView;

		protected override void Install()
		{
			var hudScreen = new HudScreen(_hudView, RootInstaller.GameContext);
			
			RootInstaller.ScreenManager.Register(hudScreen,
				new StartScreen(_startView, RootInstaller.GameStateMachine));

			RootInstaller.TickableManager.AddToTickAbles(hudScreen);
		}
	}
}