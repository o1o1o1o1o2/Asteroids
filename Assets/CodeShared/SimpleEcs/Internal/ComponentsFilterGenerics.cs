using System;
using System.Collections.Generic;
using FlyLib.Core.Utils;
using SimpleEcs.Components;
using SimpleEcs.Contracts;

namespace SimpleEcs.Internal
{
	internal class ComponentsFilter<T1> where T1 : IComponent
	{
		private static IComponentsFilterAllOf _all;

		public static IComponentsFilterAllOf All()
		{
			return _all ??= new ComponentsFilter(new HashSet<Type> { TypeOf<T1>.Raw });
		}
	}

	internal class ComponentsFilter<T1, T2>
		where T1 : IComponent
		where T2 : IComponent
	{
		private static IComponentsFilterAllOf _all;

		public static IComponentsFilterAllOf All()
		{
			return _all ??= new ComponentsFilter(new HashSet<Type> { TypeOf<T1>.Raw, TypeOf<T2>.Raw });
		}
	}

	internal class ComponentsFilter<T1, T2, T3>
		where T1 : IComponent
		where T2 : IComponent
		where T3 : IComponent
	{
		private static IComponentsFilterAllOf _all;

		public static IComponentsFilterAllOf All()
		{
			return _all ??= new ComponentsFilter(new HashSet<Type> { TypeOf<T1>.Raw, TypeOf<T2>.Raw, TypeOf<T3>.Raw });
		}
	}

	internal class ComponentsFilter<T1, T2, T3, T4>
		where T1 : IComponent
		where T2 : IComponent
		where T3 : IComponent
		where T4 : IComponent
	{
		private static IComponentsFilterAllOf _all;

		public static IComponentsFilterAllOf All()
		{
			return _all ??= new ComponentsFilter(new HashSet<Type>
			{
				TypeOf<T1>.Raw,
				TypeOf<T2>.Raw,
				TypeOf<T3>.Raw,
				TypeOf<T4>.Raw
			});
		}
	}
}