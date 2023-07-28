using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Utils.Project.Scripts.Modules.Utils.UI.LayoutListUtil
{
    public abstract class LayoutList<T,K> : MonoBehaviour where T : LayoutListItem<K>
    {
        [SerializeField] private LayoutGroup _container;
        [SerializeField] private T _prefab;
        
        private IEnumerable<K> _data;
        private IInstantiator _instantiator;
        private LayoutListPool<T> _pool;
        private readonly List<T> _activeReferences = new List<T>();
        public List<T> ActiveReferences => _activeReferences;
        
        [Inject]
        private void Construct(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }
        
        private void Awake()
        {
            _pool = new LayoutListPool<T>(_instantiator, _container.transform, _prefab);
        }
        
        public void SetData(IEnumerable<K> data)
        {
            if (_data != null)
            {
                Clear();
            }
            _data = data;
            Initialize();
        }

        public void Clear()
        {
            foreach (var reference in _activeReferences)
            {
                UnSubscribeFromItem(reference);
                reference.Release();
            }
            _activeReferences.Clear();
        }
        
        private void Initialize()
        {
            Clear();
            foreach (var dataItem in _data)
            {
                var reference  = _pool.Get();
                reference.SetData(dataItem);
                SubscribeOnItem(reference);
            }  
        }

        protected virtual void SubscribeOnItem(T item)
        {
            
        }
        
        protected virtual void UnSubscribeFromItem(T item)
        {
            
        }
    }
}