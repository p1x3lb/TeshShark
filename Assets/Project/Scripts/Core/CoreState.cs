using System.Threading;
using Cysharp.Threading.Tasks;
using GameStateMachine.Project.Scripts.Modules.GameStateMachine;
using Project.Scripts.Features;
using Project.Scripts.Infrastructure;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Modules.Utils.Extensions;
using Zenject;

namespace Project.Scripts.Core
{
    public class CoreState : IGameState
    {
        [Inject]
        private WindowManager WindowManager { get; }

        [Inject]
        private PlayerModel PlayerModel { get; }

        [Inject]
        private GoalsManager GoalsManager { get; }

        public async UniTask Enter(CancellationToken cancellationToken)
        {
            await SceneManager.LoadSceneAsync("CoreScene").ToUniTask(cancellationToken: cancellationToken);

            var scene = SceneManager.GetActiveScene();

            WindowManager.Initialize(scene.FindComponentInRootObjects<Canvas>().transform);

            var context = scene.GetSceneContext<CoreStateContext>();

            var map = context.Container.InstantiatePrefab(context.Level.MapPrefab);

            context.Map = map.transform;
            context.Initialize(map.GetComponentsInChildren<CellView>());

            GoalsManager.Initialize(context.Level.Goals);

            WindowManager.ShowWindow<HUD>(new HUDModel(context));

            if (context.Level.Suggest != null && context.Level.Suggest.Sprite != null)
            {
                WindowManager.ShowWindow<SuggestWindow>(new SuggestWindowModel(context.Level.Suggest, context));
            }
        }

        public void Dispose()
        {
            GoalsManager.Clear();
        }
    }
}