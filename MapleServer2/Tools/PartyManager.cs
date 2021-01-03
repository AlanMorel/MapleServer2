using System.Collections.Generic;
using System.Linq;
using MapleServer2.Types;

namespace MapleServer2.Tools
{
    public class PartyManager
    {
        private readonly Dictionary<long, Party> partyList;

        public PartyManager()
        {
            partyList = new Dictionary<long, Party>();
        }

        public void AddParty(Party party)
        {
            partyList.Add(party.Id, party);
        }

        public void RemoveParty(Party party)
        {
            partyList.Remove(party.Id);
        }

        public List<Party> GetPartyFinderList()
        {
            return partyList.Cast<Party>().Where(party => party.PartyFinderId != 0).ToList();
        }

        public Party GetPartyById(long id)
        {
            return partyList.TryGetValue(id, out Party foundParty) ? foundParty : null;
        }

        public Party GetPartyByLeader(Player leader)
        {
            return (from entry in partyList
                    where entry.Value.Leader.CharacterId == leader.CharacterId
                    select entry.Value).FirstOrDefault();
        }
    }
}
