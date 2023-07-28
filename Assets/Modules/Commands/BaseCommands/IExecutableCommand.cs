using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Commands.Project.Scripts.Modules.Commands.BaseCommands
{
    public interface ICommand : IDisposable
    {
        
    }
    
    public interface IExecutableCommand : ICommand
    {
        UniTask Execute(CancellationToken cancellationToken = default);
    }
    
    public interface IExecutableCommand<T> : ICommand
    {
        UniTask Execute(T payload, CancellationToken cancellationToken = default);
    }
}