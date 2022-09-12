using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlyLib.Core.AssetProvider.Contracts
{
	public interface IAssetProvider
	{
		 Task<T> LoadAsync<T>(string key) where T : class;
		 Task<IEnumerable<T>> LoadByLabelAsync<T>(string label) where T : class;
	}
}