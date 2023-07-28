using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils.Project.Scripts.Modules.Utils.Pool
{
    public class DefaultMonoBehaviourPool<T> : AbstractPool<T> where T : ObjectPoolItem
    {
        private T _prefab;
        private Transform _transform;

        public void Initialise(T healthBar, Transform defaultParent = null)
        {
            _prefab = healthBar;
            _transform = defaultParent;
        }
        
        protected override T CreateItem()
        {
            return Object.Instantiate(_prefab, _transform);
        }
    }
}