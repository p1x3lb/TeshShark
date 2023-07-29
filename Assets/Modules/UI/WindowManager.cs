using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace UI
{
    public class WindowManager
    {
        [Inject]
        private DiContainer Container { get; }

        [Inject]
        private UIConfig UIConfig { get; }

        private Transform _root;

        public void Initialize(Transform root)
        {
            _root = root;
        }

        public UniTask ShowWindow<T>(WindowModel model) where T : Window
        {
            var window = Container.InstantiatePrefabForComponent<T>(UIConfig.GetWindowPrefab<T>(), _root);
            window.Initialize(model);
            return UniTask.CompletedTask;
        }
    }
}