using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Project.Scripts.Core;
using UI;
using Zenject;

namespace Project.Scripts.Features
{
    public class TryAgainWindowModel : WindowModel
    {
    }

    public class TryAgainWindow : Window<TryAgainWindowModel>
    {
        [Inject]
        private CoreStateContext CoreStateContext { get; }

        [Inject]
        private WindowManager WindowManager { get; }

        [UsedImplicitly]
        public void OnTryAgain()
        {
            CoreStateContext.ApplyTurn(-5);
            Hide().Forget();
        }

        [UsedImplicitly]
        public void OnClose()
        {
            WindowManager.ShowWindow<LoseWindow>(new LoseWindowModel());
            Hide().Forget();
        }
    }
}