using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
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
        }

        public async UniTask Travel(ShipContent ship, CancellationToken cancellationToken)
        {
            Clear();
            await ship.Travel(Position, cancellationToken);
            SetContent(null);
        }

        public bool IsClose(CellView last)
        {
            return (Position - last.Position).magnitude <= 1;
        }
    }
}