using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseClub : DatabaseTable
{
    public DatabaseClub() : base("clubs") { }

    public long Insert(Club club)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            name = club.Name,
            creation_timestamp = club.CreationTimestamp,
            leader_account_id = club.LeaderAccountId,
            leader_character_id = club.LeaderCharacterId,
            leader_name = club.LeaderName,
            is_established = club.IsEstablished,
            last_name_change_timestamp = club.LastNameChangeTimestamp,
        });
    }

    public Guild FindById(long id)
    {
        return ReadClub(QueryFactory.Query(TableName).Where("id", id).FirstOrDefault());
    }

    public bool NameExists(string name)
    {
        return QueryFactory.Query(TableName).Where("name", name).AsCount().FirstOrDefault().count == 1;
    }

    public List<Club> FindAll()
    {
        IEnumerable<dynamic> result = QueryFactory.Query(TableName).Get();
        List<Club> clubs = new();
        foreach (dynamic data in result)
        {
            clubs.Add(ReadClub(data));
        }
        return clubs;
    }

    public void Update(Club club)
    {
        QueryFactory.Query(TableName).Where("id", club.Id).Update(new
        {
            name = club.Name,
            is_established = club.IsEstablished,
            last_name_change_timestamp = club.LastNameChangeTimestamp,
            leader_account_id = club.LeaderAccountId,
            leader_character_id = club.LeaderCharacterId,
            leader_name = club.LeaderName
        });
    }

    public bool Delete(long id)
    {
        return QueryFactory.Query(TableName).Where("id", id).Delete() == 1;
    }

    private static Club ReadClub(dynamic data)
    {
        List<ClubMember> clubMembers = DatabaseManager.ClubMembers.FindAllClubMembersByClubId(data.id);

        return new()
        {
            Id = data.id,
            Name = data.name,
            CreationTimestamp = data.creation_timestamp,
            LastNameChangeTimestamp = data.last_name_change_timestamp,
            LeaderAccountId = data.leader_account_id,
            LeaderCharacterId = data.leader_character_id,
            LeaderName = data.leader_name,
            IsEstablished = data.is_established,
            Members = clubMembers
        };
    }
}
