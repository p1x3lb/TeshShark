using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.Scripts.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Project.Scripts.Infrastructure
{

    public class InputManager : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Inject]
        private CoreStateContext CoreStateContext { get; }

        [SerializeField]
        private Camera _camera;

        private readonly List<CellView> _selected = new();

        private bool _isTouching;
        private bool _selecting;
        private CellView _lastSelected;
        private readonly RaycastHit2D[] _results = new RaycastHit2D[5];

        public bool IsLocked { get; set; }

        public void OnDrag(PointerEventData eventData)
        {
#if !UNITY_EDITOR
            if (Input.touches.Length != 1) return;
#endif
            if (!_selecting) return;

            var screenToWorldPoint = _camera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y));
            var size = Physics2D.RaycastNonAlloc(screenToWorldPoint, Vector2.zero, _results);

            if (size == 0) return;

            for (var i = 0; i < size; i++)
            {
                var hit = _results[i];
                if (!hit.collider.TryGetComponent(out CellView selectable) || !selectable.Available) continue;

                if (!_selected.Contains(selectable))
                {
                    var last = _selected.Last();
                    if (selectable.IsClose(last))
                    {
                        last.SetNext(selectable);
                        selectable.Select();
                        _selected.Add(selectable);
                        _lastSelected = selectable;
                        break;
                    }
                }

                if (_lastSelected != selectable)
                {
                    var index = _selected.IndexOf(selectable) + 1;
                    for (var j = index; j < _selected.Count; j++)
                    {
                        _selected[j].Clear();
                    }

                    _selected.RemoveRange(index, _selected.Count - index);
                    _lastSelected = selectable;
                    _lastSelected.SetNext(null);
                    break;
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
#if !UNITY_EDITOR
            if (Input.touches.Length != 1) return;
#endif
            if (IsLocked) return;

            var screenToWorldPoint = _camera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y));
            var size = Physics2D.RaycastNonAlloc(screenToWorldPoint, Vector2.zero, _results);

            for (var i = 0; i < size; i++)
            {
                var hit = _results[i];

                if (!hit.collider.TryGetComponent(out CellView touchable)) continue;

                _selected.Add(touchable);
                touchable.OnTap();

                if (touchable.IsSelectable)
                {
                    _selecting = true;
                }

                return;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
#if !UNITY_EDITOR
              if (Input.touches.Length != 0) return;
#endif

            if (!_selecting) return;

            if (_selected.Count > 1 && _selected[0]?.Content is ShipContent ship)
            {
                Travel(ship, _selected.ToList()).Forget();
            }

            _selected.Clear();
            _lastSelected = null;
            _selecting = false;
        }

        private async UniTask Travel(ShipContent ship, List<CellView> points)
        {
            CoreStateContext.StepsLeft--;

            for (var i = 1; i < points.Count; i++)
            {
                await _selected[i].Travel(ship, default);
            }

            foreach (var selectable in _selected)
            {
                selectable.Clear();
            }
        }
    }
}