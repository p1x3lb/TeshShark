using System.Threading;
using Cysharp.Threading.Tasks;
using GameStateMachine.Project.Scripts.Modules.GameStateMachine;
using UI;
using UnityEngine;
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

            var scene = SceneManager.GetActiveScene();

            WindowManager.Initialize(scene.FindComponentInRootObjects<Canvas>().transform);

            var context = scene.GetSceneContext<MetaStateContext>();

            await WindowManager.ShowWindow<MetaWindow>(new MetaWindowModel());
        }

        public void Dispose()
        {

        }
    }
}