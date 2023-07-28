using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField, ReadOnly, DontValidate]
        private ItemReference _reference;

        [SerializeField]
        private string _name;
        [SerializeField]
        private Sprite _sprite;

        public ItemReference Reference => _reference;

        public string Name => _name;

        public Sprite Sprite => _sprite;

        public static implicit operator ItemReference(Item item)
        {
            // ReSharper disable once MergeConditionalExpression
            return ReferenceEquals(item, null) ? null : item.Reference;
        }
    }
}