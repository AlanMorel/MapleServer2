using System;
using Maple2Storage.Enums;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class AdBalloon
    {
        public string Name;
        public int InteractId;
        public string Model;
        public string Asset;
        public string NormalState;
        public string Reactable;
        public float Scale;
        public Player Owner;
        public string Title;
        public string Description;
        public bool PublicHouse;
        public InteractObjectType Type;
        public long CreationTimestamp;
        public long ExpirationTimestamp; // TODO: Remove from field if expired

        public AdBalloon(Player owner, Item item, string title, string description, bool publicHouse)
        {
            int nameId = GuidGenerator.Int();
            Name = "BillBoard_" + nameId.ToString();
            Owner = owner;
            InteractId = item.AdBalloon.InteractId;
            Model = item.AdBalloon.Model;
            Asset = item.AdBalloon.Asset;
            NormalState = item.AdBalloon.NormalState;
            Reactable = item.AdBalloon.Reactable;
            Scale = item.AdBalloon.Scale;
            Title = title;
            Description = description;
            PublicHouse = publicHouse;
            Type = InteractObjectType.AdBalloon;
            CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount;
            ExpirationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount + item.AdBalloon.Duration;
        }
    }
}
