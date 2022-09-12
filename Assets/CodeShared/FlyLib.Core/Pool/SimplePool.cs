using System.Collections.Generic;
using FlyLib.Core.Singleton;
using UnityEngine;

namespace FlyLib.Core.Pool
{
	public class SimplePool : Singleton<SimplePool>
	{
		private const string PoolSuffix = "Pool";

		private readonly Dictionary<string, Transform> _defaultParents = new();
		private readonly Dictionary<string, Queue<Component>> _poolDictionary = new();

		public T Spawn<T>(T component, Transform parent = null, Vector3 position = default, Quaternion rotation = default) where T : Component
		{
			var poolName = $"{component.name}{PoolSuffix}";
			if (!_poolDictionary.ContainsKey(component.name))
			{
				_poolDictionary.Add(component.name, new Queue<Component>());
				parent ??= new GameObject(poolName) { transform = { parent = transform } }.transform;
				_defaultParents.Add(poolName, parent.transform);
			}
			else
			{
				parent ??= _defaultParents[poolName];
			}

			var compQueue = _poolDictionary[component.name];
			if (compQueue.Count == 0)
			{
				return SpawnMore(component, parent, position, rotation);
			}

			T compT;
			while (true)
			{
				var comp = compQueue.Dequeue();
				compT = comp as T;
				if (compT != null)
				{
					break;
				}

				comp.gameObject.TryGetComponent(out compT);
				if (compT != null)
				{
					break;
				}

				Destroy(comp.gameObject);
				if (compQueue.Count == 0)
				{
					return SpawnMore(component, parent, position, rotation);
				}
			}

			compT.gameObject.SetActive(true);
			compT.gameObject.transform.SetParent(parent);
			var poolable = compT as IPoolable;
			poolable?.OnSpawn();
			return compT;
		}

		private T SpawnMore<T>(T component, Transform parent, Vector3 position, Quaternion rotation) where T : Component
		{
			var compInstance = Instantiate(component, position, rotation, parent).GetComponent<T>();
			compInstance.name = component.name;
			var poolable = compInstance as IPoolable;
			poolable?.OnSpawn();
			return compInstance;
		}

		public void Recycle(Component component)
		{
			if (component == null)
			{
				Debug.LogWarning("Recycle component is null!");
				return;
			}

			var poolable = component as IPoolable;
			poolable?.OnDespawn();
			component.gameObject.SetActive(false);
			_poolDictionary[component.name].Enqueue(component);
		}
	}
}