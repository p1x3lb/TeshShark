using System;

namespace Project.Scripts.Core
{
    public interface IGoal
    {
        GoalModel Produce();
    }

    public abstract class GoalModel : IDisposable
    {
        public event Action<GoalModel> Complete;
        public event Action<GoalModel> Updated;

        private int _current;

        protected abstract int Aim { get; }

        public void Initialize()
        {
            _current = 0;
            OnInitialize();
        }

        public void Dispose()
        {
            OnDispose();
            Complete = null;
            Updated = null;
        }

        protected virtual void OnDispose()
        {
        }

        protected virtual void OnInitialize()
        {

        }

        protected void Fire()
        {
            _current++;
            Updated?.Invoke(this);

            if (_current == Aim)
            {
                Complete?.Invoke(this);
            }
        }
    }
}