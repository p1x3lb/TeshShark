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

        public bool IsCompleted => Current == Target;
        public abstract int Target { get; }
        public int Current { get; private set; }

        public void Initialize()
        {
            Current = 0;
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

        protected virtual void OnComplete()
        {
            Dispose();
        }

        protected void Fire()
        {
            Current++;
            Updated?.Invoke(this);

            if (IsCompleted)
            {
                Complete?.Invoke(this);
                OnComplete();
            }
        }
    }
}