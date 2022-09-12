using System;
using System.Collections.Generic;
using SimpleEcs.Public;

namespace SimpleEcs.Contracts
{
	public interface IComponentsFilter
	{
		HashSet<Type> CompTypes { get; }
		bool Matches(Entity entity);
	}
}