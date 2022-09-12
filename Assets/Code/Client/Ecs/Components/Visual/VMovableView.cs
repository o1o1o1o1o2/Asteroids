using Asteroids.Client.Ecs.Components.Interfaces;
using SimpleEcs.Components;
using SimpleEcs.Public;

namespace Asteroids.Client.Ecs.Components.Visual
{
	public class VMovableView : ValueComponent<IMovableView>
	{
	}

	public static class VMovableViewExtensions
	{
		public static IMovableView GetValue<T>(this Entity entity) where T : VMovableView
		{
			return entity.Get<T, IMovableView>();
		}

		public static void Set<T>(this Entity entity, IMovableView value) where T : VMovableView, new()
		{
			entity.Set<T, IMovableView>(value);
		}
	}
}