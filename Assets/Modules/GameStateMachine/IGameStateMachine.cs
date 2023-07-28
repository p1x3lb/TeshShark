using System.Threading;
using Cysharp.Threading.Tasks;

namespace GameStateMachine.Project.Scripts.Modules.GameStateMachine
{
    public interface IGameStateMachine
    {
        UniTask Enter<T>(CancellationToken cancellationToken = default) where T : class, IGameState;
        UniTask Enter<T>(object context, CancellationToken cancellationToken = default) where T : class, IGameState;
    }
}