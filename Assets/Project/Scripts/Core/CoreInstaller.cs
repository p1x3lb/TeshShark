using Project.Scripts.Core;
using Project.Scripts.Infrastructure;
using Zenject;

namespace Project.GameAssets.Core
{
    public class CoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<CoreEcsManager>().AsSingle();
            Container.BindInterfacesTo<InputManager>().AsSingle();
        }
    }
}