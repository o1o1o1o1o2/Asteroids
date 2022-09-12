using FlyLib.Core.GameConfigs.Controls;

namespace FlyLib.Core.GameConfigs.Contracts
{
	public interface IGameConfigs
	{
		T GetConfig<T>() where T : GameConfigUniq;
	}
}