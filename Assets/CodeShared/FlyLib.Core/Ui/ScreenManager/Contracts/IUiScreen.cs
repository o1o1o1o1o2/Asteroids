using System.Threading.Tasks;

namespace FlyLib.Core.Ui.ScreenManager.Contracts
{
	public interface IUIScreenBase
	{
		Task HideScreenAsync();
	}
	
	public interface IUIScreen: IUIScreenBase
	{
		Task ShowScreenAsync();
	}
	
	public interface IUIScreen<in TArgs>: IUIScreenBase 
		where TArgs: IScreenArgs 
	{
		Task ShowScreenAsync(TArgs args);
	}
}