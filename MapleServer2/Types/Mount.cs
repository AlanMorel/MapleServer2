using System.Collections.Generic;
using MapleServer2.Enums;

namespace MapleServer2.Types
{
    public class Mount
    {
        public RideType Type;
        public int Id;
        public long Uid;
        public List<IFieldObject<Player>> Players = new List<IFieldObject<Player>>();
    }
}
