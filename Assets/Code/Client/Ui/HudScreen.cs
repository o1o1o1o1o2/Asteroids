using System.Linq;
using System.Threading.Tasks;
using Asteroids.Client.Db.ConfigsScriptableObjects;
using Asteroids.Client.Ecs.Components;
using FlyLib.Core.GameConfigs.Contracts;
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
		private readonly IFilteredGroup _laserCoolDownGroup;
		private readonly IFilteredGroup _laserCountGroup;
		private readonly PlayerConfig _playerConfig;

		private bool _needUpdate;
		private float _lastUpdateTime;

		public HudScreen(HudView view, GameContext gameContext, IGameConfigs gameConfigs)
		{
			_playerConfig = gameConfigs.GetConfig<PlayerConfig>();
			_hudView = view;
			_playerSpeedGroup = gameContext.AllOf<CPlayerTag, CPosition>();
			_playerPositionGroup = gameContext.AllOf<CPlayerTag, CRotation>();
			_playerRotationGroup = gameContext.AllOf<CPlayerTag, CVelocity>();
			_laserCountGroup = gameContext.AllOf<CLaserTag, CLaserShotCount>();
			_laserCoolDownGroup = gameContext.AllOf<CLaserTag, CLaserCoolDownSec>();
		}

		protected override Task ShowAsync()
		{
			_playerSpeedGroup.OnAddedOrUpdated += SetNeedUpdate;
			_playerPositionGroup.OnAddedOrUpdated += SetNeedUpdate;
			_playerRotationGroup.OnAddedOrUpdated += SetNeedUpdate;
			_laserCountGroup.OnAddedOrUpdated += SetNeedUpdate;
			_laserCoolDownGroup.OnAddedOrUpdated += SetNeedUpdate;
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

			var laserE = _laserCountGroup.FirstOrDefault();
			if (laserE == null)
			{
				return;
			}

			_hudView.SetLaserCooldownText($"Laser cooldown {laserE.GetValue<CLaserCoolDownSec>():0}/{_playerConfig.LaserCooldownSec}");
			_hudView.SetLaserCountText($"Laser count {laserE.GetValue<CLaserShotCount>()}/{_playerConfig.LaserMaxShots}");
		}

		protected override Task HideAsync()
		{
			_playerSpeedGroup.OnAddedOrUpdated -= SetNeedUpdate;
			_playerPositionGroup.OnAddedOrUpdated -= SetNeedUpdate;
			_playerRotationGroup.OnAddedOrUpdated -= SetNeedUpdate;
			_laserCountGroup.OnAddedOrUpdated -= SetNeedUpdate;
			_laserCoolDownGroup.OnAddedOrUpdated -= SetNeedUpdate;
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