using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Asteroids.Client.Db.ConfigsScriptableObjects;
using Asteroids.Client.Ecs;
using Asteroids.Client.GameStates;
using Asteroids.Client.SceneViews;
using FlyLib.Core.AssetProvider.Contracts;
using FlyLib.Core.AssetProvider.Controls;
using FlyLib.Core.GameConfigs.Contracts;
using FlyLib.Core.GameConfigs.Controls;
using FlyLib.Core.SimpleStateMachine.Contracts;
using FlyLib.Core.SimpleStateMachine.Controls;
using FlyLib.Core.Ui.ScreenManager.Contracts;
using FlyLib.Core.Ui.ScreenManager.Controlls;
using InputManger;
using SimpleEcs.Contracts;
using SimpleEcs.Public;
using SimpleEcs.Tickables.Contracts;
using SimpleEcs.Tickables.Controls;
using SimpleEcs.Tickables.Scene;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Asteroids.Client
{
	[SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
	public class RootInstaller
	{
		public IGameConfigs GameConfigs { get; private set; }
		public GameContext GameContext { get; private set; }
		public IEcsRunner EcsRunner { get; private set; }
		public IAssetProvider AssetProvider { get; private set; }
		public ITickableManager TickableManager { get; private set; }
		public InputActions InputActions { get; private set; }
		public IScreenManager ScreenManager { get; private set; }
		public ISimpleStateMachine GameStateMachine { get; private set; }

		public async Task InitAsync(IEnumerable<SceneInstaller> sceneInstallers)
		{
			await Task.Yield();
			GameStateMachine = new SimpleStateMachine();
			ScreenManager = new ScreenManager();
			InputActions = new InputActions();
			Application.targetFrameRate = -1;
			QualitySettings.vSyncCount = 2;

			GameContext = new GameContext();
			EcsRunner = new EcsRunner();

#if UNITY_EDITOR
			AssetProvider = new EditorAssetProvider();
#else
			AssetProvider = new AddressablesAssetProvider();
#endif
			GameConfigs = new GameConfigs(AssetProvider);

			var ecsInstaller = new EcsInstaller(this);
			await ecsInstaller.InitAsync();

			TickableManager = new TickableManager();
			var tickablePrefab = await AssetProvider.LoadAsync<GameObject>(GameConfigs.GetConfig<MainConfig>().TickableManagerSceneObject);
			if (tickablePrefab == null)
			{
				throw new Exception(
					$"Cannot find tickable Prefab with name:{GameConfigs.GetConfig<MainConfig>().TickableManagerSceneObject}");
			}

			Object.Instantiate(tickablePrefab).GetComponent<TickAbleManagerSceneObject>().Initialize(TickableManager);
			TickableManager.AddToTickAbles(EcsRunner);

			foreach (var sceneInstaller in sceneInstallers)
			{
				sceneInstaller.Construct(this);
			}

			InstallStates();
			
			await ScreenManager.HideAllScreensAsync();
			await Task.Yield();
			await GameStateMachine.EnterAsync<MainMenuState>();
		}

		private void InstallStates()
		{
			GameStateMachine.Register(new MainMenuState(ScreenManager),
				new PlayGameState(GameContext, EcsRunner, ScreenManager));
		}
	}
}