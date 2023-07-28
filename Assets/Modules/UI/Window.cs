using UnityEngine;

namespace UI
{
    public abstract class WindowModel
    {
    }

    public abstract class Window : MonoBehaviour
    {
        private WindowModel _model;

        internal WindowModel Model
        {
            get => _model;
            private set
            {
                if (_model == value)
                {
                    return;
                }

                _model = value;

                if (value != null)
                {
                    OnModelRemoved();
                    return;
                }

                OnInitialize();
            }
        }

        public void Initialize(WindowModel model)
        {
            Model = model;
        }

        public void OnDestroy()
        {
            Model = null;
        }

        protected virtual void OnInitialize()
        {
        }

        protected virtual void OnModelRemoved()
        {
        }
    }

    public abstract class Window<T> : Window where T : WindowModel
    {
        public new T Model => (T) base.Model;
    }
}