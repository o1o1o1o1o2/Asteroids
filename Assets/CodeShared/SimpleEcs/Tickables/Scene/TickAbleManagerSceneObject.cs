using SimpleEcs.Tickables.Contracts;
using UnityEngine;

namespace SimpleEcs.Tickables.Scene
{
	public class TickAbleManagerSceneObject : MonoBehaviour
	{
		private ITickableManager _tickableManager;

		public void Initialize(ITickableManager tickableManager)
		{
			_tickableManager = tickableManager;
		}

		private void Update()
		{
			if (_tickableManager != null)
			{
				_tickableManager.Tick(Time.deltaTime);
			}
		}
	}
}