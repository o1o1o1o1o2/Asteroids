using System;
using System.Collections.Generic;
using SimpleEcs.Contracts;
using SimpleEcs.Public;

namespace SimpleEcs.Internal
{
	internal class ComponentsFilter : IComponentsFilterAllOf
	{
		public HashSet<Type> CompTypes => AllOfCompTypes;
		private HashSet<Type> AllOfCompTypes { get; }

		public ComponentsFilter(HashSet<Type> allOfCompTypes)
		{
			AllOfCompTypes = allOfCompTypes;
		}

		public bool Matches(Entity entity)
		{
			return AllOfCompTypes == null || entity.HasComponents(AllOfCompTypes);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType())
			{
				return false;
			}

			return ((ComponentsFilter)obj).AllOfCompTypes.IsSubsetOf(AllOfCompTypes);
		}

		public override int GetHashCode()
		{
			return AllOfCompTypes != null ? AllOfCompTypes.GetHashCode() : 0;
		}
	}
}