using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseBuddy : DatabaseTable
    {
        public DatabaseBuddy() : base("buddies") { }

        public long Insert(Buddy buddy)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                buddy.SharedId,
                buddy.CharacterId,
                friendcharacterid = buddy.Friend.CharacterId,
                buddy.Message,
                buddy.IsFriendRequest,
                buddy.IsPending,
                buddy.Blocked,
                buddy.BlockReason,
                buddy.Timestamp
            });
        }

        public List<Buddy> FindAll()
        {
            List<Buddy> buddyList = new();
            IEnumerable<dynamic> results = QueryFactory.Query(TableName).Get();
            foreach (dynamic result in results)
            {
                buddyList.Add(new Buddy(result.Id, result.SharedId, result.CharacterId,
                    DatabaseManager.Characters.FindPartialPlayerById(result.FriendCharacterId),
                    result.Message, result.IsFriendRequest, result.IsPending, result.Blocked,
                    result.BlockReason, result.Timestamp));
            }
            return buddyList;
        }

        public void Update(Buddy buddy)
        {
            QueryFactory.Query(TableName).Where("id", buddy.Id).Update(new
            {
                buddy.SharedId,
                buddy.CharacterId,
                friendcharacterid = buddy.Friend.CharacterId,
                buddy.Message,
                buddy.IsFriendRequest,
                buddy.IsPending,
                buddy.Blocked,
                buddy.BlockReason,
                buddy.Timestamp
            });
        }

        public bool Delete(long buddyId) => QueryFactory.Query(TableName).Where("id", buddyId).Delete() == 1;
    }
}
