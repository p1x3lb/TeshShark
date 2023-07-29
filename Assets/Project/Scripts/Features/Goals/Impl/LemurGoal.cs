using System;
using UnityEngine;

namespace Project.Scripts.Core.Goals
{
    [Serializable]
    public class LemurGoal : IGoal
    {
        [SerializeField]
        private int _toDefeat = 1;

        public GoalModel Produce()
        {
            return new LemurGoalModel(_toDefeat);
        }
    }

    public class LemurGoalModel : GoalModel
    {
        protected override int Aim { get; }

        public LemurGoalModel(int toDefeat)
        {
            Aim = toDefeat;
        }

        protected override void OnInitialize()
        {
            LemurContent.Destroyed += OnDestroyed;
        }

        private void OnDestroyed()
        {
            Fire();
        }

        protected override void OnDispose()
        {
            LemurContent.Destroyed -= OnDestroyed;
        }
    }
}