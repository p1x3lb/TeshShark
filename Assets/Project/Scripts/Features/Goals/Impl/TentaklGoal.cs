using System;
using UnityEngine;

namespace Project.Scripts.Core.Goals
{
    [Serializable]
    public class TentaklGoal : IGoal
    {
        [SerializeField]
        private int _toDefeat = 1;

        public GoalModel Produce()
        {
            return new TentaklGoalModel(_toDefeat);
        }
    }

    public class TentaklGoalModel : GoalModel
    {
        public override Type ContentType => typeof(TentataklContent);

        public override int Target { get; }

        public TentaklGoalModel(int toDefeat)
        {
            Target = toDefeat;
        }

        protected override void OnInitialize()
        {
            EnemyShipContent.Destroyed += OnDestroyed;
        }

        private void OnDestroyed()
        {
            Fire();
        }

        protected override void OnDispose()
        {
            EnemyShipContent.Destroyed -= OnDestroyed;
        }
    }
}