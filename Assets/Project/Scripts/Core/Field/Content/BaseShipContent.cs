using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.Core
{
    public abstract class BaseShipContent : CellContent
    {

        [SerializeField]
        private float _swimDuration;

        public async virtual UniTask Travel(Vector2 position, CancellationToken cancellationToken)
        {
            await transform.DOMove(position, _swimDuration).ToUniTask(cancellationToken: cancellationToken);
        }
    }
}