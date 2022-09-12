using FlyLib.Core.GameConfigs.Controls;
using UnityEngine;

namespace Asteroids.Client.Db.ConfigsScriptableObjects
{
	[CreateAssetMenu(fileName = "MainConfig", menuName = "GameConfigs/New MainConfig", order = 51)]
	public class MainConfig : GameConfigUniq
	{
		[field: SerializeField] public string TickableManagerSceneObject { get; private set; }
	}
}