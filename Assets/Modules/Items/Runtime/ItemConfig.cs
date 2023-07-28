using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
    public class ItemConfig : ScriptableObject
    {
        [SerializeField]
        private List<Item> _items;

        public List<Item> Items => _items;
    }
}