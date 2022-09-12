using System;
using UnityEngine;

namespace Asteroids.Client.Db.EnemyTypes
{
	[Serializable]
	public abstract class EnemyDefinition : ScriptableObject
	{
		[field: SerializeField] public int ScoresOnDestroy { get; private set; }
		[field: SerializeField] public string PrefabName { get; private set; }
	}
}