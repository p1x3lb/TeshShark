using System.Collections.Generic;
using Modules.PlayerRecord;

namespace Items
{
    public class ItemProfile : ProfileRecord
    {
        public List<ConsumableRecord> ConsumableRecords { get; }
        public List<PermanentRecord> PermanentRecord { get; }

       // public void
    }

    public class ConsumableRecord : ProfileRecord
    {
        public ItemReference ItemReference { get; }
        public int Count { get; }
    }

    public class PermanentRecord : ProfileRecord
    {
        public ItemReference ItemReference { get; }
    }
}