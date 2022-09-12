using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlyLib.Core.AssetProvider.Contracts;
using FlyLib.Core.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FlyLib.Core.AssetProvider.Controls
{
	public class AddressablesAssetProvider : IAssetProvider
	{
		public async Task<T> LoadAsync<T>(string key) where T : class
		{
			if (!TypeOf<UnityEngine.Object>.Raw.IsAssignableFrom(TypeOf<T>.Raw))
			{
				throw new Exception($"Error loading address='{key}' in editor mode: asset type ('{TypeOf<T>.Name}') must be derived from Unity.Object!");
			}

			var asyncOpHandle = Addressables.LoadAssetAsync<T>(key);
			await asyncOpHandle.Task;
			return asyncOpHandle.Result;
		}

		public async Task<IEnumerable<T>> LoadByLabelAsync<T>(string label) where T : class
		{
			if (string.IsNullOrEmpty(label))
			{
				throw new Exception("key is null or empty");
			}

			try
			{
				var asyncOperationHandle = Addressables.LoadAssetsAsync<T>(label, null);
				await asyncOperationHandle.Task;
				return asyncOperationHandle.Task.Result;
			}
			catch (Exception ex)
			{
				Debug.LogError($"Error loading assets by label '{label}': {ex.Message}");
				throw;
			}
		}
	}
}