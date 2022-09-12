using Asteroids.Client.Ecs.Components;
using SimpleEcs.Contracts;
using SimpleEcs.Public;
using UnityEngine;

namespace Asteroids.Client.Ecs.Systems.Logic
{
	public class RotateSystem : IExecuteSystem
	{
		private readonly IFilteredGroup _movables;

		public RotateSystem(GameContext gameContext)
		{
			_movables = gameContext.AllOf<CRotation, CAngularSpeed, CAngularVelocity>();
		}

		public void Execute(float deltaTime)
		{
			foreach (var entity in _movables)
			{
				entity.Set<CRotation>(entity.GetValue<CRotation>() *
					Quaternion.Euler(entity.GetValue<CAngularVelocity>() * (entity.GetValue<CAngularSpeed>() * deltaTime)));
			}
		}
	}
}