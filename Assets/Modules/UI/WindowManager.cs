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

        [Inject]
        private Transform Root { get; }

        public UniTask ShowWindow<T>(WindowModel model) where T : Window
        {
            var window = Container.InstantiatePrefabForComponent<T>(UIConfig.GetWindowPrefab<T>(), Root);
            window.Initialize(model);
            return UniTask.CompletedTask;
        }
    }
}