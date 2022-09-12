using SimpleEcs.Components;
using SimpleEcs.Public;

namespace SimpleEcs.Contracts
{
	public interface IGameContext
	{
		public Entity CreateEntity();
		public void Reset();
		void DestroyEntity(Entity entity);
		public IFilteredGroup GetFilteredGroup(IComponentsFilter componentsFilter);

		public IFilteredGroup AllOf<T1>() where T1 : IComponent;
		public IFilteredGroup AllOf<T1, T2>() where T1 : IComponent where T2 : IComponent;
		public IFilteredGroup AllOf<T1, T2, T3>() where T1 : IComponent where T2 : IComponent where T3 : IComponent;
	}
}