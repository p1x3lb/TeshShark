using GameStateMachine.Project.Scripts.Modules.GameStateMachine;
using Project.Scripts.Core;
using Project.Scripts.Infrastructure;
using UI;
using UnityEngine;
using Zenject;

namespace Project.GameAssets
{
    public class ApplicationInstaller : MonoInstaller
    {
        [SerializeField]
        private LevelListConfig _levelListConfig;

        [SerializeField]
        private UIConfig _uiConfig;

        public override void InstallBindings()
        {
            Container.Install<GameStateMachineInstaller>();
            Container.Install<UIInstaller>();

            Container.BindInterfacesAndSelfTo<PlayerModel>().AsSingle();
            Container.Bind<LevelListConfig>().FromInstance(_levelListConfig).AsSingle();
            Container.Bind<UIConfig>().FromInstance(_uiConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<GoalsManager>().AsSingle();
        }
    }
}