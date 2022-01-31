using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Enums;

namespace MapleServer2.Types;

public class InteractObject
{
    public readonly string Id;
    public readonly int InteractId;
    public InteractObjectState State;
    public readonly InteractObjectType Type;
    public string Model;
    public string Asset;
    public string NormalState;
    public string Reactable;
    public float Scale;
    public CoordF Position;
    public CoordF Rotation;

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

public class AdBalloon : InteractObject
{
    public readonly Player Owner;
    public readonly string Title;
    public readonly string Description;
    public readonly bool PublicHouse;
    public readonly long CreationTimestamp;
    public readonly long ExpirationTimestamp; // TODO: Remove from field if expired

    public AdBalloon(string id, int interactId, InteractObjectState state, InteractObjectType type, IFieldObject<Player> owner, InstallBillboard metadata,
        string title, string description, bool publicHouse) : base(id, interactId, type, state)
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
        CreationTimestamp = TimeInfo.Now() + Environment.TickCount;
        ExpirationTimestamp = TimeInfo.Now() + Environment.TickCount + metadata.Duration;
    }
}

public class MapChest : InteractObject
{
    public MapChest(string id, int interactId, InteractObjectType type, InteractObjectState state) : base(id, interactId, type, state) { }
}
