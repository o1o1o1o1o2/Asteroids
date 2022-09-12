using System;
using System.Collections.Generic;

namespace FlyLib.Core.Utils
{
	public static class EnumerableExtensions
	{
		public static int IndexOf<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			var index = 0;
			foreach (var item in source)
			{
				if (predicate.Invoke(item))
				{
					return index;
				}

				index++;
			}

			return -1;
		}
	}
}