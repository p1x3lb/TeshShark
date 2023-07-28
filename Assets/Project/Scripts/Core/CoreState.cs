using System.Threading;
using Cysharp.Threading.Tasks;
using GameStateMachine.Project.Scripts.Modules.GameStateMachine;
using Project.Scripts.Infrastructure;
using UnityEngine.SceneManagement;
using Utils.Modules.Utils.Extensions;
using Zenject;

namespace Project.Scripts.Core
{
    public class CoreState : IGameState
    {
        [Inject]
        private PlayerModel PlayerModel { get; }

        public async UniTask Enter(CancellationToken cancellationToken)
        {
            await SceneManager.LoadSceneAsync("CoreScene").ToUniTask(cancellationToken: cancellationToken);

            var context = SceneManager.GetActiveScene().GetSceneContext<CoreStateContext>();
            context.Initialize();
            var map = context.Container.InstantiatePrefab(context.Level.MapPrefab);
        }

        public void Dispose()
        {

        }
    }
}