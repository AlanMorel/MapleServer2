using MapleServer2.Enums;

namespace MapleServer2.Types;

public class Mount
{
    public RideType Type;
    public int Id;
    public long Uid;
    public IFieldObject<Player>[] Players = new IFieldObject<Player>[3];
    public UGC Ugc;
}
