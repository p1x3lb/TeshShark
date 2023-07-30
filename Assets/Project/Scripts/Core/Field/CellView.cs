using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Core.Field;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Core
{
    public class CellView : MonoBehaviour
    {
        [Inject]
        private CoreStateContext CoreStateContext { get; }

        [SerializeField]
        private GameObject _arrows;

        [SerializeField]
        private CellContent _cellContent;

        [SerializeField]
        private DirectionView _directionView;

        public bool Walkable => _cellContent?.IsWalkable ?? true;
        public bool IsSelectable => _cellContent is ShipContent;

        public Vector2 Position => new Vector2(transform.position.x, transform.position.y);
        public CellContent Content => _cellContent;

        public void Select()
        {
            _arrows.gameObject.SetActive(true);
        }

        public void Clear()
        {
            _arrows.gameObject.SetActive(false);
        }

        public void SetContent(CellContent content)
        {
            _cellContent?.OnRemoved();

            _cellContent = content;

            _cellContent?.OnSpawn();
        }

        public void OnTap()
        {
        }

        public void SetNext(CellView selectable)
        {
            _directionView.Apply(selectable != null ? (selectable.Position - Position).normalized : Vector2.zero);
        }

        public async UniTask Travel(BaseShipContent ship, CancellationToken cancellationToken)
        {
            await ship.Travel(Position, cancellationToken);
        }

        public bool IsClose(CellView last)
        {
            var lastPosition = Position - last.Position;
            return Mathf.Abs(lastPosition.x) <= 1 && Mathf.Abs(lastPosition.y) <= 1;
        }
    }
}