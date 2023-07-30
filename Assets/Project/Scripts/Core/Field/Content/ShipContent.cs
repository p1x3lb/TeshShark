using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Core
{
    public class ShipContent : BaseShipContent
    {
        [Inject]
        private CoreStateContext CoreStateContext { get; }

        [SerializeField]
        private Transform _cannonBall;

        [SerializeField]
        private GameObject _cannonShot;

        [SerializeField]
        private Animator _animator;

        public override bool IsWalkable => true;

        public async UniTask Process(CellView cell)
        {
            switch (cell.Content)
            {
                case EnemyShipContent enemyShip:

                    _cannonBall.gameObject.SetActive(true);
                    _cannonShot.SetActive(true);
                    _cannonBall.transform.position = transform.position;
                    await _cannonBall.DOJump(enemyShip.transform.position, 1, 1, 0.25f).SetEase(Ease.InOutQuad).ToUniTask();
                    _cannonBall.gameObject.SetActive(false);
                    _cannonShot.SetActive(false);

                    if (enemyShip.TryDamage(CoreStateContext.Damage))
                    {
                        enemyShip.DestroyShip();
                        cell.SetContent(null);
                    }

                    break;
            }
        }

        public void ToggleHighlight(bool isHighlighted)
        {
            _animator?.SetBool("IsHighlighted", isHighlighted);
        }
    }
}