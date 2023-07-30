using System;
using UnityEngine;

namespace Project.Scripts.Core.Goals
{
    [Serializable]
    public class BarrelGoal : IGoal
    {
        [SerializeField]
        private int _toDefeat = 1;

        public GoalModel Produce()
        {
            return new BarrelGoalModel(_toDefeat);
        }
    }

    public class BarrelGoalModel : GoalModel
    {
        public override Type ContentType => typeof(BarrelContent);

        public override int Target { get; }

        public BarrelGoalModel(int toDefeat)
        {
            Target = toDefeat;
        }

        protected override void OnInitialize()
        {
            BarrelContent.Destroyed += OnDestroyed;
        }

        private void OnDestroyed()
        {
            Fire();
        }

        protected override void OnDispose()
        {
            BarrelContent.Destroyed -= OnDestroyed;
        }
    }
}