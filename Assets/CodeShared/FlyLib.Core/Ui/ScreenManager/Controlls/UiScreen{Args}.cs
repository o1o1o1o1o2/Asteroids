using System.Threading.Tasks;
using FlyLib.Core.Ui.ScreenManager.Contracts;

namespace FlyLib.Core.Ui.ScreenManager.Controlls
{
	public abstract class UIScreen<TArgs> : IUIScreen<TArgs> where TArgs : IScreenArgs
	{
		public Task ShowScreenAsync(TArgs args)
		{
			return ShowAsync(args);
		}

		public Task HideScreenAsync()
		{
			return HideAsync();
		}

		protected abstract Task ShowAsync(TArgs args);
		protected abstract Task HideAsync();
	}
}