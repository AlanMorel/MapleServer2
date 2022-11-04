using MapleServer2.Types;

namespace MapleServer2.Managers;

public class PartyManager
{
    private readonly Dictionary<int, Party> PartyList;

    public PartyManager()
    {
        PartyList = new();
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

    public Party GetPartyById(int id)
    {
        return PartyList.TryGetValue(id, out Party foundParty) ? foundParty : null;
    }

    public Party GetPartyByMember(long characterId)
    {
        return PartyList.Values.FirstOrDefault(x => x.Members.Any(z => z.CharacterId == characterId));
    }

    public Party GetPartyByLeader(Player leader)
    {
        return (from entry in PartyList
                where entry.Value.Leader.CharacterId == leader.CharacterId
                select entry.Value).FirstOrDefault();
    }
}
