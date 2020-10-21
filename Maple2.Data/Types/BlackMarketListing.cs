using System;
using Maple2.Data.Types.Items;

namespace Maple2.Data.Types {
    public class BlackMarketListing {
        public long Id;
        public long AccountId;
        public long CharacterId;
        public long CreationTime;
        public long ExpiryTime;
        public long Price;
        public Item Item;

        public BlackMarketListing(TimeSpan duration = default) {
            if (duration != default) {
                ExpiryTime = (DateTimeOffset.UtcNow + duration).ToUnixTimeSeconds();
            }
        }
    }
}