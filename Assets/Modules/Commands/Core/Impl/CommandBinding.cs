using Commands.Project.Scripts.Modules.Commands.BaseCommands;

namespace Commands.Project.Scripts.Modules.Commands.Core.Impl
{
    public class CommandBinding : ICommandBinding
    {
        public ICommandInfo Info { get; private set; }

        public void To<TBind>() where TBind : class, ICommand
        {
            Info = new CommandInfo(typeof(TBind));
        }
    }
}