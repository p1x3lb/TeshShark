using System;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class EnemyShipContent : CellContent
    {
        public static event Action Destroyed;

        [SerializeField]
        private int _health = 1;

        public int Health => _health;
        public override bool IsWalkable => false;

        public bool TryDamage(int damage)
        {
            _health -= damage;

            if (_health <= 0 )
            {
                Destroyed?.Invoke();
                return true;
            }

            return false;
        }
    }
}