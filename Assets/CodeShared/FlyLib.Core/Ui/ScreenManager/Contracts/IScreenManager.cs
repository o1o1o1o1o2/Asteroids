using System;
using System.Threading.Tasks;

namespace FlyLib.Core.Ui.ScreenManager.Contracts
{
	public interface IScreenManager
	{
		public IUIScreenBase TopScreen { get; }
		void Register(params IUIScreenBase[] screens);
		Task ShowAsync(Type screenType);
		public Task ShowAsync<T>(Type screenType, T args) where T : IScreenArgs;
		Task HideAsync(Type screenType);
		public Task HideAllScreensAsync();
	}
}