using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlyLib.Core.Ui.ScreenManager.Contracts;
using UnityEngine;

namespace FlyLib.Core.Ui.ScreenManager.Controlls
{
	public class ScreenManager : IScreenManager
	{
		private readonly List<IUIScreenBase> _screens = new(16);
		private readonly List<IUIScreenBase> _screenStack = new(16);

		public IUIScreenBase TopScreen => _screenStack.Count > 0 ? _screenStack.Last() : null;

		public void Register(params IUIScreenBase[] screens)
		{
			AddScreens(screens);
		}

		private void AddScreens(IEnumerable<IUIScreenBase> screens)
		{
			foreach (var screen in screens)
			{
				if (screen == null)
				{
					continue;
				}

				if (_screens.Contains(screen))
				{
					continue;
				}

				_screens.Add(screen);
			}
		}

		private IUIScreenBase GetScreen(Type screenType)
		{
			return _screens.FirstOrDefault(screenType.IsInstanceOfType);
		}

		public async Task ShowAsync(Type screenType)
		{
			var screen = GetScreen(screenType);
			if (screen == null)
			{
				Debug.LogError($"Cannot find screen of type '{screenType}'");
				return;
			}

			if (_screenStack.Contains(screen))
			{
				Debug.LogError($"Screen {screenType} already in stack");
				return;
			}

			try
			{
				_screenStack.Add(screen);

				if (screen is not IUIScreen uiScreen)
				{
					throw new Exception($"Type {screenType} must be instance of {nameof(IUIScreen)}");
				}

				await uiScreen.ShowScreenAsync();
			}
			catch (Exception)
			{
				_screenStack.Remove(screen);
				throw;
			}
		}

		public async Task ShowAsync<T>(Type screenType, T args) where T : IScreenArgs
		{
			var screen = GetScreen(screenType);
			if (screen == null)
			{
				Debug.LogError($"Cannot find screen of type '{screenType}'");
				return;
			}

			if (_screenStack.Contains(screen))
			{
				Debug.LogError($"Screen {screenType} already in stack");
				return;
			}

			try
			{
				_screenStack.Add(screen);

				if (screen is not IUIScreen<T> uiScreen)
				{
					throw new Exception($"Type {screenType} must be instance of {nameof(IUIScreen<T>)}");
				}

				await uiScreen.ShowScreenAsync(args);
			}
			catch (Exception)
			{
				_screenStack.Remove(screen);
				throw;
			}
		}

		public async Task HideAsync(Type screenType)
		{
			var screen = GetScreen(screenType);
			if (screen == null)
			{
				Debug.LogError($"Cannot find screen of type '{screenType}'");
				return;
			}

			var stackIndex = _screenStack.IndexOf(screen);
			if (stackIndex < 0)
			{
				Debug.LogWarning($"Cannot hide '{screenType}' - view is not visible! {this}");
				return;
			}

			_screenStack.RemoveAt(stackIndex);
			await screen.HideScreenAsync();
		}

		public async Task HideAllScreensAsync()
		{
			foreach (var screen in _screenStack)
			{
				await screen.HideScreenAsync();
			}

			_screenStack.Clear();
		}
	}
}