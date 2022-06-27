using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace MapleServer2.Types;

public class LiftableObject
{
    public readonly string EntityId;
    public readonly MapLiftableObject Metadata;
    public LiftableState State;
    public int ItemCount;

    public LiftableObject(string entityId, MapLiftableObject metadata)
    {
        EntityId = entityId;
        Metadata = metadata;
        State = LiftableState.Active;
        ItemCount = metadata.ItemStackCount;
    }
}

public enum LiftableState : byte
{
    Active = 0,
    Removed = 1,
    Disabled = 2
}
