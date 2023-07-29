using Zenject;

namespace Project.Scripts.Meta
{
    public class MetaInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MetaStateContext>().AsSingle();
        }
    }
}