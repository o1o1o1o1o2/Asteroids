using System;
using FlyLib.Core.Utils;

namespace SimpleEcs.Components
{
	public abstract class ValueComponent<T> : IValueComponent<T>, IComparable<ValueComponent<T>>
	{
		public T Value { get; set; }

		public static implicit operator T(ValueComponent<T> component)
		{
			return component != null ? component.Value : default;
		}

		public override string ToString()
		{
			return $"{GetType().Name}({Value})";
		}

		public int CompareTo(ValueComponent<T> other)
		{
			if (ReferenceEquals(this, other))
			{
				return 0;
			}

			if (ReferenceEquals(null, other))
			{
				return 1;
			}

			if (Value is IComparable<T> comparableT)
			{
				return comparableT.CompareTo(other.Value);
			}

			throw new InvalidOperationException(
				$"{GetType().FullName} cannot compare value of type {TypeOf<T>.Name} - value must be IComparable<{TypeOf<T>.Name}>");
		}
	}
}