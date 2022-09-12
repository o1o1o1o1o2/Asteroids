using Asteroids.Client.Types;
using SimpleEcs.Components;
using SimpleEcs.Public;

namespace Asteroids.Client.Ecs.Components
{
	public class CShootInfo : ValueComponent<ShootInfo>
	{
	}

	public static class CShootInfoExtensions
	{
		public static ShootInfo GetValue<T>(this Entity entity) where T : CShootInfo
		{
			return entity.Get<T, ShootInfo>();
		}

		public static void Set<T>(this Entity entity, ShootInfo value) where T : CShootInfo, new()
		{
			entity.Set<T, ShootInfo>(value);
		}
	}
}