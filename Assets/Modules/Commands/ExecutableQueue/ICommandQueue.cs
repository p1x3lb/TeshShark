using System;
using System.Threading;
using Commands.Project.Scripts.Modules.Commands.BaseCommands;

namespace Commands.Project.Scripts.Modules.Commands.ExecutableQueue
{
    public interface ICommandQueue : IDisposable
    {
        bool AutoExecute { get; set; }
        bool IsPlaying { get;}
        void Add<TCommand, TPayload>(TPayload payload, int priority, CancellationToken cancellationToken = default) where TCommand : Command, IExecutableCommand<TPayload>;
        void Add<TCommand>(int priority, CancellationToken cancellationToken = default) where TCommand : Command, IExecutableCommand;
        void AddTrigger<TCommand>(int priority, CancellationToken cancellationToken = default) where TCommand : IExecutableCommand;
        void AddTrigger<TCommand,TPayload>(TPayload payload, int priority, CancellationToken cancellationToken = default) where TCommand : IExecutableCommand<TPayload>;
        void Play(CancellationToken cancellationToken = default);
        void Stop();
    }
}