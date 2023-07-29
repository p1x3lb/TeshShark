using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public class InputManager : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IInitializable
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

        public void OnDrag(PointerEventData eventData)
        {
            if (!_selecting || Input.touches.Length != 1) return;

            var size = Physics2D.RaycastNonAlloc(_camera.ScreenToWorldPoint(eventData.position), Vector2.zero, _results);

            if (size == 0)
            {
            }

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

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Input.touches.Length != 1) return;

            var size = Physics2D.RaycastNonAlloc(_camera.ScreenToWorldPoint(eventData.position), Vector2.zero, _results);

            for (var i = 0; i < size; i++)
            {
                var hit = _results[i];
                if (!hit.collider.TryGetComponent(out ITouchable touchable) || !touchable.TrySelect()) continue;
                _selecting = true;
                return;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (Input.touches.Length != 0) return;

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