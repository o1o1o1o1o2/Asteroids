using System.Threading;
using System.Threading.Tasks;

namespace FlyLib.Core.SimpleStateMachine.Contracts
{
	public interface ISimpleStateMachine
	{
		void Register(params IGameState[] states);
		Task EnterAsync<T>(CancellationToken ct = default) where T : class, IEnterState;
	}
}