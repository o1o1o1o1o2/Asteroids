using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleEcs.Components;
using SimpleEcs.Contracts;
using SimpleEcs.Public;

namespace SimpleEcs.Internal
{
	internal class FilteredGroup : IFilteredGroup
	{
		public int Count => _entities.Count;

		public event GroupChanged OnEntityAdded;
		public event GroupChanged OnEntityRemoved;
		public event GroupChanged OnEntityUpdated;
		public event Action OnAddedOrUpdated;

		public IComponentsFilter ComponentsFilter { get; }

		private readonly HashSet<Entity> _entities = new();
		private Entity[] _entitiesCache;

		public FilteredGroup(IComponentsFilter componentsFilter)
		{
			ComponentsFilter = componentsFilter;
			OnEntityAdded += AddedOrUpdated;
			OnEntityUpdated += AddedOrUpdated;
		}

		private void AddedOrUpdated(IFilteredGroup filterGroup, Entity entity, IComponent component)
		{
			OnAddedOrUpdated?.Invoke();
		}

		public GroupChanged HandleEntityAddOrRemove(Entity entity)
		{
			return ComponentsFilter.Matches(entity)
				? AddEntityToGroup(entity)
					? OnEntityAdded
					: null
				: RemoveEntityFromGroup(entity)
					? OnEntityRemoved
					: null;
		}

		public void TryFireEntityUpdatedEvent(Entity entity, IComponent newComponent)
		{
			if (_entities.Contains(entity))
			{
				OnEntityUpdated?.Invoke(this, entity, newComponent);
			}
		}

		private bool AddEntityToGroup(Entity entity)
		{
			if (!_entities.Add(entity))
			{
				return false;
			}

			_entitiesCache = null;
			return true;
		}

		private bool RemoveEntityFromGroup(Entity entity)
		{
			if (!_entities.Remove(entity))
			{
				return false;
			}

			_entitiesCache = null;
			return true;
		}

		public Entity[] CachedArray()
		{
			return _entitiesCache ??= _entities.ToArray();
		}

		public IEnumerator<Entity> GetEnumerator()
		{
			var entities = (IEnumerable<Entity>)CachedArray();
			return entities.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		
		public void Clear()
		{
			_entities.Clear();
			_entitiesCache = null;
		}
	}
}