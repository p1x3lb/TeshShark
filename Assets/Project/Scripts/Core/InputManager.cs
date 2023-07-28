using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Infrastructure
{
    public interface ISelectable
    {
        void Select();
        void Clear();
        bool Available { get; }
    }

    public interface ITouchable
    {
        bool TrySelect();
    }

    public class InputManager : ITickable, IInitializable
    {
        private readonly List<ISelectable> _selected = new();

        private Camera _camera;
        private bool _isTouching;
        private bool _selecting;
        private ISelectable _lastSelected;
        private readonly RaycastHit2D[] _results = new RaycastHit2D[5];

        public void Initialize()
        {
            _camera = Camera.current;
        }

        void ITickable.Tick()
        {
            var touchesLength = Input.touches.Length;
            if (touchesLength == 0)
            {
                if (_isTouching)
                {
                    OnPointUp();
                }

                _isTouching = false;
                return;
            }

            if (touchesLength > 1) return;

            var touch = Input.touches[0];

            if (!_isTouching)
            {
                OnPointDown(touch.position);
                return;
            }

            OnPointMove(touch.position);

            _isTouching = true;
        }

        private void OnPointMove(Vector2 touchPosition)
        {
            if (!_selecting) return;

            var size = Physics2D.RaycastNonAlloc(_camera.ScreenToWorldPoint(touchPosition), Vector2.zero, _results);

            if (size == 0) { }

            for (var i = 0; i < size; i++)
            {
                var hit = _results[i];
                if (!hit.collider.TryGetComponent(out ISelectable selectable) || !selectable.Available) continue;

                if (_lastSelected != selectable && _selected.Contains(selectable))
                {
                    var index = _selected.IndexOf(selectable) + 1;
                    for (var j = index; j < _selected.Count; j++)
                    {
                        _selected[j].Clear();
                    }
                    _selected.RemoveRange(index, _selected.Count - index);
                    _lastSelected = selectable;
                    return;
                }

                selectable.Select();
                _selected.Add(selectable);
            }
        }

        private void OnPointDown(Vector2 touchPosition)
        {
            var size = Physics2D.RaycastNonAlloc(_camera.ScreenToWorldPoint(touchPosition), Vector2.zero, _results);

            for (var i = 0; i < size; i++)
            {
                var hit = _results[i];
                if (!hit.collider.TryGetComponent(out ITouchable touchable) || !touchable.TrySelect()) continue;
                _selecting = true;
                return;
            }
        }

        private void OnPointUp()
        {
            foreach (var selectable in _selected)
            {
                Process(selectable);
                selectable.Clear();
            }
            _lastSelected = null;
            _selecting = false;
        }

        private void Process(ISelectable selectable)
        {

        }
    }
}