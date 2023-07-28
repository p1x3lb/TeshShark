using GameStateMachine.Project.Scripts.Modules.GameStateMachine;
using JetBrains.Annotations;
using Project.Scripts.Core;
using UI;
using Zenject;

namespace Project.Scripts.Meta
{
    public class MetaWindowModel : WindowModel
    {

    }

    public class MetaWindow : Window<MetaWindowModel>
    {
        [Inject]
        private IGameStateMachine GameStateMachine { get; }

        [UsedImplicitly]
        public void OnClick()
        {
            GameStateMachine.Enter<CoreState>();
        }
    }
}