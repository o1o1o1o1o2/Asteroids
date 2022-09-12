using System.Collections.Generic;
using SimpleEcs.Contracts;

namespace SimpleEcs.Public
{
	public class EcsRunner : IEcsRunner
	{
		private readonly List<ISystem> _systems = new();
		private readonly List<IExecuteSystem> _executeSystems = new();
		private readonly List<IInitializeSystem> _initializeSystems = new();

		private bool _started;

		public void Register(params ISystem[] systems)
		{
			AddSystems(systems);
		}

		private void AddSystems(IEnumerable<ISystem> systems)
		{
			foreach (var system in systems)
			{
				if (system == null)
				{
					continue;
				}

				if (_systems.Contains(system))
				{
					continue;
				}

				if (system is IInitializeSystem initializeSystem)
				{
					_initializeSystems.Add(initializeSystem);
					if (_started)
					{
						initializeSystem.Initialize();
					}
				}

				_systems.Add(system);

				if (system is IExecuteSystem executeSystem)
				{
					_executeSystems.Add(executeSystem);
				}
			}
		}

		public void Start()
		{
			_started = true;

			foreach (var initializeSystem in _initializeSystems)
			{
				initializeSystem.Initialize();
			}
		}

		public void Stop()
		{
			_started = false;
		}

		public void Tick(float deltaTime)
		{
			if (!_started)
			{
				return;
			}
			foreach (var executeSystem in _executeSystems)
			{
				if (!_started)
				{
					return;
				}

				executeSystem.Execute(deltaTime);
			}
		}
	}
}