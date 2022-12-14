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
		[SerializeField] private TMP_Text _laserCount;
		[SerializeField] private TMP_Text _laserCoolDown;

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
		
		public void SetLaserCountText(string text)
		{
			_laserCount.text = text;
		}
		public void SetLaserCooldownText(string text)
		{
			_laserCoolDown.text = text;
		}
	}
}