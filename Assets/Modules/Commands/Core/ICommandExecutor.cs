using System.Threading;
using Commands.Project.Scripts.Modules.Commands.BaseCommands;
using Cysharp.Threading.Tasks;

namespace Commands.Project.Scripts.Modules.Commands.Core
{
    public interface ICommandExecutor
    {
        UniTask Execute<T>(CancellationToken cancellationToken = default) where T : IExecutableCommand;
        UniTask Execute<TCommand, TPayload>(TPayload payload, CancellationToken cancellationToken = default) where TCommand : IExecutableCommand<TPayload>;
    }
}