using System;
using System.Collections.Generic;
using MapleServer2.Database;

namespace MapleServer2.Types
{
    public class MapleopolyEvent // Mapleopoly
    {
        public int Id { get; set; }
        public int TripAmount { get; set; }
        public int ItemId { get; set; }
        public byte ItemRarity { get; set; }
        public int ItemAmount { get; set; }

        public MapleopolyEvent() { }
    }
}
