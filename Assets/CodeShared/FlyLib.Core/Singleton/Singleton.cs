using UnityEngine;

namespace FlyLib.Core.Singleton
{
	public class Singleton<T> : MonoBehaviour where T : Component
	{
		private static T _instance;

		private static readonly object _lock = new();

		public static T Instance
		{
			get
			{
				lock (_lock)
				{
					if (_instance != null)
					{
						return _instance;
					}

					var instances = FindObjectsOfType<T>();

					if (instances.Length != 0)
					{
						_instance = instances[0];
					}

					if (instances.Length > 1)
					{
						Debug.LogError($"There is more than one {typeof(T).Name} in the scene, destroying");

						for (var i = 1; i < instances.Length; i++)
						{
							Destroy(instances[i]);
						}
					}

					if (_instance != null)
					{
						return _instance;
					}

					Debug.Log($"Singleton not found  {typeof(T)} Creating new");

					_instance = new GameObject(typeof(T).Name).AddComponent<T>();

					return _instance;
				}
			}
		}
	}
}