using UnityEngine;

namespace Project.Scripts.Core
{
    public abstract class CellContent : MonoBehaviour
    {
        public abstract bool IsWalkable { get; }

        public virtual void OnSpawn()
        {

        }

        public virtual void OnRemoved()
        {

        }
    }
}