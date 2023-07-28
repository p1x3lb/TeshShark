using GameStateMachine.Modules.GameStateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Utils.Modules.Utils.Extensions
{
    public static class SceneExtensions
    {
        public static T FindComponentInRootObjects<T>(this Scene scene) where T : Component
        {
            foreach (var gameObject in scene.GetRootGameObjects())
            {
                if (gameObject.TryGetComponent(out T component))
                {
                    return component;
                }
            }
            return null;
        }

        public static T GetSceneContext<T>(this Scene scene) where T : IGameStateContext
        {
            foreach (var obj in scene.GetRootGameObjects())
            {
                if (obj.TryGetComponent(out SceneContext sceneContext))
                {
                    return sceneContext.Container.Resolve<T>();
                }
            }

            return default;
        }
    }
}