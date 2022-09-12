using System;
using FlyLib.Core.Ui.ScreenManager.SceneViews;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroids.Client.Ui
{
	public class StartView : UiView
	{
		[SerializeField] private Button _btStart;
		[SerializeField] private TMP_Text _txtButtonLabel;
		[SerializeField] private TMP_Text _txtScores;

		public Action StartClick { get; set; }

		protected override void InitializeView()
		{
			_btStart.onClick.AddListener(() => StartClick?.Invoke());
		}

		public void SetButtonText(string text)
		{
			_txtButtonLabel.text = text;
		}
		
		public void SetHiScoresActive(bool active)
		{
			_txtScores.gameObject.SetActive(active);
		}

		public void SetHiScoresText(string text)
		{
			_txtScores.text = text;
		}
	}
}