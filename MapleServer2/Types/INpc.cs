using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Enums;

namespace MapleServer2.Types;

public interface INpc : IFieldActor<NpcMetadata>
{
    public NpcState State { get; set; }
    public NpcAction Action { get; set; }
    public MobMovement Movement { get; set; }
}
