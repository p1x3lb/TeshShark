using System;

namespace Project.Scripts.Core
{
    public class TorchContent : BonusContent
    {
        public static event Action Destroyed;

        public override bool IsWalkable => true;

        public override void OnEnter()
        {
            base.OnEnter();
            Destroyed?.Invoke();
        }
    }
}