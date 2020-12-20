using System;
using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Data
{
    public class PartyManager
    {
        private readonly Dictionary<long, Party> partyList;

        public PartyManager()
        {
            this.partyList = new Dictionary<long, Party>();
        }

        public void AddParty(Party party)
        {
            partyList.Add(party.Uid, party);
        }

        public void RemoveParty(Party party)
        {
            partyList.Remove(party.Uid);
        }

        public Party GetPartyById(long id)
        {
            System.Console.WriteLine("Party list: ");
            foreach (KeyValuePair<long, Party> entry in partyList)
            {
                Party p = entry.Value;
                System.Console.WriteLine(p.Leader.Name + ", " + p.Uid + ", Members:" + p.Players.Count);
            }
            Party foundParty;
            if (partyList.TryGetValue(id, out foundParty))
            {
                return foundParty;
            }
            return null;
        }

        public Party GetPartyByLeader(Player leader)
        {
            foreach (KeyValuePair<long, Party> entry in partyList)
            {
                if (entry.Value.Leader.CharacterId == leader.CharacterId)
                {
                    return entry.Value;
                }
            }
            return null;
        }
    }
}
