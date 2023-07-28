using System;
using Newtonsoft.Json;

namespace Modules.PlayerRecord
{
    [Serializable]
    public abstract class ProfileRecord
    {
        [JsonIgnore]
        public static bool IsDirty { get; internal set; }

        protected ProfileRecord()
        {
            SetDirty();
        }

        public void SetDirty()
        {
            IsDirty = true;
        }
    }
}