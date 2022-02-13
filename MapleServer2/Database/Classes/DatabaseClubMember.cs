using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseClubMember : DatabaseTable
{
    public DatabaseClubMember() : base("club_members") { }

    public long Insert(ClubMember member)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            club_id = member.ClubId,
            character_id = member.Player.CharacterId,
            join_timestamp = member.JoinTimestamp
        });
    }

    public List<ClubMember> FindAllClubIdsByCharacterId(long characterId)
    {
        IEnumerable<dynamic> result = QueryFactory.Query(TableName).Where("character_id", characterId).Get();
        List<ClubMember> members = new();
        foreach (dynamic entry in result)
        {
            members.Add(ReadClubMember(entry));
        }
        return members;
    }

    public List<ClubMember> FindAllClubMembersByClubId(long clubId)
    {
        IEnumerable<dynamic> result = QueryFactory.Query(TableName).Where("club_id", clubId).Get();
        List<ClubMember> members = new();
        foreach (dynamic entry in result)
        {
            members.Add(ReadClubMember(entry));
        }
        return members;
    }

    public bool Delete(long clubId, long characterId)
    {
        return QueryFactory.Query(TableName).Where(new
        {
            club_id = clubId,
            character_id = characterId
        }).Delete() == 1;
    }

    private static ClubMember ReadClubMember(dynamic data)
    {
        return new ClubMember(DatabaseManager.Characters.FindPartialPlayerById(data.character_id), data.club_id, data.join_timestamp);
    }
}
