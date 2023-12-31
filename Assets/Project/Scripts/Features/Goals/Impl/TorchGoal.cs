using System;
using UnityEngine;

namespace Project.Scripts.Core.Goals
{
    [Serializable]
    public class TorchGoal : IGoal
    {
        [SerializeField]
        private int _toDefeat = 1;

        public GoalModel Produce()
        {
            return new TorchGoalModel(_toDefeat);
        }
    }

    public class TorchGoalModel : GoalModel
    {
        public override Type ContentType => typeof(TorchContent);

        public override int Target { get; }

        public TorchGoalModel(int toDefeat)
        {
            Target = toDefeat;
        }

        protected override void OnInitialize()
        {
            TorchContent.Destroyed += OnDestroyed;
        }

        private void OnDestroyed()
        {
            Fire();
        }

        protected override void OnDispose()
        {
            TorchContent.Destroyed -= OnDestroyed;
        }
    }
}