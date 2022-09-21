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
			return Task.CompletedTask;
		}

		private Task HideInternalAsync()
		{
			gameObject.SetActive(false);
			return Task.CompletedTask;
		}
	}
}