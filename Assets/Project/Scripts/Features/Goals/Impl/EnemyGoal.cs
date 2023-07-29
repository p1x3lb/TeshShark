using System;
using UnityEngine;

namespace Project.Scripts.Core.Goals
{
    [Serializable]
    public class EnemyGoal : IGoal
    {
        [SerializeField]
        private int _toDefeat = 1;

        public GoalModel Produce()
        {
            return new EnemyGoalModel(_toDefeat);
        }
    }

    public class EnemyGoalModel : GoalModel
    {
        protected override int Aim { get; }

        public EnemyGoalModel(int toDefeat)
        {
            Aim = toDefeat;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}