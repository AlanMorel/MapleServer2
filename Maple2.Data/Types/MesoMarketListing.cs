using System;

namespace Maple2.Data.Types {
    public class MesoMarketListing {
        public long Id;
        public long AccountId;
        public long CreationTime;
        public long ExpiryTime;
        public long Price;
        public long Mesos;

        public MesoMarketListing(TimeSpan duration = default) {
            if (duration != default) {
                ExpiryTime = (DateTimeOffset.UtcNow + duration).ToUnixTimeSeconds();
            }
        }
    }
}