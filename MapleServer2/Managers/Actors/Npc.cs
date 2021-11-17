using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Managers;

public partial class FieldManager
{
    private partial class Npc : FieldActor<NpcMetadata>, INpc
    {
        public NpcState State { get; set; }
        public NpcAction Action { get; set; }
        public MobMovement Movement { get; set; }

        public Npc(int objectId, int npcId) : this(objectId, NpcMetadataStorage.GetNpcMetadata(npcId)) { }

        public Npc(int objectId, NpcMetadata metadata) : base(objectId, metadata)
        {
            Animation = 255;
            Stats = new(metadata);
            State = NpcState.Normal;
            Action = NpcAction.Idle;
        }
    }
}
