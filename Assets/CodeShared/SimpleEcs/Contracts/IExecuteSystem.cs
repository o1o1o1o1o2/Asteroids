namespace SimpleEcs.Contracts
{
	public interface IExecuteSystem : ISystem
	{
		void Execute(float deltaTime);
	}
}