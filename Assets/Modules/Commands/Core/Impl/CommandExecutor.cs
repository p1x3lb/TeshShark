using System.Threading;
using Commands.Project.Scripts.Modules.Commands.BaseCommands;
using Commands.Project.Scripts.Modules.Commands.Exception;
using Cysharp.Threading.Tasks;

namespace Commands.Project.Scripts.Modules.Commands.Core.Impl
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly ICommandBinder _commandBinder;
        private readonly ICommandFactory _commandFactory;

        public CommandExecutor(ICommandBinder commandBinder, 
            ICommandFactory commandFactory)
        {
            _commandBinder = commandBinder;
            _commandFactory = commandFactory;
        }

        public UniTask Execute<T>(CancellationToken cancellationToken) where T : IExecutableCommand
        {
            if (_commandBinder.TryGetBind<T>(out ICommandBinding binding) && 
                _commandFactory.Create(binding.Info) is IExecutableCommand executableCommand)
            {
                return executableCommand.Execute();
            }
            throw new NoSuchCommandException();
        }
        
        public UniTask Execute<TCommand, TPayload>(TPayload payload, CancellationToken cancellationToken) where TCommand : IExecutableCommand<TPayload>
        {
            if (_commandBinder.TryGetBind<TCommand>(out ICommandBinding binding) && 
                _commandFactory.Create(binding.Info) is IExecutableCommand<TPayload> executableCommand)
            {
                return executableCommand.Execute(payload);
            }
            throw new NoSuchCommandException();
        }
    }
}