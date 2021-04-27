using System;
using System.Collections.Generic;
using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class GuildBuff
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int StartTimestamp { get; set; }

        public GuildBuff(int id)
        {
            Id = id;
            Level = 1;
        }
    }
}
