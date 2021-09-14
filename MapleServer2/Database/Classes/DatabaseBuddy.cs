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
                shared_id = buddy.SharedId,
                character_id = buddy.CharacterId,
                friend_character_id = buddy.Friend.CharacterId,
                buddy.Message,
                is_friend_request = buddy.IsFriendRequest,
                is_pending = buddy.IsPending,
                buddy.Blocked,
                block_reason = buddy.BlockReason,
                buddy.Timestamp
            });
        }

        public List<Buddy> FindAll()
        {
            List<Buddy> buddyList = new();
            IEnumerable<dynamic> results = QueryFactory.Query(TableName).Get();
            foreach (dynamic result in results)
            {
                buddyList.Add(new Buddy(result.id, result.shared_id, result.character_id,
                    DatabaseManager.Characters.FindPartialPlayerById(result.friend_character_id),
                    result.message, result.is_friend_request, result.is_pending, result.blocked,
                    result.block_reason, result.timestamp));
            }
            return buddyList;
        }

        public void Update(Buddy buddy)
        {
            QueryFactory.Query(TableName).Where("id", buddy.Id).Update(new
            {
                shared_id = buddy.SharedId,
                character_id = buddy.CharacterId,
                friend_character_id = buddy.Friend.CharacterId,
                buddy.Message,
                is_friend_request = buddy.IsFriendRequest,
                is_pending = buddy.IsPending,
                buddy.Blocked,
                block_reason = buddy.BlockReason,
                buddy.Timestamp
            });
        }

        public bool Delete(long buddyId) => QueryFactory.Query(TableName).Where("id", buddyId).Delete() == 1;
    }
}
