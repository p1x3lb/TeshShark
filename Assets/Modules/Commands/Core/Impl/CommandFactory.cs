using Commands.Project.Scripts.Modules.Commands.BaseCommands;
using Zenject;

namespace Commands.Project.Scripts.Modules.Commands.Core.Impl
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IInstantiator _instantiator;

        public CommandFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public ICommand Create(ICommandInfo binding)
        {
            return _instantiator.Instantiate(binding.BindedType) as ICommand;
        }
    }
}