using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Asteroids.Client.SceneViews
{
	[SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
	public class RootInstallerSceneObject : MonoBehaviour
	{
		private async void Awake()
		{
			var rootInstaller = new RootInstaller();
			await rootInstaller.InitAsync(GetComponentsInChildren<SceneInstaller>());
		}
	}
}