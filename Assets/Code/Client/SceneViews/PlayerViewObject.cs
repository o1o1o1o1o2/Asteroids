using System;
using Asteroids.Client.Ecs.Components.Interfaces;
using Asteroids.Client.Ecs.Components.Visual;
using FlyLib.Core.Utils;
using SimpleEcs.Public;
using UnityEngine;

namespace Asteroids.Client.SceneViews
{
	public class PlayerViewObject : MonoBehaviour, ISceneEntity, IMovableView, IPlayerView
	{
		[SerializeField] private SpriteRenderer _renderer;
		[SerializeField] private Sprite _spShipNormal;
		[SerializeField] private Sprite _spShipAccel;
		[SerializeField] private Transform _trBulletSpawnPoint;
		[SerializeField] private GameObject _goLaser;

		public Component Component => this;
		public Vector3 BulletSpawnPoint => _trBulletSpawnPoint.position;
		public Vector3 Position => _tr.position;

		private Transform _tr;

		private void Awake()
		{
			_tr = transform;
		}

		public void AddToEntity(Entity logicEntity)
		{
			logicEntity.Set<VMovableView>(this);
			logicEntity.Set<VPlayerView>(this);
		}

		private void OnEnable()
		{
			_renderer.sprite = _spShipNormal;
		}

		public void SetPosition(Vector3 position)
		{
			transform.position = position;
		}

		public void SetRotation(Quaternion rotation)
		{
			transform.rotation = rotation;
		}

		public void SetAccelerated(bool accelerated)
		{
			_renderer.sprite = accelerated ? _spShipAccel : _spShipNormal;
			TryGetComponent(TypeOf<PolygonCollider2D>.Raw, out var col);
			if (!ReferenceEquals(col, null))
			{
				Destroy(col);
			}

			gameObject.AddComponent<PolygonCollider2D>().isTrigger = true;
		}

		public void SetLaserActive(bool active)
		{
			_goLaser.SetActive(active);
		}
	}
}