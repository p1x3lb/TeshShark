using System.Collections.Generic;
using GameStateMachine.Modules.GameStateMachine;
using Project.Scripts.Infrastructure;
using Zenject;

namespace Project.Scripts.Core
{
    public class CoreStateContext : IGameStateContext
    {
        [Inject]
        public DiContainer Container { get; set; }

        [Inject]
        private PlayerModel PlayerModel { get; set; }

        [Inject]
        private LevelListConfig LevelListConfig { get; }


        public LevelConfig Level => LevelListConfig.GetLevel(PlayerModel.PlayerLevel);
        public int StepsLeft { get; set; }
        public IReadOnlyList<CellView> Cells { get; private set; }
        public int Speed => Level.Speed;

        public void Initialize(IReadOnlyList<CellView> cells)
        {
            Cells = cells;
            StepsLeft = Level.StepCount;
        }
    }
}