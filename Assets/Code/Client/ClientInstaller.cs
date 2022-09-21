using Asteroids.Client.SceneViews;
using Asteroids.Client.Ui;
using FlyLib.Core.Ui.ScreenManager.SceneViews;
using UnityEngine;

namespace Asteroids.Client
{
	public class ClientInstaller : SceneInstaller
	{
		[SerializeField] private HudView _hudView;
		[SerializeField] private StartView _startView;

		protected override void Install()
		{
			foreach (var uiView in FindObjectsOfType<UiView>(true))
			{
				uiView.Construct();
			}
			var hudScreen = new HudScreen(_hudView, RootInstaller.GameContext, RootInstaller.GameConfigs);
			
			RootInstaller.ScreenManager.Register(hudScreen,
				new StartScreen(_startView, RootInstaller.GameStateMachine));

			RootInstaller.TickableManager.AddToTickAbles(hudScreen);
		}
	}
}