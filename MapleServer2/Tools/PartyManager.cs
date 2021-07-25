using System.Collections.Generic;
using System.Linq;
using MapleServer2.Types;

namespace MapleServer2.Tools
{
    public class PartyManager
    {
        private readonly Dictionary<long, Party> PartyList;

        public PartyManager()
        {
            PartyList = new Dictionary<long, Party>();
        }

        public void AddParty(Party party)
        {
            PartyList.Add(party.Id, party);
        }

        public void RemoveParty(Party party)
        {
            PartyList.Remove(party.Id);
        }

        public List<Party> GetPartyFinderList()
        {
            return PartyList.Values.Where(party => party.PartyFinderId != 0).ToList();
        }

        public Party GetPartyById(long id)
        {
            return PartyList.TryGetValue(id, out Party foundParty) ? foundParty : null;
        }

        public Party GetPartyByMember(long id)
        {
            return PartyList.Values.FirstOrDefault(x => x.Members.Any(z => z.CharacterId == id));
        }

        public Party GetPartyByLeader(Player leader)
        {
            return (from entry in PartyList
                    where entry.Value.Leader.CharacterId == leader.CharacterId
                    select entry.Value).FirstOrDefault();
        }
    }
}
