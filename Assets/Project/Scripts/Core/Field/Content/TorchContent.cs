using System;

namespace Project.Scripts.Core
{
    public class TorchContent : CellContent
    {
        public static event Action Destroyed;

        public override bool IsWalkable => false;

        public override void OnRemoved()
        {
            Destroyed?.Invoke();
            Destroy(gameObject);
        }
    }
}