using System.Threading;
using Commands.Project.Scripts.Modules.Commands.BaseCommands;
using Cysharp.Threading.Tasks;

namespace Commands.Project.Scripts.Modules.Commands.ExecutableQueue.Impl
{
    public class QueuedCommand : IQueuedCommand
    {
        private readonly ICommand _command;

        public QueuedCommand(ICommand command)
        {
            _command = command;
        }

        public UniTask Execute(CancellationToken cancellationToken)
        {
            return Invoke(cancellationToken);
        }

        private UniTask Invoke(CancellationToken cancellationToken)
        {
            if (_command is IExecutableCommand executableCommand)
            {
                return executableCommand.Execute(cancellationToken);
            }

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            _command.Dispose();
        }
    }

    public class QueuedCommand<TPayload> : IQueuedCommand
    {
        private readonly ICommand _command;
        private readonly TPayload _payload;

        public QueuedCommand(ICommand command, TPayload payload)
        {
            _command = command;
            _payload = payload;
        }

        public UniTask Execute(CancellationToken cancellationToken)
        {
            return Invoke(cancellationToken);
        }

        private UniTask Invoke(CancellationToken cancellationToken)
        {
            if (_command is IExecutableCommand<TPayload> executableCommandWithPayload)
            {
                return executableCommandWithPayload.Execute(_payload, cancellationToken);
            }

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            _command.Dispose();
        }
    }
}