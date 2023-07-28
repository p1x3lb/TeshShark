using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace GameStateMachine.Project.Scripts.Modules.GameStateMachine
{
    public interface IGameState : IDisposable
    {
        UniTask Enter(CancellationToken cancellationToken);
    }
}