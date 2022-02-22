using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace MapleServer2.Types;

public class LiftableObject
{
    public string EntityId;
    public MapLiftableObject Metadata;
    public CoordF Position;
    public CoordF Rotation;
    public LiftableState State;
    public bool Enabled;

    public LiftableObject(string entityId, MapLiftableObject metadata)
    {
        EntityId = entityId;
        Metadata = metadata;
        State = LiftableState.Active;
        Enabled = true;
    }
}

public enum LiftableState : byte
{
    Active = 0,
    Removed = 1,
    Disabled = 2
}
