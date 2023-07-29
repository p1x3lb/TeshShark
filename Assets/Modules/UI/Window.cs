using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI
{
    public abstract class WindowModel
    {
        internal UniTaskCompletionSource ShowSource { get; } = new UniTaskCompletionSource();
        internal UniTaskCompletionSource HideSource { get; } = new UniTaskCompletionSource();
        internal CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();

        public UniTask Show => ShowSource.Task;
        public UniTask Hide => HideSource.Task;
        public CancellationToken CancellationToken => CancellationTokenSource.Token;

        protected WindowModel()
        {
            CancellationTokenSource.Token.Register(() =>
            {
                ShowSource.TrySetCanceled();
                HideSource.TrySetCanceled();
            });
        }

        public void Destroy()
        {
            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
        }
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
            Show().Forget();
        }

        public async UniTask Show()
        {
            await OnShow();
            Model.ShowSource.TrySetResult();
        }

        public async UniTask Hide()
        {
            await OnHide();
            Model.HideSource.TrySetResult();
            Model.Destroy();
            Model = null;
        }

        protected virtual UniTask OnShow()
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask OnHide()
        {
            return UniTask.CompletedTask;
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
        public new T Model => (T)base.Model;
    }
}