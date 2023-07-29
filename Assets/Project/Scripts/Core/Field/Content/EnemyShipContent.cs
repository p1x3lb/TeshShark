using UnityEngine;

namespace Project.Scripts.Core
{
    public class EnemyShipContent : CellContent
    {
        [SerializeField]
        private int _health = 1;

        public int Health { get; set; }

        public override bool IsWalkable => false;
    }
}