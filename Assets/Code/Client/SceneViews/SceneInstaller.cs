using UnityEngine;

namespace Asteroids.Client.SceneViews
{
	public abstract class SceneInstaller : MonoBehaviour
	{
		protected RootInstaller RootInstaller;
		
		public void Construct(RootInstaller rootInstaller)
		{
			RootInstaller = rootInstaller;
			Install();
		}

		protected abstract void Install();
	}
}