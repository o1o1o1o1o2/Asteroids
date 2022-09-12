namespace SimpleEcs.Tickables.Contracts
{
	public interface ITickableManager : ITickable
	{
		void AddToTickAbles(ITickable tickable);
	}
}