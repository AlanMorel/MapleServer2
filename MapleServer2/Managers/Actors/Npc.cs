using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Managers.Actors;

public class Npc : FieldActor<NpcMetadata>, INpc
{
    public NpcState State { get; set; }
    public NpcAction Action { get; set; }
    public MobMovement Movement { get; set; }

    public Npc(int objectId, int npcId, FieldManager fieldManager) : this(objectId, NpcMetadataStorage.GetNpcMetadata(npcId), fieldManager) { }

    public Npc(int objectId, NpcMetadata metadata, FieldManager fieldManager) : base(objectId, metadata, fieldManager)
    {
        Animation = 255;
        Stats = new(metadata);
        State = NpcState.Normal;
        Action = NpcAction.Idle;
    }

    public override void Animate(string sequenceName)
    {
        Animation = AnimationStorage.GetSequenceIdBySequenceName(Value.Model, sequenceName);
        // TODO implement stopping animation
    }
}
