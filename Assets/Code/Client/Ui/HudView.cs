using FlyLib.Core.Ui.ScreenManager.SceneViews;
using TMPro;
using UnityEngine;

namespace Asteroids.Client.Ui
{
	public class HudView : UiView
	{
		[SerializeField] private TMP_Text _txtPosition;
		[SerializeField] private TMP_Text _txtSpeed;
		[SerializeField] private TMP_Text _txtRotation;

		protected override void InitializeView()
		{
		}

		public void SetPositionText(string text)
		{
			_txtPosition.text = text;
		}

		public void SetSpeedText(string text)
		{
			_txtSpeed.text = text;
		}

		public void SetRotationText(string text)
		{
			_txtRotation.text = text;
		}
	}
}