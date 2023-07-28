using Zenject;

namespace GameStateMachine.Project.Scripts.Modules.GameStateMachine
{
    public class GameStateMachineInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();
        }
    }
}