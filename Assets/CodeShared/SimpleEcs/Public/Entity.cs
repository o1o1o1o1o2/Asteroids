using System;
using System.Collections.Generic;
using System.Linq;
using FlyLib.Core.Utils;
using SimpleEcs.Components;
using SimpleEcs.Contracts;

namespace SimpleEcs.Public
{
	public partial class Entity : IEntity
	{
		public int Id { get; private set; }

		private readonly Dictionary<Type, IComponent> _components = new();

		public event EntityComponentChanged OnComponentAdded;
		public event EntityComponentChanged OnComponentRemoved;
		public event EntityComponentChanged OnComponentReplaced;
		public event EntityEvent OnEntityDestroy;

		public void InitEntity(int id)
		{
			Id = id;
		}

		public void Destroy()
		{
			OnEntityDestroy?.Invoke(this);
		}

		public void CleanAfterDestroy()
		{
			OnComponentAdded = null;
			OnComponentRemoved = null;
			OnComponentReplaced = null;
			OnEntityDestroy = null;
		}

		public T TryGetOrAdd<T>() where T : IComponent, new()
		{
			T component;

			var compType = TypeOf<T>.Raw;
			if (_components.ContainsKey(compType))
			{
				component = (T)_components[compType];
			}
			else
			{
				component = CreateComponent<T>();
				AddComponent(component, compType);
			}

			return component;
		}

		private T CreateComponent<T>() where T : new()
		{
			return new T();
		}

		private void AddComponent(IComponent component, Type compType)
		{
			_components.Add(compType, component);
			OnComponentAdded?.Invoke(this, component);
		}

		public void Remove<T>() where T : IComponent
		{
			RemoveComponent(TypeOf<T>.Raw);
		}

		public void RemoveAllComponents()
		{
			var components = _components.Values.ToArray();
			_components.Clear();
			
			foreach (var component in components)
			{
				OnComponentRemoved?.Invoke(this, component);
			}
		}
		
		public void RemoveAllComponentsSilent()
		{
			_components.Clear();
		}

		private void RemoveComponent(Type compType)
		{
			if (!_components.ContainsKey(compType))
			{
				return;
			}

			var comp = _components[compType];
			_components.Remove(compType);
			OnComponentRemoved?.Invoke(this, comp);
		}

		public void SetTag<T>(bool exists) where T : class, IComponent, new()
		{
			if (exists)
			{
				TryGetOrAdd<T>();
			}
			else
			{
				Remove<T>();
			}
		}

		public void Set<T, TValue>(TValue value) where T : class, IValueComponent<TValue>, new()
		{
			var component = TryGetOrAdd<T>();
			component.Value = value;
			OnComponentReplaced?.Invoke(this, component);
		}

		public TValue Get<T, TValue>(TValue defaultVal = default) where T : class, IValueComponent<TValue>
		{
			var c = Get<T>();
			return c != null ? c.Value : defaultVal;
		}

		public T Get<T>() where T : IComponent
		{
			return (T)GetComponent(TypeOf<T>.Raw);
		}

		private IComponent GetComponent(Type compType)
		{
			_components.TryGetValue(compType, out var comp);
			return comp;
		}

		public bool Has<T>() where T : IComponent
		{
			return _components.ContainsKey(TypeOf<T>.Raw);
		}

		public bool HasComponents(HashSet<Type> compTypes)
		{
			return compTypes.IsSubsetOf(_components.Keys);
		}

		public void CommitChanges<T>() where T : IComponent, new()
		{
			CommitChanges(TypeOf<T>.Raw);
		}

		private void CommitChanges(Type compType)
		{
			OnComponentReplaced?.Invoke(this, GetComponent(compType));
		}
	}
}