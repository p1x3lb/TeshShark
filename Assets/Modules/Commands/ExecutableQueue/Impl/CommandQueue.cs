using System;
using System.Collections.Generic;
using System.Threading;
using Commands.Project.Scripts.Modules.Commands.BaseCommands;
using Commands.Project.Scripts.Modules.Commands.Core;
using Commands.Project.Scripts.Modules.Commands.Core.Impl;
using Cysharp.Threading.Tasks;

namespace Commands.Project.Scripts.Modules.Commands.ExecutableQueue.Impl
{
    public class PriorityComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            return y - x;
        }
    }

    internal class CommandQueue : ICommandQueue
    {
        public bool AutoExecute { get; set; }
        public bool IsPlaying { get; set; }

        private readonly ICommandFactory _commandFactory;
        private readonly ICommandBinder _commandBinder;
        private readonly IPriorityQueue<int, IQueuedCommand> _priorityQueue;
        
        private IQueuedCommand _currentCommand;

        public CommandQueue(ICommandFactory commandFactory, 
            ICommandBinder commandBinder)
        {
            _commandFactory = commandFactory;
            _commandBinder = commandBinder;
            _priorityQueue = new PriorityQueue<int, IQueuedCommand>(new PriorityComparer());
        }


        public void Add<TCommand, TPayload>(TPayload payload, int priority, CancellationToken cancellationToken = default)
            where TCommand : Command, IExecutableCommand<TPayload>
        {
            var command = _commandFactory.Create(new CommandInfo(typeof(TCommand)));
            var queuedCommand = new QueuedCommand<TPayload>(command, payload);
            _priorityQueue.Enqueue(priority, queuedCommand);
            if (AutoExecute)
            {
                Play(cancellationToken);
            }
        }

        public void Add<T>(int priority, CancellationToken cancellationToken = default) where T : Command, IExecutableCommand
        {
            var command = _commandFactory.Create(new CommandInfo(typeof(T)));
            var queuedCommand = new QueuedCommand(command);
            _priorityQueue.Enqueue(priority, queuedCommand);
            if (AutoExecute)
            {
                Play(cancellationToken);
            }
        }

        public void AddTrigger<T>(int priority, CancellationToken cancellationToken = default) where T : IExecutableCommand
        {
            if (_commandBinder.TryGetBind<T>(out ICommandBinding commandBinding))
            {
                var command = _commandFactory.Create(commandBinding.Info);
                var queuedCommand = new QueuedCommand(command);
                _priorityQueue.Enqueue(priority, queuedCommand);
                if (AutoExecute)
                {
                    Play(cancellationToken);
                }
            }
        }

        public void AddTrigger<TCommand, TPayload>(TPayload payload, int priority, CancellationToken cancellationToken = default) where TCommand : IExecutableCommand<TPayload>
        {
            if (_commandBinder.TryGetBind<TCommand>(out ICommandBinding commandBinding))
            {
                var command = _commandFactory.Create(commandBinding.Info);
                var queuedCommand = new QueuedCommand<TPayload>(command, payload);
                _priorityQueue.Enqueue(priority, queuedCommand);
                if (AutoExecute)
                {
                    Play(cancellationToken);
                }
            }
        }

        public void Play(CancellationToken cancellationToken)
        {
            IsPlaying = true;
            TryExecuteNext(cancellationToken).Forget();
        }

        public void Stop()
        {
            IsPlaying = false;
        }

        private async UniTask TryExecuteNext(CancellationToken cancellationToken)
        {
            if (_currentCommand != null || !IsPlaying || _priorityQueue.IsEmpty)
            {
                IsPlaying = false;
                return;
            }

            _currentCommand = _priorityQueue.Dequeue();

            try
            {
                await _currentCommand.Execute(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                Stop();
                return;
            }
            finally
            {
                _currentCommand = null;
            }
            
            await TryExecuteNext(cancellationToken);
        }
        

        public void Dispose()
        {
            _priorityQueue.Clear();
        }
    }
}