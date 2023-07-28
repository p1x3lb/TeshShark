using Zenject;

namespace Items
{
    public class ItemInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ItemManager>().AsSingle();
        }
    }
}