using Zenject;

namespace UI
{
    public class UIInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<WindowManager>().AsSingle();
        }
    }
}