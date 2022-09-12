using Asteroids.Client.Types;
using SimpleEcs.Components;
using SimpleEcs.Public;

namespace Asteroids.Client.Ecs.Components
{
	public class CPlayerCopyType : ValueComponent<PlayerCopyType>
	{
	}

	public static class CPlayerCopyTypeExtensions
	{
		public static PlayerCopyType GetValue<T>(this Entity entity) where T : CPlayerCopyType
		{
			return entity.Get<T, PlayerCopyType>();
		}

		public static void Set<T>(this Entity entity, PlayerCopyType value) where T : CPlayerCopyType, new()
		{
			entity.Set<T, PlayerCopyType>(value);
		}
	}
}