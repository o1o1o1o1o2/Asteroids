using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FlyLib.Core.GameConfigs.Controls
{
	public abstract class GameConfigUniq : ScriptableObject
	{
#if UNITY_EDITOR
		private Object _another;

		private void OnEnable()
		{
			var configType = GetType();
			var objects = Resources.FindObjectsOfTypeAll(configType);
			if (objects.Length <= 1)
			{
				return;
			}

			_another = objects.FirstOrDefault(x => x != this);

			Debug.LogError($"You cannot create more than one config of type:{GetType().Name}");
			EditorApplication.update += DestroyOnDuplicate;
		}

		private void DestroyOnDuplicate()
		{
			EditorApplication.update -= DestroyOnDuplicate;
			var e = new Event { keyCode = KeyCode.Escape, type = EventType.KeyDown };
			EditorWindow.focusedWindow.SendEvent(e);
			EditorGUIUtility.PingObject(_another);
			DestroyImmediate(this);
		}
#endif
	}
}