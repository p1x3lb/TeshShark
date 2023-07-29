using System;
using System.Collections.Generic;
using System.Linq;
using GameStateMachine.Modules.GameStateMachine;
using Project.Scripts.Features;
using Project.Scripts.Infrastructure;
using UI;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Core
{
    public class CoreStateContext : IGameStateContext
    {
        public event Action<int> TurnsChanged;

        [Inject]
        public DiContainer Container { get; set; }

        [Inject]
        private PlayerModel PlayerModel { get; set; }

        [Inject]
        private WindowManager WindowManager { get; set; }

        [Inject]
        private LevelListConfig LevelListConfig { get; }

        public LevelConfig Level => LevelListConfig.GetLevel(PlayerModel.PlayerLevel);
        public int StepsLeft { get; private set; }
        public CellView[,] Cells { get; private set; }
        public Transform Map { get; set; }
        public int Speed => Level.Speed;
        public int Damage => Level.Damage;

        public IEnumerable<CellView> CellsEnumerable
        {
            get
            {
                for (int row = 0; row < Cells.GetLength(0); row++)
                {
                    for (int col = 0; col < Cells.GetLength(1); col++)
                    {
                        yield return Cells[row, col];
                    }
                }
            }
        }

        public void Initialize(IReadOnlyList<CellView> cells)
        {
            var orderdCels = cells.OrderBy(cell => cell.transform.position.y).ThenBy(cell => cell.transform.position.x)
                .ToArray();

            Cells = new CellView[Level.Rows, Level.Columns];

            int i = 0;

            for (int row = 0; row < Cells.GetLength(0); row++)
            {
                for (int col = 0; col < Cells.GetLength(1); col++)
                {
                    Cells[row, col] = orderdCels[i++];
                }
            }

            StepsLeft = Level.StepCount;
        }

        public bool ApplyTurn(int turns)
        {
            StepsLeft -= turns;
            TurnsChanged?.Invoke(StepsLeft);
            if (StepsLeft <= 0)
            {
                WindowManager.ShowWindow<TryAgainWindow>(new TryAgainWindowModel(this));
                return false;
            }

            return true;
        }
    }
}