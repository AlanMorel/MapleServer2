using System;
using System.Collections.Generic;
using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class Club
    {
        public long Id { get; }
        public string Name { get; set; }
        public bool Approval { get; set; } //Require approval before someone can join
        public Player Leader { get; set; }
        public int MaxMembers { get; set; }
        public List<Player> Members { get; }

        public Club(Party party, string name)
        {
            Id = GuidGenerator.Int();
            Name = name;
            MaxMembers = 10;
            Leader = party.Leader;
            Members = party.Members; // TODO change from out of party
            Approval = false;
            Leader.ClubId = Id;
        }
    }
}