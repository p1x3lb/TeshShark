using Cysharp.Threading.Tasks;
using GameStateMachine.Project.Scripts.Modules.GameStateMachine;
using JetBrains.Annotations;
using Project.Scripts.Meta;
using UI;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Core
{
    public class ExitWindowModel : WindowModel
    {

    }

    public class ExitWindow : Window<ExitWindowModel>
    {
        [Inject]
        private IGameStateMachine GameStateMachine { get; }

        [UsedImplicitly]
        public void OnCancel()
        {
            Hide().Forget();
        }

        [UsedImplicitly]
        public void OnExit()
        {
            GameStateMachine.Enter<MetaState>();
        }
    }
}