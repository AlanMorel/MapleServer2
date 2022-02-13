using MapleServer2.Database;
using MapleServer2.Types;

namespace MapleServer2.Managers;

public class ClubManager
{
    private readonly Dictionary<long, Club> ClubList;

    public ClubManager()
    {
        ClubList = new();
        List<Club> list = DatabaseManager.Clubs.FindAll();
        foreach (Club club in list)
        {
            if (!club.IsEstablished)
            {
                List<ClubMember> clubMembers = DatabaseManager.ClubMembers.FindAllClubMembersByClubId(club.Id);
                foreach (ClubMember member in clubMembers)
                {
                    DatabaseManager.ClubMembers.Delete(member.ClubId, member.Player.CharacterId);
                }
                DatabaseManager.Clubs.Delete(club.Id);
                continue;
            }
            AddClub(club);
        }
    }

    public void AddClub(Club club)
    {
        ClubList.Add(club.Id, club);
    }

    public void RemoveClub(Club club)
    {
        ClubList.Remove(club.Id);
        DatabaseManager.Clubs.Delete(club.Id);
    }

    public Club GetClubById(long id)
    {
        return ClubList.TryGetValue(id, out Club foundClub) ? foundClub : null;
    }
}
