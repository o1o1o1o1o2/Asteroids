using UnityEngine;

namespace Asteroids.Client.Types
{
	public static class CameraExtensions
	{
		public static Rect GetCamWorldRect(this Camera camera)
		{
			var aspect = (float)Screen.width / Screen.height;
			var worldHeight = camera.orthographicSize * 2;
			var worldWidth = worldHeight * aspect;
			return new Rect(camera.ViewportToWorldPoint(new Vector3(0, 0, 0)), new Vector2(worldWidth, worldHeight));
		}
	}
}