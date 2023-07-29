using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class ShipContent : CellContent
    {
        [SerializeField]
        private float _swimDuration;

        public override bool IsSelectable => true;

        public async UniTask Travel(Vector2 position, CancellationToken cancellationToken)
        {
            await transform.DOMove(position, _swimDuration).ToUniTask(cancellationToken: cancellationToken);
        }
    }
}