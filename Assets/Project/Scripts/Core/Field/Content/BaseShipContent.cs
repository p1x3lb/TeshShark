using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Scripts.Core.Field;
using UnityEngine;

namespace Project.Scripts.Core
{
    public abstract class BaseShipContent : CellContent
    {
        [SerializeField]
        private DirectionView _directionView;

        [SerializeField]
        private float _swimDuration;

        public async virtual UniTask Travel(Vector2 position, CancellationToken cancellationToken)
        {
            var direction = (position - new Vector2(transform.position.x, transform.position.y)).normalized;
            Debug.Log(direction);
            SetDirection(direction);
            await transform.DOMove(position, _swimDuration).ToUniTask(cancellationToken: cancellationToken);
        }

        public void SetDirection(Vector2 vector)
        {
            _directionView.Apply(vector);
        }
    }
}