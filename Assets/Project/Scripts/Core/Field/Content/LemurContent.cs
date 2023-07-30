using System;

namespace Project.Scripts.Core
{
    public class LemurContent : CellContent
    {
        public static event Action Destroyed;

        public override bool IsWalkable => true;

        public override void OnEnter()
        {
            Destroyed?.Invoke();
            Destroy(gameObject);
        }
    }
}