using SimpleEcs.Tickables.Contracts;

namespace SimpleEcs.Contracts
{
	public interface IEcsRunner : ITickable
	{
		void Register(params ISystem[] systems);
		void Start();
		void Stop();
	}
}