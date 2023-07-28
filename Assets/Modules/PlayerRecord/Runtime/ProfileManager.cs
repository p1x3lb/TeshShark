using Newtonsoft.Json;
using Utils.Modules.Utils;
using Zenject;

namespace Modules.PlayerRecord
{
    public class ProfileManager : ILateTickable, IInitializable
    {
        private static readonly string ProfileName = $"{nameof(Profile)}.json";

        public Profile Profile { get; private set; }

        public void Initialize()
        {
            Profile ??= LoadProfile();
        }

        private Profile LoadProfile()
        {
            if (!Persistent.Exists(ProfileName)) return new Profile();

            var json = Persistent.Load(ProfileName);
            return JsonConvert.DeserializeObject<Profile>(json);
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(Profile);
            Persistent.Save(ProfileName, json);
        }

        void ILateTickable.LateTick()
        {
            if (ProfileRecord.IsDirty)
            {
                Save();
                ProfileRecord.IsDirty = false;
            }
        }
    }
}