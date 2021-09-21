using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace MapleServer2.Types
{
    public class InteractObject
    {
        public string Id;
        public int InteractId;
        public InteractObjectState State;
        public InteractObjectType Type;

        public InteractObject(string id, int interactId, InteractObjectType type, InteractObjectState state)
        {
            Id = id;
            InteractId = interactId;
            State = state;
            // enabling all interact objects. Seems like the default status in the xblock/flat files has it disabled.
            // TODO: find out where these are actually turned on?
            Type = type;

        }
    }

    public enum InteractObjectState : byte
    {
        Disable = 0,
        Default = 1,
        Activated = 2
    }

    public class AdBalloon : InteractObject
    {
        public string Model;
        public string Asset;
        public string NormalState;
        public string Reactable;
        public float Scale;
        public Player Owner;
        public CoordF Position;
        public CoordF Rotation;
        public string Title;
        public string Description;
        public bool PublicHouse;
        public long CreationTimestamp;
        public long ExpirationTimestamp; // TODO: Remove from field if expired

        public AdBalloon(string id, int interactId, InteractObjectState state, InteractObjectType type, IFieldObject<Player> owner, InstallBillboard metadata, string title, string description, bool publicHouse) : base(id, interactId, type, state)
        {
            Owner = owner.Value;
            Position = owner.Coord;
            Rotation = owner.Rotation;
            Model = metadata.Model;
            Asset = metadata.Asset;
            NormalState = metadata.NormalState;
            Reactable = metadata.Reactable;
            Scale = metadata.Scale;
            Title = title;
            Description = description;
            PublicHouse = publicHouse;
            CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount;
            ExpirationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount + metadata.Duration;
        }
    }
}
