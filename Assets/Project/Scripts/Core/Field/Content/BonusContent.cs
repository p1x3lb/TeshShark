using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class BonusContent : CellContent
    {
        [SerializeField]
        private GameObject _effect;

        [SerializeField]
        private Transform _view;

        public override bool IsWalkable => true;

        public override void OnEnter()
        {
            DestroyFlow().Forget();
        }

        private async UniTask DestroyFlow()
        {
            _effect.gameObject.SetActive(false);
            _effect.gameObject.SetActive(true);
            await _view.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InQuad);
            Destroy(this);
        }
    }
}