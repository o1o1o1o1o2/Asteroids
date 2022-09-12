using SimpleEcs.Components;

namespace SimpleEcs.Contracts
{
	public delegate void EntityEvent(IEntity entity);

	public delegate void EntityComponentChanged(IEntity entity, IComponent component);

	public interface IEntity
	{
		public int Id { get; }
	}
}