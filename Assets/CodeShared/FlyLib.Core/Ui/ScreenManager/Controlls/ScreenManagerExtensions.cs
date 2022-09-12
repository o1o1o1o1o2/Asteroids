using System.Threading.Tasks;
using FlyLib.Core.Ui.ScreenManager.Contracts;
using FlyLib.Core.Utils;

namespace FlyLib.Core.Ui.ScreenManager.Controlls
{
	public static class ScreenManagerExtensions
	{
		public static bool IsOnTop<T>(this IScreenManager manager)
			where T : IUIScreenBase
		{
			return manager.TopScreen is T;
		}
		
		public static Task ShowAsync<T>(this IScreenManager manager)
			where T : IUIScreen
		{
			return manager.ShowAsync(TypeOf<T>.Raw);
		}

		public static Task ShowAsync<T, TArgs>(this IScreenManager manager, TArgs args)
			where T : IUIScreen<TArgs>
			where TArgs : IScreenArgs
		{
			return manager.ShowAsync(TypeOf<T>.Raw, args);
		}

		public static Task HideAsync<T>(this IScreenManager manager)
			where T : IUIScreenBase
		{
			return manager.HideAsync(TypeOf<T>.Raw);
		}
	}
}