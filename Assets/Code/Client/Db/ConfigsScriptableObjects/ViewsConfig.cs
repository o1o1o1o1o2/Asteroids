using FlyLib.Core.GameConfigs.Controls;
using UnityEngine;

namespace Asteroids.Client.Db.ConfigsScriptableObjects
{
	[CreateAssetMenu(fileName = "ViewsConfig", menuName = "GameConfigs/New ViewsConfig", order = 51)]
	public class ViewsConfig : GameConfigUniq
	{
		[field: Header("Player")]
		[field: SerializeField]
		public string ShipPrefabAddress { get; private set; }

		[field: Header("Enemies")]
		[field: SerializeField]
		public string EnemyAsteroidAddressLabel { get; private set; }
	}
}