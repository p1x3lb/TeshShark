using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameStateMachine.Modules.GameStateMachine;
using Project.Scripts.Features;
using Project.Scripts.Infrastructure;
using UI;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Core
{
    public class CoreStateContext : IGameStateContext, IDisposable
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

        [Inject]
        private GoalsManager GoalsManager { get; }

        public LevelConfig Level => LevelListConfig.GetLevel(PlayerModel.PlayerLevel);
        public int StepsLeft { get; private set; }
        public CellView[,] Cells { get; private set; }
        public Transform Map { get; set; }
        public int Speed => Level.Speed;
        public int Damage => Level.Damage;

        public bool IsLocked { get; set; }

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

            GoalsManager.Win += OnGoalsManaerWin;
        }

        public void Dispose()
        {
            GoalsManager.Win -= OnGoalsManaerWin;
        }

        private void OnGoalsManaerWin()
        {
            PlayerModel.CompleteLevel();
            PlayWinFlow().Forget();
        }

        private async UniTask PlayWinFlow()
        {
            IsLocked = true;
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
            if (PlayerModel.PlayerLevel == LevelListConfig.LevelCount - 1)
            {
                WindowManager.ShowWindow<FinishWindow>(new FinishWindowModel());
            }
            else
            {
                WindowManager.ShowWindow<WinWindow>(new WinWindowModel());
            }

            IsLocked = false;
        }

        public bool ApplyTurn(int turns)
        {
            StepsLeft -= turns;
            TurnsChanged?.Invoke(StepsLeft);
            if (StepsLeft <= 0)
            {
                PlayLoseFlow().Forget();
                return false;
            }

            return true;
        }

        private async UniTask PlayLoseFlow()
        {
            IsLocked = true;
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
            IsLocked = false;
            WindowManager.ShowWindow<TryAgainWindow>(new TryAgainWindowModel(this));
        }
    }
}