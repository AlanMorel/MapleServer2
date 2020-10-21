using System;
using System.Collections.Generic;
using Maple2.Data.Types.Items;

namespace Maple2.Data.Types {
    public class MailInfo {
        public long Id;
        public long SenderId; // Player CharacterId
        public long RecipientId; // Player CharacterId
        public byte Type; // 1 = Regular Mail, 101+? = Special Mail, 200 = Ad
        public string SenderName;
        public string Title;
        public string Content;
        public string MetadataKey;
        public string MetadataValue;

        public long CreationTime;
        public long ExpiryTime;
        public long ReadTime;

        // More than 1 item may not display properly
        public IList<Item> Items;

        public MailInfo(TimeSpan duration = default) {
            if (duration != default) {
                ExpiryTime = (DateTimeOffset.UtcNow + duration).ToUnixTimeSeconds();
            }

            SenderName = "";
            Title = "";
            Content = "";
            MetadataKey = "";
            MetadataValue = "";
            Items = new List<Item>();
        }
    }
}