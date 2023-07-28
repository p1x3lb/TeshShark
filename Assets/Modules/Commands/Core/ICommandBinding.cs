using Commands.Project.Scripts.Modules.Commands.BaseCommands;

namespace Commands.Project.Scripts.Modules.Commands.Core
{
    public interface ICommandBinding
    {
        ICommandInfo Info { get; }
        void To<TBind>() where TBind : class, ICommand;
    }
}