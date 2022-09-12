using Asteroids.Client.Db.EnemyTypes;
using SimpleEcs.Components;
using SimpleEcs.Public;

namespace Asteroids.Client.Ecs.Components
{
	public class CEnemyDef : ValueComponent<EnemyDefinition>
	{
	}
	
	public static class CEnemyTypeExtensions
	{
		public static EnemyDefinition GetValue<T>(this Entity entity) where T : CEnemyDef
		{
			return entity.Get<T, EnemyDefinition>();
		}

		public static void Set<T>(this Entity entity, EnemyDefinition value) where T : CEnemyDef, new()
		{
			entity.Set<T, EnemyDefinition>(value);
		}
	}
}