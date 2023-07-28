using Commands.Project.Scripts.Modules.Commands.Installers;
using Zenject;

namespace Project.GameAssets
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Install<CommandsInstaller>();
        }
    }
}