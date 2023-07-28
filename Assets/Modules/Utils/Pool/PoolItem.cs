namespace Utils.Project.Scripts.Modules.Utils.Pool
{
    public class PoolItem : IPoolItem
    {
        public bool IsFree { get; private set; }
        
        public virtual void Load()
        {
            Release();
        }

        public virtual void Retain()
        {
            IsFree = false;
        }

        public virtual void Release()
        {
            IsFree = true;
        }
    }
}