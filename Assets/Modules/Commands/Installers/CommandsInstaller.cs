using Commands.Project.Scripts.Modules.Commands.Core;
using Commands.Project.Scripts.Modules.Commands.Core.Impl;
using Commands.Project.Scripts.Modules.Commands.ExecutableQueue;
using Commands.Project.Scripts.Modules.Commands.ExecutableQueue.Impl;
using Zenject;

namespace Commands.Project.Scripts.Modules.Commands.Installers
{
    public class CommandsInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<ICommandBinder>().To<CommandBinder>().AsSingle();
            Container.Bind<ICommandExecutor>().To<CommandExecutor>().AsSingle();
            Container.Bind<ICommandFactory>().To<CommandFactory>().AsSingle();
            Container.Bind<ICommandQueueFactory>().To<CommandQueueFactory>().AsSingle();
        }
    }
}