using UnityEngine;

namespace Project.Scripts.Core
{
    public abstract class CellContent : MonoBehaviour
    {
        public abstract bool IsSelectable { get; }

        public virtual void OnSpawn()
        {

        }

        public virtual void OnRemoved()
        {

        }
    }
}