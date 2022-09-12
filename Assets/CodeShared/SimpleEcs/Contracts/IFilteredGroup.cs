using System;
using System.Collections.Generic;
using SimpleEcs.Components;
using SimpleEcs.Public;

namespace SimpleEcs.Contracts
{
	public delegate void GroupChanged(IFilteredGroup filterGroup, Entity entity, IComponent component);
	public delegate void EntityChanged(Entity entity);

	public interface IFilteredGroup : IEnumerable<Entity>
	{
		void Clear();
		int Count { get; }
		event GroupChanged OnEntityAdded;
		event GroupChanged OnEntityRemoved;
		event GroupChanged OnEntityUpdated;
		event Action OnAddedOrUpdated;
		IComponentsFilter ComponentsFilter { get; }
		GroupChanged HandleEntityAddOrRemove(Entity entity);
		void TryFireEntityUpdatedEvent(Entity entity, IComponent newComponent);
		Entity[] CachedArray();
	}
}