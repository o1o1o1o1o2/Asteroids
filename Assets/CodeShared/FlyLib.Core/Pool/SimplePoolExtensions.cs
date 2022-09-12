using UnityEngine;

namespace FlyLib.Core.Pool
{
	public static class SimplePoolExtensions
	{
		public static T Spawn<T>(this T component, Transform parent = null, Vector3 position = default, Quaternion rotation = default)
			where T : Component
		{
			return SimplePool.Instance.Spawn(component, parent, position, rotation);
		}
		
		public static void Recycle<T>(this T component) where T : Component
		{
			SimplePool.Instance.Recycle(component);
		}
		
		public static void Recycle(this GameObject gameObject)
		{
			SimplePool.Instance.Recycle(gameObject.transform);
		}
	}
}