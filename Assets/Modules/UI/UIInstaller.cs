using UnityEngine;
using Zenject;

namespace UI
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField]
        private UIConfig _uiConfig;

        [SerializeField]
        private Transform _root;

        public override void InstallBindings()
        {
            Container.Bind<UIConfig>().FromInstance(_uiConfig).AsSingle();
            Container.Bind<WindowManager>().AsSingle().WithArguments(_root);
        }
    }
}