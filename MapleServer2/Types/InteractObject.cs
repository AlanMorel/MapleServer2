using Maple2Storage.Enums;

namespace MapleServer2.Types
{
    public class InteractObject
    {
        public string Uuid;
        public string Name;
        public InteractObjectType Type;
        public AdBalloon Balloon;

        public InteractObject(string uuid, string name, InteractObjectType type)
        {
            Uuid = uuid;
            Name = name;
            Type = type;
        }
    }

    public class AdBalloon
    {
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
        public long CreationTimestamp;
        public long ExpirationTimestamp; // TODO: Remove from field if expired

        public AdBalloon(Player owner, Item item, string title, string description, bool publicHouse)
        {
            Owner = owner;
            InteractId = item.Function.InstallBillboard.InteractId;
            Model = item.Function.InstallBillboard.Model;
            Asset = item.Function.InstallBillboard.Asset;
            NormalState = item.Function.InstallBillboard.NormalState;
            Reactable = item.Function.InstallBillboard.Reactable;
            Scale = item.Function.InstallBillboard.Scale;
            Title = title;
            Description = description;
            PublicHouse = publicHouse;
            CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount;
            ExpirationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount + item.Function.InstallBillboard.Duration;
        }
    }
}
