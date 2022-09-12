using SimpleEcs.Components;
using SimpleEcs.Contracts;
using SimpleEcs.Internal;

namespace SimpleEcs.Public
{
	public partial class GameContext
	{
		public IFilteredGroup AllOf<T1>() where T1 : IComponent
		{
			return GetFilteredGroup(ComponentsFilter<T1>.All());
		}

		public IFilteredGroup AllOf<T1, T2>() where T1 : IComponent where T2 : IComponent
		{
			return GetFilteredGroup(ComponentsFilter<T1, T2>.All());
		}

		public IFilteredGroup AllOf<T1, T2, T3>() where T1 : IComponent where T2 : IComponent where T3 : IComponent
		{
			return GetFilteredGroup(ComponentsFilter<T1, T2, T3>.All());
		}
		
		public IFilteredGroup AllOf<T1, T2, T3, T4>() where T1 : IComponent where T2 : IComponent where T3 : IComponent where T4 : IComponent
		{
			return GetFilteredGroup(ComponentsFilter<T1, T2, T3, T4>.All());
		}
	}
}