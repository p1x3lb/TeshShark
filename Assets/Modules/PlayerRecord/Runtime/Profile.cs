using System;
using System.Collections.Generic;

namespace Modules.PlayerRecord
{
    [Serializable]
    public class Profile
    {
        public List<ProfileRecord> Records { get; } = new();

        public T Get<T>() where T : ProfileRecord, new()
        {
            foreach (var record in Records)
            {
                if (record is T typedRecord)
                {
                    return typedRecord;
                }
            }

            var createdRecord = new T();
            Records.Add(createdRecord);

            return createdRecord;
        }
    }
}