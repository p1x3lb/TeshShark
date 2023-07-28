using System.Threading;
using Cysharp.Threading.Tasks;
using GameStateMachine.Project.Scripts.Modules.GameStateMachine;
using Project.Scripts.Core;
using UI;
using UnityEngine.SceneManagement;
using Utils.Modules.Utils.Extensions;
using Zenject;

namespace Project.Scripts.Meta
{
    public class MetaState : IGameState
    {
        [Inject]
        private WindowManager WindowManager { get; }

        public async UniTask Enter(CancellationToken cancellationToken)
        {
            await SceneManager.LoadSceneAsync("MetaScene").ToUniTask(cancellationToken: cancellationToken);

            var context = SceneManager.GetActiveScene().GetSceneContext<CoreStateContext>();

            await WindowManager.ShowWindow<MetaWindow>(new MetaWindowModel());
        }

        public void Dispose()
        {

        }
    }
}