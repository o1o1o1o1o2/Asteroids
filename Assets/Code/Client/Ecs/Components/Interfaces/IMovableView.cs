using UnityEngine;

namespace Asteroids.Client.Ecs.Components.Interfaces
{
	public interface IMovableView
	{
		public Component Component { get; }
		void SetPosition(Vector3 position);
		void SetRotation(Quaternion rotation);
	}
}