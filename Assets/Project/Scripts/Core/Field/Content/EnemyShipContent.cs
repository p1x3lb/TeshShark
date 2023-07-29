using System;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class EnemyShipContent : BaseShipContent
    {
        public static event Action Destroyed;

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

            if (_health <= 0)
            {
                Destroyed?.Invoke();
                return true;
            }

            return false;
        }

        public void DestroyShip()
        {
           Destroy(gameObject);
        }
    }
}