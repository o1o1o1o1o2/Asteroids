using System.Threading;
using System.Threading.Tasks;

namespace FlyLib.Core.SimpleStateMachine.Contracts
{
	public interface IExitState
	{
		Task OnExitAsync(CancellationToken ct);
	}
}