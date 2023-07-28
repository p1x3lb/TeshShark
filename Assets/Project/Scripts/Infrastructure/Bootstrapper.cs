using GameStateMachine.Project.Scripts.Modules.GameStateMachine;
using Project.Scripts.Meta;
using UnityEngine;
using Zenject;

namespace Project.GameAssets
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField]
        private SceneContext _sceneContext;

        private void Start()
        {
            var container = _sceneContext.Container;
            container.Resolve<IGameStateMachine>().Enter<MetaState>();
        }
    }
}