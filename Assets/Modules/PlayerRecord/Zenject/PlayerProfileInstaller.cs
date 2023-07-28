using Zenject;

namespace Modules.PlayerRecord.Zenject
{
    public class PlayerProfileInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ProfileManager>().AsSingle();
        }
    }
}