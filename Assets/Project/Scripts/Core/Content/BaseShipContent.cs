using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.Core
{
    public abstract class BaseShipContent : CellContent
    {
        [SerializeField]
        private int _speed = 3;

        [SerializeField]
        private int _damage = 1;

        [SerializeField]
        private float _swimDuration;

        public int Speed => _speed;
        public int Damage => _damage;

        public async virtual UniTask Travel(Vector2 position, CancellationToken cancellationToken)
        {
            await transform.DOMove(position, _swimDuration).ToUniTask(cancellationToken: cancellationToken);
        }
    }
}