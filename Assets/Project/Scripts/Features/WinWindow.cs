using GameStateMachine.Project.Scripts.Modules.GameStateMachine;
using JetBrains.Annotations;
using Project.Scripts.Core;
using Project.Scripts.Meta;
using UI;
using Zenject;

namespace Project.Scripts.Features
{
    public class WinWindowModel : WindowModel
    {

    }

    public class WinWindow : Window<WinWindowModel>
    {
        [Inject]
        private IGameStateMachine GameStateMachine { get; }

        [UsedImplicitly]
        public void OnNextLevel()
        {
            GameStateMachine.Enter<CoreState>();
        }

        [UsedImplicitly]
        public void OnHome()
        {
            GameStateMachine.Enter<MetaState>();
        }
    }
}