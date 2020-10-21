using System;

namespace Maple2.Data.Types {
    public class Account {
        public DateTime LastModified;

        public long Id;
        public long Merets;
        public long Mesos; // Storage mesos
        public int MaxCharacters;
        public int PrestigeLevel;
        public long PrestigeExperience;
        public long PremiumTime;
    }
}