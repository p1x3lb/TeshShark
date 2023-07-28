using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils.Project.Scripts.Modules.Utils.InRangeWatcher
{
    public class BaseInRangeWatcher<T> : MonoBehaviour where T : IInRangeObservable
    {
        [SerializeField]
        private Collider _collider;

        private readonly List<T> _inRangeUnits = new();
        
        public T First => _inRangeUnits.FirstOrDefault();
        public bool HasUnit => _inRangeUnits.Any();
        public IReadOnlyList<T> InRangeUnits => _inRangeUnits;

        protected Collider Collider => _collider;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out T unit) && !_inRangeUnits.Contains(unit))
            {
                Enter(unit);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out T unit))
            {
                Exit(unit);
            }
        }
        
        private void Enter(T unit)
        {
            _inRangeUnits.Add(unit);
            unit.Missed += OnMissed;
            OnEnterInternal(unit);
        }
        
        private void Exit(T unit)
        {
            _inRangeUnits.Remove(unit);
            unit.Missed -= OnMissed;
            OnExitInternal(unit);
        }

        protected virtual void OnExitInternal(T unit)
        {
          
        }
        
        protected virtual void OnEnterInternal(T unit)
        {
          
        }

        private void OnMissed(IInRangeObservable inRangeObservable)
        {
            if (inRangeObservable is T unit)
            {
                Exit(unit);
            }
        }
        
        private void OnDisable()
        {
            foreach (var unit in _inRangeUnits)
            {
                unit.Missed -= OnMissed;
            }
            _inRangeUnits.Clear();
        }
    }
}