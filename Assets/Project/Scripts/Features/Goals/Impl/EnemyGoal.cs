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
        public override Type ContentType => typeof(EnemyShipContent);

        public override int Target { get; }

        public EnemyGoalModel(int toDefeat)
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