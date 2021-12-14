using Maple2Storage.Types;

namespace MapleServer2.Types;

public class LiftableObject
{
    public string EntityId;
    public int ItemId;
    public string MaskQuestId;
    public string MaskQuestState;
    public CoordF Position;
    public CoordF Rotation;
    public LiftableState State;
    public bool Enabled;

    public LiftableObject(string entityId, int itemId, string maskQuestId, string maskQuestState)
    {
        EntityId = entityId;
        ItemId = itemId;
        MaskQuestId = maskQuestId;
        MaskQuestState = maskQuestState;
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
