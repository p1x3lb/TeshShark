using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Features;
using Project.Scripts.Infrastructure;
using UI;
using Zenject;

namespace Project.Scripts.Core
{
    public class GoalsManager
    {
        [Inject]
        private PlayerModel PlayerModel { get; }

        [Inject]
        private WindowManager WindowManager { get; }

        public List<GoalModel> ActiveGoals { get; } = new List<GoalModel>();

        public void Initialize(IReadOnlyList<IGoal> goalConfigs)
        {
            foreach (var goalConfig in goalConfigs)
            {
                var goal = goalConfig.Produce();
                goal.Initialize();
                goal.Complete += OnGoalComplete;
                ActiveGoals.Add(goal);
            }
        }

        private void OnGoalComplete(GoalModel obj)
        {
            if (ActiveGoals.All(goal => goal.IsCompleted))
            {
                PlayerModel.CompleteLevel();
                WindowManager.ShowWindow<WinWindow>(new WinWindowModel());
            }
        }

        public void Clear()
        {
            foreach (var goal in ActiveGoals)
            {
                goal.Dispose();
            }

            ActiveGoals.Clear();
        }
    }
}