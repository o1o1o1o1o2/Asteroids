using SimpleEcs.Components;
using UnityEngine;

namespace SimpleEcs.Public
{
	public partial class Entity
	{
		public void Set<T>(int value) where T : class, IValueComponent<int>, new()
		{
			Set<T, int>(value);
		}

		public int GetValue<T>(int defaultVal = default) where T : class, IValueComponent<int>
		{
			return Get<T, int>(defaultVal);
		}

		public void Set<T>(float value) where T : class, IValueComponent<float>, new()
		{
			Set<T, float>(value);
		}

		public float GetValue<T>(float defaultVal = default) where T : class, IValueComponent<float>
		{
			return Get<T, float>(defaultVal);
		}

		public void Set<T>(Vector3 value) where T : class, IValueComponent<Vector3>, new()
		{
			Set<T, Vector3>(value);
		}

		public Vector3 GetValue<T>(Vector3 defaultVal = default) where T : class, IValueComponent<Vector3>
		{
			return Get<T, Vector3>(defaultVal);
		}
		
		public void Set<T>(Quaternion value) where T : class, IValueComponent<Quaternion>, new()
		{
			Set<T, Quaternion>(value);
		}

		public Quaternion GetValue<T>(Quaternion defaultVal = default) where T : class, IValueComponent<Quaternion>
		{
			return Get<T, Quaternion>(defaultVal);
		}
		
		public void Set<T>(Transform value) where T : class, IValueComponent<Transform>, new()
		{
			Set<T, Transform>(value);
		}

		public Transform GetValue<T>(Transform defaultVal = default) where T : class, IValueComponent<Transform>
		{
			return Get<T, Transform>(defaultVal);
		}
	}
}