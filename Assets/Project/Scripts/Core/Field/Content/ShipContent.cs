using Zenject;

namespace Project.Scripts.Core
{
    public class ShipContent : BaseShipContent
    {
        [Inject]
        private CoreStateContext CoreStateContext { get; }

        public override bool IsWalkable => true;

        public void Process(CellView cell)
        {
            switch (cell.Content)
            {
                case EnemyShipContent enemyShip:
                    if (enemyShip.TryDamage(CoreStateContext.Damage))
                    {
                        enemyShip.DestroyShip();
                        cell.SetContent(null);
                    }
                    break;
            }
        }
    }
}