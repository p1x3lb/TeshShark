using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class EnemyShipContent : BaseShipContent
    {
        public static event Action Destroyed;

        [SerializeField]
        private TMP_Text _counter;

        [SerializeField]
        private GameObject _counterGO;

        [SerializeField]
        private int _health = 1;

        [SerializeField]
        private int _speed = 3;

        [SerializeField]
        private int _damage = 1;

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private Transform _cannonBall;

        [SerializeField]
        private GameObject _cannonShot;

        public int Health => _health;
        public override bool IsWalkable => false;

        public int Speed => _speed;
        public int Damage => _damage;

        public bool TryDamage(int damage)
        {
            _health -= damage;
            UpdateHealth();

            if (_health <= 0)
            {
                _counterGO.gameObject.SetActive(false);
                Destroyed?.Invoke();
                return true;
            }

            return false;
        }

        private void OnEnable()
        {
            UpdateHealth();
        }

        private void UpdateHealth()
        {
            _counter.text = _health.ToString();
        }

        public void DestroyShip()
        {
            Destroy(gameObject);
        }

        public async UniTask ShowDmg(Transform target)
        {
            if (_animator == null)
            {
                _cannonBall.gameObject.SetActive(true);
                _cannonShot.SetActive(true);
                _cannonBall.transform.position = transform.position;
                await _cannonBall.DOJump(target.position, 1, 1, 0.25f).SetEase(Ease.InOutQuad).ToUniTask();
                _cannonBall.gameObject.SetActive(false);
                _cannonShot.SetActive(false);
            }
            else
            {
                _animator.SetTrigger("Attack");
                await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
            }
        }
    }
}