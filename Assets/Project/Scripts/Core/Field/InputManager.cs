using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Core;
using Project.Scripts.Core.Field;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Project.Scripts.Infrastructure
{
    public class InputManager : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IDisposable, IInitializable
    {
        public event Action<int> RouteUpdated;

        [Inject]
        private CoreStateContext CoreStateContext { get; }

        [Inject]
        private ContentListConfig ContentListConfig { get; }

        [Inject]
        private GoalsManager GoalsManager { get; }

        [SerializeField]
        private Camera _camera;

        private readonly List<CellView> _selected = new();

        private bool _isTouching;
        private bool _selecting;
        private CellView _lastSelected;
        private readonly RaycastHit2D[] _results = new RaycastHit2D[5];
        private CancellationTokenSource _cancellationTokenSource;
        private BfsShortestPath _bgsShortPath;

        public BfsShortestPath BfsShortestPath => _bgsShortPath ??= new BfsShortestPath(CoreStateContext.Cells);

        public bool IsLocked { get; set; }

        public void Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        #region Input

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
                if (!hit.collider.TryGetComponent(out CellView selectable)) continue;

                if (!_selected.Contains(selectable))
                {
                    var last = _selected.Last();
                    if (selectable.IsClose(last) && selectable.Walkable && CoreStateContext.Speed > _selected.Count - 1)
                    {
                        last.SetNext(selectable);
                        selectable.Select();
                        _selected.Add(selectable);
                        _lastSelected = selectable;
                        RouteUpdated?.Invoke(_selected.Count - 1);
                    }

                    break;
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
                    RouteUpdated?.Invoke(_selected.Count - 1);
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
                ProcessTurn(ship, _selected.ToList()).Forget();
            }

            _selected.Clear();
            RouteUpdated?.Invoke(0);
            _lastSelected = null;
            _selecting = false;
        }

        #endregion

        private async UniTask ProcessTurn(ShipContent ship, List<CellView> points)
        {
            IsLocked = true;

            CoreStateContext.StepsLeft--;

            await ProcessPlayerStep(ship, points);
            await ProcessShipsStep();
            await ProcessSpawnStep();

            IsLocked = false;
        }

        private async UniTask ProcessSpawnStep()
        {
            Find<ShipContent>(out int column, out int row);
            var shipCell = CoreStateContext.Cells[row, column];
            foreach (var goalModel in GoalsManager.ActiveGoals.Where(goal => goal is ISpawnableGoal && !goal.IsCompleted))
            {
                var spawnableGoal = goalModel as ISpawnableGoal;
                var goalTargets = CoreStateContext.CellsEnumerable.Count(cell => cell.Content != null && cell.Content.GetType() == spawnableGoal.ContentType);
                if (goalTargets == 0)
                {
                    var cell = CoreStateContext.CellsEnumerable.FirstOrDefault(cell =>
                    {
                        var range = (cell.Position - shipCell.Position);
                        return cell.Content == null &&
                               Mathf.Abs(range.x) >= ContentListConfig.MinRange &&
                               Mathf.Abs(range.y) >= ContentListConfig.MinRange &&
                               Mathf.Abs(range.x) <= ContentListConfig.MaxRange &&
                               Mathf.Abs(range.y) <= ContentListConfig.MaxRange;
                    });

                    var content = CoreStateContext.Container.InstantiatePrefabForComponent<CellContent>(ContentListConfig.GetContent(spawnableGoal.ContentType), cell.Position, Quaternion.identity, CoreStateContext.Map);
                    cell.SetContent(content);
                }
            }
        }

        private async UniTask ProcessShipsStep()
        {
            Find<ShipContent>(out var column, out var row);

            var cellViews = CoreStateContext.CellsEnumerable.Where(cell => cell.Content is EnemyShipContent enemyShipContent).ToList();
            foreach (var cell in cellViews)
            {
                var enemyShipContent = cell.Content as EnemyShipContent;

                Find(cell, out var currColumn, out var currRow);

                var path = BfsShortestPath.FindShortestPath(currRow, currColumn, row, column);

                CellView endCell = null;

                for (int i = 1; i < enemyShipContent.Speed + 1 && i < path.Count - 1; i++)
                {
                    var (x, y) = path[i];
                    endCell = CoreStateContext.Cells[x, y];
                    await endCell.Travel(enemyShipContent, _cancellationTokenSource.Token);
                }

                if (endCell != null)
                {
                    cell.SetContent(null);
                    endCell.SetContent(enemyShipContent);

                    if (CoreStateContext.Cells[column, row].IsClose(endCell) && !CoreStateContext.ApplyTurn(enemyShipContent.Damage))
                    {
                        break;
                    }
                }
            }
        }

        private bool Find(CellView cellView, out int column, out int row)
        {
            var cells = CoreStateContext.Cells;
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    if (cells[i, j] == cellView)
                    {
                        column = j;
                        row = i;
                        return true;
                    }
                }
            }

            row = 0;
            column = 0;
            return false;
        }

        private bool Find<T>(out int column, out int row)
        {
            var cells = CoreStateContext.Cells;
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    if (cells[i, j].Content is T)
                    {
                        column = j;
                        row = i;
                        return true;
                    }
                }
            }

            row = 0;
            column = 0;
            return false;
        }

        private async UniTask ProcessPlayerStep(ShipContent ship, List<CellView> points)
        {
            for (var i = 1; i < points.Count; i++)
            {
                points[i].Clear();
                await points[i].Travel(ship, _cancellationTokenSource.Token);
                points[i].SetContent(null);

                foreach (var cell in CoreStateContext.CellsEnumerable.Except(points).Where(cell => cell.Content != null && cell.IsClose(points[i])))
                {
                    ship.Process(cell);
                }
            }

            points.First().SetContent(null);
            points.Last().SetContent(ship);
        }
    }
}