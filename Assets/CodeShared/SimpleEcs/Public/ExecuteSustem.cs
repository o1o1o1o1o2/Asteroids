using SimpleEcs.Contracts;

namespace SimpleEcs.Public
{
	public abstract class ExecuteSystem : IExecuteSystem
	{
		public abstract void Execute(float deltaTime);
		
		public void StartSystem()
		{
			Start();
		}

		public virtual void Start()
		{
		}
	}
}