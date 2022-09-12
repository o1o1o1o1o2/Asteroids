using System.Collections.Generic;
using System.Linq;
using SimpleEcs.Components;
using SimpleEcs.Contracts;
using SimpleEcs.Internal;

namespace SimpleEcs.Public
{
	public partial class GameContext : IGameContext
	{
		private int _creationIdx = 0;
		private readonly HashSet<Entity> _entities = new();
		private readonly Dictionary<IComponentsFilter, IFilteredGroup> _groups = new();

		public Entity CreateEntity()
		{
			var newEntity = new Entity();
			_entities.Add(newEntity);
			newEntity.InitEntity(++_creationIdx);
			newEntity.OnComponentAdded += UpdateGroupsOnComponentAddedOrRemoved;
			newEntity.OnComponentRemoved += UpdateGroupsOnComponentAddedOrRemoved;
			newEntity.OnComponentReplaced += FireGroupsEventOnComponentReplaced;
			newEntity.OnEntityDestroy += e => DestroyEntity((Entity)e);
			return newEntity;
		}

		public void Reset()
		{
			foreach (var filteredGroup in _groups.Values)
			{
				filteredGroup.Clear();
			}

			foreach (var entity in _entities)
			{
				entity.RemoveAllComponentsSilent();
				entity.CleanAfterDestroy();
			}

			_entities.Clear();
			_creationIdx = 0;
		}

		public void DestroyEntity(Entity entity)
		{
			entity.RemoveAllComponents();
			entity.CleanAfterDestroy();
			_entities.Remove(entity);
		}

		public IFilteredGroup GetFilteredGroup(IComponentsFilter componentsFilter)
		{
			if (_groups.TryGetValue(componentsFilter, out var group))
			{
				return group;
			}

			var newGroup = new FilteredGroup(componentsFilter);

			foreach (var entity in _entities)
			{
				newGroup.HandleEntityAddOrRemove(entity);
			}

			_groups.Add(componentsFilter, newGroup);

			return newGroup;
		}

		private void UpdateGroupsOnComponentAddedOrRemoved(IEntity entity, IComponent newComponent)
		{
			var entityE = (Entity)entity;
			var events = _groups.Select(group => new { Event = group.Value.HandleEntityAddOrRemove(entityE), Group = group.Value })
				.ToList();

			foreach (var e in events)
			{
				e.Event?.Invoke(e.Group, entityE, newComponent);
			}
		}

		private void FireGroupsEventOnComponentReplaced(IEntity entity, IComponent newComponent)
		{
			if (_groups == null)
			{
				return;
			}

			var tEntity = (Entity)entity;

			foreach (var group in _groups)
			{
				group.Value.TryFireEntityUpdatedEvent(tEntity, newComponent);
			}
		}
	}
}