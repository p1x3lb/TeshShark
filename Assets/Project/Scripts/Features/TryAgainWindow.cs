using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Project.Scripts.Core;
using UI;
using Zenject;

namespace Project.Scripts.Features
{
    public class TryAgainWindowModel : WindowModel
    {
        public CoreStateContext CoreStateContext { get; }

        public TryAgainWindowModel(CoreStateContext coreStateContext)
        {
            CoreStateContext = coreStateContext;
        }
    }

    public class TryAgainWindow : Window<TryAgainWindowModel>
    {
        [Inject]
        private WindowManager WindowManager { get; }

        [UsedImplicitly]
        public void OnTryAgain()
        {
            Model.CoreStateContext.ApplyTurn(-5);
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