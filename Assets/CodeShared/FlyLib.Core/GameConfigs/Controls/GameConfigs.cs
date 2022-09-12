using System.Linq;
using FlyLib.Core.AssetProvider.Contracts;
using FlyLib.Core.GameConfigs.Contracts;
using FlyLib.Core.Utils;

namespace FlyLib.Core.GameConfigs.Controls
{
	public class GameConfigs : IGameConfigs
	{
		private readonly GameConfigUniq[] _configs;

		public GameConfigs(IAssetProvider assetProvider)
		{
			_configs = assetProvider.LoadByLabelAsync<GameConfigUniq>("GameConfigs").Result.Select(x => x).ToArray();
		}

		public T GetConfig<T>() where T : GameConfigUniq
		{
			return (T)_configs.FirstOrDefault(x => x.GetType() == TypeOf<T>.Raw);
		}
	}
}