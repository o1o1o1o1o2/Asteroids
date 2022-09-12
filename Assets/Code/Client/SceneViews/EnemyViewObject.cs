using System;
using System.Threading;
using System.Threading.Tasks;
using Asteroids.Client.Ecs.Components.Interfaces;
using Asteroids.Client.Ecs.Components.Visual;
using FlyLib.Core.Pool;
using FlyLib.Core.Utils;
using SimpleEcs.Public;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.Client.SceneViews
{
	public class EnemyViewObject : MonoBehaviour, ISceneEntity, IMovableView
	{
		[SerializeField] string _playerTag = "Player";
		[SerializeField] string _playerBulletTag = "PlayerBullet";
		[SerializeField] private SpriteRenderer _renderer;
		[SerializeField] private Sprite[] _sprites;
		[SerializeField] private ParticleSystem _vfx;

		public Component Component => this;
		public Action TriggerEnter { get; set; }
		public Action TriggerEnterPlayer { get; set; }

		private Transform _tr;
		private CancellationTokenSource _cts;

		private void Awake()
		{
			_tr = transform;
		}

		private void OnDisable()
		{
			_cts?.Cancel();
			_cts = null;
		}

		public void AddToEntity(Entity logicEntity)
		{
			logicEntity.Set<VMovableView>(this);
		}

		private void OnEnable()
		{
			_renderer.enabled = true;
			if (_sprites is not { Length: > 0 })
			{
				return;
			}

			_renderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
			TryGetComponent(TypeOf<PolygonCollider2D>.Raw, out var col);
			if (!ReferenceEquals(col, null))
			{
				Destroy(col);
			}

			gameObject.AddComponent<PolygonCollider2D>().isTrigger = true;
		}

		public void SetPosition(Vector3 position)
		{
			_tr.position = position;
		}

		public void SetRotation(Quaternion rotation)
		{
			_tr.rotation = rotation;
		}

		public async void DestroyEnemy()
		{
			_cts ??= new CancellationTokenSource();
			await DestroyAsync(_cts.Token);
		}

		private async Task DestroyAsync(CancellationToken ct)
		{
			_renderer.enabled = false;
			if (_vfx == null)
			{
				return;
			}

			_vfx.Play();
			var time = (int)(_vfx.main.duration * 1000);
			await Task.Delay(time, ct);
			_vfx.Stop();
			this.Recycle();
		}

		private void OnTriggerEnter2D(Collider2D col)
		{
			if (col.CompareTag(_playerBulletTag))
			{
				TriggerEnter?.Invoke();
			}

			if (col.CompareTag(_playerTag))
			{
				TriggerEnterPlayer?.Invoke();
			}
		}
	}
}