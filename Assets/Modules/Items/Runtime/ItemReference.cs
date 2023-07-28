using System;
using System.Runtime.Serialization;
using Modules.Core.Common;

namespace Items
{
    [Serializable]
    public class ItemReference : AssetReference<Item>
    {
        public ItemReference(string guid) : base(guid)
        {
        }

        public ItemReference(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}