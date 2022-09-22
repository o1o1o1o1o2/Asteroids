using System.Threading.Tasks;
using UnityEngine;

namespace FlyLib.Core.Ui.ScreenManager.SceneViews
{
	public abstract class UiView : MonoBehaviour
	{
		public void Construct()
		{
			InitializeView();
			gameObject.SetActive(false);
		}

		protected abstract void InitializeView();

		public Task ShowAsync()
		{
			Debug.Log($"[UI][{GetType().Name}] Show");
			return ShowInternalAsync();
		}

		public Task HideAsync()
		{
			Debug.Log($"[UI][{GetType().Name}] Hide");
			return HideInternalAsync();
		}

		private Task ShowInternalAsync()
		{
			gameObject.transform.SetAsLastSibling();
			gameObject.SetActive(true);
			FixBug();
			return Task.CompletedTask;
		}

		private void
			FixBug() //temporal fix for unity bug 2021.3.6f1 https://forum.unity.com/threads/canvas-hierarchy-drawing-order-broken-after-upgrade-to-unity-2021-3.1308642/
		{
			var canvas = gameObject.GetComponent<Canvas>();
			canvas.overrideSorting = true;
			canvas.overrideSorting = false;
		}

		private Task HideInternalAsync()
		{
			gameObject.SetActive(false);
			return Task.CompletedTask;
		}
	}
}