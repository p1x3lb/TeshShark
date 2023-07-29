using Project.Scripts.Core;
using Project.Scripts.Infrastructure;
using UnityEngine;
using Zenject;

namespace Project.GameAssets.Core
{
    public class CoreInstaller : MonoInstaller
    {
        [SerializeField]
        private InputManager _inputManager;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<CoreStateContext>().AsSingle();
            Container.Bind<InputManager>().FromInstance(_inputManager).AsSingle();
        }
    }
}