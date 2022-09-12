using System.Threading;
using System.Threading.Tasks;

namespace FlyLib.Core.SimpleStateMachine.Contracts
{
	public interface IEnterState
	{
		Task OnEnterAsync(CancellationToken ct);
	}
}