using System;
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
    }
}