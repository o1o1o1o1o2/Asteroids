#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlyLib.Core.AssetProvider.Contracts;
using FlyLib.Core.Utils;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using Object = UnityEngine.Object;

namespace FlyLib.Core.AssetProvider.Controls
{
	public class EditorAssetProvider : IAssetProvider
	{
		private readonly List<AddressableAssetGroup> _groups;

		public EditorAssetProvider()
		{
			_groups = LoadAssets<AddressableAssetGroup>();
		}

		private static List<T> LoadAssets<T>(string name = "") where T : Object
		{
			var result = AssetDatabase.FindAssets($"t:{typeof(T).Name} {name}")
				.Select(AssetDatabase.GUIDToAssetPath)
				.Select(AssetDatabase.LoadAssetAtPath<T>);

			if (!string.IsNullOrEmpty(name))
			{
				result = result.Where(x => x.name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
			}

			return result.ToList();
		}

		public Task<T> LoadAsync<T>(string key) where T : class
		{
			return Task.FromResult(Load<T>(key));
		}

		private T Load<T>(string key) where T : class
		{
			if (!TypeOf<Object>.Raw.IsAssignableFrom(TypeOf<T>.Raw))
			{
				throw new Exception($"Error loading address='{key}' in editor mode: asset type ('{TypeOf<T>.Name}') must be derived from Unity.Object!");
			}

			foreach (var assetGroup in _groups)
			{
				foreach (var entry in assetGroup.entries)
				{
					if (entry.address.Equals(key, StringComparison.InvariantCultureIgnoreCase))
					{
						return LoadEntry<T>(entry);
					}
				}
			}

			throw new Exception($"Cannot find '{TypeOf<T>.Name}' with address='{key}'");
		}

		private static T LoadEntry<T>(AddressableAssetEntry entry) where T : class
		{
			var asset = AssetDatabase.LoadAssetAtPath<Object>(entry.AssetPath);
			if (asset == null)
			{
				throw new Exception($"Cannot load '{TypeOf<T>.Name}' with address='{entry.address}' path='{entry.AssetPath}': loaded null asset");
			}

			return asset as T ??
				throw new Exception(
					$"Cannot cast asset to type '{TypeOf<T>.Name}' with address='{entry.address}' path='{entry.AssetPath}' (real type is {asset.GetType().FullName})");
		}

		public Task<IEnumerable<T>> LoadByLabelAsync<T>(string label) where T : class
		{
			var result = _groups
				.SelectMany(x => x.entries)
				.Where(x => x.labels.Contains(label))
				.Select(LoadEntry<T>);
			return Task.FromResult(result);
		}
	}
}
#endif