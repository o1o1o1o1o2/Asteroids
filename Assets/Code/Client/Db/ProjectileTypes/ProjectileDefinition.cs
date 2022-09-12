using Asteroids.Client.SceneViews;
using UnityEngine;

namespace Asteroids.Client.Db.ProjectileTypes
{
	[CreateAssetMenu(fileName = "Projectile", menuName = "GameConfigs/Projectile", order = 51)]
	public class ProjectileDefinition : ScriptableObject
	{
		[field: SerializeField] public ProjectileViewObject ProjectilePrefab { get; private set; }
	}
}