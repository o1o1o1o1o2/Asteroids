using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Asteroids.Client.SceneViews
{
	[SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
	public class RootInstallerSceneObject : MonoBehaviour
	{
		private void Awake()
		{
			new RootInstaller(GetComponentsInChildren<SceneInstaller>());
		}
	}
}