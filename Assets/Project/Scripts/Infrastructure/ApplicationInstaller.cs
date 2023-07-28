using GameStateMachine.Project.Scripts.Modules.GameStateMachine;
using Project.Scripts.Infrastructure;
using UnityEngine;
using Zenject;

namespace Project.GameAssets
{
    public class ApplicationInstaller : MonoInstaller
    {
        [SerializeField]
        private LevelListConfig _levelListConfig;

        public override void InstallBindings()
        {
            Container.Install<GameStateMachineInstaller>();

            Container.BindInterfacesAndSelfTo<PlayerModel>();
            Container.Bind<LevelListConfig>().FromInstance(_levelListConfig).AsSingle();
        }
    }
}