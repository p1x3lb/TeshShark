using System;
using UnityEngine;

namespace Items
{

    [Serializable]
    public class PermanentPack : Pack, IConsumablePack
    {

    }

    [Serializable]
    public class ConsumablePack : Pack, IConsumablePack
    {
        [SerializeField]
        private int _amount;

        public int Amount => _amount;
    }

    [Serializable]
    public class Pack : IPack
    {
        [SerializeField]
        private ItemReference _itemReference;

        public ItemReference Reference => _itemReference;
    }

    public interface IConsumablePack : IPack
    {

    }

    public interface IPack
    {

    }
}