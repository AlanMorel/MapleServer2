using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace MapleServer2.Types;

public class LiftableObject
{
    public readonly string EntityId;
    public readonly MapLiftableObject Metadata;
    public LiftableState State;
    public bool Enabled;
    public bool PickedUp;

    public LiftableObject(string entityId, MapLiftableObject metadata)
    {
        EntityId = entityId;
        Metadata = metadata;
        State = LiftableState.Active;
        Enabled = true;
    }
}

public enum LiftableState
{
    Removed = 0,
    Active = 1,
    Disabled = 2
}
