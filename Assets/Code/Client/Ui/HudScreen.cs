using System.Linq;
using System.Threading.Tasks;
using Asteroids.Client.Ecs.Components;
using FlyLib.Core.Ui.ScreenManager.Controlls;
using SimpleEcs.Contracts;
using SimpleEcs.Public;
using SimpleEcs.Tickables.Contracts;

namespace Asteroids.Client.Ui
{
	public class HudScreen : UIScreen, ITickable
	{
		private const float UpdateRate = 0.5f;

		private readonly HudView _hudView;
		private readonly IFilteredGroup _playerSpeedGroup;
		private readonly IFilteredGroup _playerPositionGroup;
		private readonly IFilteredGroup _playerRotationGroup;

		private bool _needUpdate;
		private float _lastUpdateTime;

		public HudScreen(HudView view, GameContext gameContext)
		{
			_hudView = view;
			_playerSpeedGroup = gameContext.AllOf<CPlayerTag, CPosition>();
			_playerPositionGroup = gameContext.AllOf<CPlayerTag, CRotation>();
			_playerRotationGroup = gameContext.AllOf<CPlayerTag, CVelocity>();
		}

		protected override Task ShowAsync()
		{
			_playerSpeedGroup.OnAddedOrUpdated += SetNeedUpdate;
			_playerPositionGroup.OnAddedOrUpdated += SetNeedUpdate;
			_playerRotationGroup.OnAddedOrUpdated += SetNeedUpdate;
			return _hudView.ShowAsync();
		}

		private void SetNeedUpdate()
		{
			_needUpdate = true;
		}

		private void UpdateView()
		{
			var playerE = _playerSpeedGroup.FirstOrDefault();
			if (playerE == null)
			{
				return;
			}

			_hudView.SetRotationText($"Rotation : {playerE.GetValue<CRotation>().eulerAngles.z:0}");
			_hudView.SetPositionText($"Position {playerE.GetValue<CPosition>().ToString("0")}");
			_hudView.SetSpeedText($"Speed {playerE.GetValue<CVelocity>().magnitude:0}");
		}

		protected override Task HideAsync()
		{
			_playerSpeedGroup.OnAddedOrUpdated -= SetNeedUpdate;
			_playerPositionGroup.OnAddedOrUpdated -= SetNeedUpdate;
			_playerRotationGroup.OnAddedOrUpdated -= SetNeedUpdate;
			return _hudView.HideAsync();
		}

		public void Tick(float deltaTime)
		{
			_lastUpdateTime += deltaTime;

			if (!_needUpdate || _lastUpdateTime < UpdateRate)
			{
				return;
			}

			UpdateView();
			_needUpdate = false;
			_lastUpdateTime = 0f;
		}
	}
}