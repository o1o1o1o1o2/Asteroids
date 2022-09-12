using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FlyLib.Core.SimpleStateMachine.Contracts;
using FlyLib.Core.Utils;
using UnityEngine;

namespace FlyLib.Core.SimpleStateMachine.Controls
{
	public class SimpleStateMachine : ISimpleStateMachine
	{
		private Dictionary<Type, IGameState> _states = new(5);

		public IGameState CurrentState { get; private set; }

		public void Register(params IGameState[] states)
		{
			AddStates(states);
		}

		private void AddStates(IEnumerable<IGameState> states)
		{
			foreach (var state in states)
			{
				if (state == null)
				{
					continue;
				}

				if (_states.Keys.Contains(state.GetType()))
				{
					continue;
				}

				_states.Add(state.GetType(), state);
			}
		}

		public async Task EnterAsync<T>(CancellationToken ct) where T : class, IEnterState
		{
			if (CurrentState != null && CurrentState.GetType() == TypeOf<T>.Raw)
			{
				return;
			}

			if (!_states.TryGetValue(TypeOf<T>.Raw, out var newState))
			{
				Debug.LogError($"Cannot find state with type {TypeOf<T>.Raw}");
				return;
			}

			Debug.Log($"[{TypeOf<T>.Name}] Change state {CurrentState?.GetType().Name ?? "n/a"} -> {newState.GetType().Name}");

			if (CurrentState is IExitState exitState)
			{
				await exitState.OnExitAsync(ct);
			}

			CurrentState = newState;

			if (CurrentState is IEnterState enterState)
			{
				await enterState.OnEnterAsync(ct);
			}
		}
	}
}