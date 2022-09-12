using Asteroids.Client.Ecs.Components.Interfaces;
using SimpleEcs.Components;
using SimpleEcs.Public;

namespace Asteroids.Client.Ecs.Components.Visual
{
	public class VPlayerView : ValueComponent<IPlayerView>
	{
	}

	public static class VPlayerViewExtensions
	{
		public static IPlayerView GetValue<T>(this Entity entity) where T : VPlayerView
		{
			return entity.Get<T, IPlayerView>();
		}

		public static void Set<T>(this Entity entity, IPlayerView value) where T : VPlayerView, new()
		{
			entity.Set<T, IPlayerView>(value);
		}
	}
}