using System.Threading.Tasks;
using FlyLib.Core.Ui.ScreenManager.Contracts;

namespace FlyLib.Core.Ui.ScreenManager.Controlls
{
	public abstract class UIScreen : IUIScreen
	{
		public Task ShowScreenAsync()
		{
			return ShowAsync();
		}

		public Task HideScreenAsync()
		{
			return HideAsync();
		}

		protected abstract Task ShowAsync();
		protected abstract Task HideAsync();
	}
}