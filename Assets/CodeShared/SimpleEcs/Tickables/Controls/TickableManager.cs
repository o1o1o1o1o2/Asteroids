using System.Collections.Generic;
using SimpleEcs.Tickables.Contracts;

namespace SimpleEcs.Tickables.Controls
{
	public class TickableManager : ITickableManager
	{
		private readonly List<ITickable> _tickAbles = new();

		public void Tick(float deltaTime)
		{
			_tickAbles.RemoveAll(x => x == null);
			foreach (var tickAble in _tickAbles)
			{
				tickAble.Tick(deltaTime);
			}
		}

		public void AddToTickAbles(ITickable tickable)
		{
			if (tickable != null)
			{
				_tickAbles.Add(tickable);
			}
		}
	}
}