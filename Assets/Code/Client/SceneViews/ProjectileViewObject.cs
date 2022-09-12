using System;
using Asteroids.Client.Ecs.Components.Interfaces;
using Asteroids.Client.Ecs.Components.Visual;
using SimpleEcs.Public;
using UnityEngine;

namespace Asteroids.Client.SceneViews
{
	public class ProjectileViewObject : MonoBehaviour, ISceneEntity, IMovableView
	{
		[SerializeField] private string _collidedWithTag;
		
		public Action TriggerEnter { get; set; }
		
		private Transform _tr;
		public Component Component => this;

		private void Awake()
		{
			_tr = transform;
		}
		
		public void AddToEntity(Entity logicEntity)
		{
			logicEntity.Set<VMovableView>(this);
		}

		public void SetPosition(Vector3 position)
		{
			_tr.position = position;
		}

		public void SetRotation(Quaternion rotation)
		{
			_tr.rotation = rotation;
		}
		
		private void OnTriggerEnter2D(Collider2D col)
		{
			if (col.CompareTag(_collidedWithTag))
			{
				TriggerEnter?.Invoke();
			}
		}
	}
}