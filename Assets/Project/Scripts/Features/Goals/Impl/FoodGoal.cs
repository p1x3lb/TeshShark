using System;
using UnityEngine;

namespace Project.Scripts.Core.Goals
{
    [Serializable]
    public class FoodGoal : IGoal
    {
        [SerializeField]
        private int _toDefeat = 1;

        public GoalModel Produce()
        {
            return new FoodGoalModel(_toDefeat);
        }
    }

    public class FoodGoalModel : GoalModel
    {
        protected override int Aim { get; }

        public FoodGoalModel(int toDefeat)
        {
            Aim = toDefeat;
        }

        protected override void OnInitialize()
        {
            FoodContent.Destroyed += OnDestroyed;
        }

        private void OnDestroyed()
        {
            Fire();
        }

        protected override void OnDispose()
        {
            FoodContent.Destroyed -= OnDestroyed;
        }
    }
}