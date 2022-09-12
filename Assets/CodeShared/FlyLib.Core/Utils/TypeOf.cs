using System;
using System.Diagnostics.CodeAnalysis;

namespace FlyLib.Core.Utils
{
	[SuppressMessage("ReSharper", "StaticMemberInGenericType")]
	public static class TypeOf<T>
	{
		public static readonly Type Raw = typeof(T);
		public static readonly string Name = Raw.Name;
	}
}