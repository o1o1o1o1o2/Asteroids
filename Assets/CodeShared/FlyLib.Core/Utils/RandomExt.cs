using UnityEngine;

namespace FlyLib.Core.Utils
{
	public static class RandomExt
	{
		public static Vector2 GetRandomPositionAtTheEdgeOfRect(Rect rect, int? side = null)
		{
			side ??= Mathf.FloorToInt(Random.Range(0, 4));
			//    1
			//  0   2
			//    3   

			Vector2 pos;
			switch (side)
			{
				case 0:
				{
					pos.x = rect.xMin;
					pos.y = Random.Range(rect.yMin, rect.yMax);
					break;
				}

				case 1:
				{
					pos.x = Random.Range(rect.xMin, rect.xMax);
					pos.y = rect.yMax;
					break;
				}

				case 2:
				{
					pos.x = rect.xMax;
					pos.y = Random.Range(rect.yMin, rect.yMax);
					break;
				}

				default:
				{
					pos.x = Random.Range(rect.xMin, rect.xMax);
					pos.y = rect.yMin;
					break;
				}
			}

			return pos;
		}
	}
}