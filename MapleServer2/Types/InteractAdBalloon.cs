using System;
using Maple2Storage.Enums;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class InteractAdBalloon
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
        public InteractActorType Type;
        public long CreationTimestamp;
        public long ExpirationTimestamp; // TODO: Remove from field if expired

        public InteractAdBalloon(Player owner, Item item, string title, string description, bool publicHouse)
        {
            int nameId = GuidGenerator.Int();
            Name = "BillBoard_" + nameId.ToString();
            Owner = owner;
            InteractId = item.Function.Id;
            Model = item.Function.Model;
            Asset = item.Function.Asset;
            NormalState = item.Function.NormalState;
            Reactable = item.Function.Reactable;
            Scale = item.Function.Scale;
            Title = title;
            Description = description;
            PublicHouse = publicHouse;
            Type = InteractActorType.AdBalloon;
            CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount;
            ExpirationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount + item.Function.Duration;
        }
    }
}
