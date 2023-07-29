using GameStateMachine.Project.Scripts.Modules.GameStateMachine;
using JetBrains.Annotations;
using Project.Scripts.Meta;
using UI;
using Zenject;

namespace Project.Scripts.Core
{
    public class LoseWindowModel : WindowModel
    {

    }

    public class LoseWindow : Window<LoseWindowModel>
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