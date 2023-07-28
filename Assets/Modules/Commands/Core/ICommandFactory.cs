using Commands.Project.Scripts.Modules.Commands.BaseCommands;

namespace Commands.Project.Scripts.Modules.Commands.Core
{
    public interface ICommandFactory
    {
        ICommand Create(ICommandInfo binding);
    }
}