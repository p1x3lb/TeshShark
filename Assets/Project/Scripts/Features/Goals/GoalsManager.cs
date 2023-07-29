using System.Collections.Generic;

namespace Project.Scripts.Core
{
    public class GoalsManager
    {
        public List<GoalModel> ActiveGoals { get; } = new List<GoalModel>();

        public void Initialize(IReadOnlyList<IGoal> goalConfigs)
        {
            foreach (var goalConfig in goalConfigs)
            {
                var goal = goalConfig.Produce();
                goal.Initialize();
                ActiveGoals.Add(goal);
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