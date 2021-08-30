using MapleServer2.Database;

namespace MapleServer2.Types
{
    public class Buddy
    {
        public long Id { get; }
        public long SharedId { get; }
        public long CharacterId { get; set; }
        public Player Friend { get; set; }
        public string Message { get; set; }
        public bool IsFriendRequest { get; set; }
        public bool IsPending { get; set; }
        public bool Blocked { get; set; }
        public string BlockReason { get; set; }
        public long Timestamp { get; }

        public Buddy(long id, long sharedId, long characterId, Player friend, string message, bool isFriendRequest, bool isPending,
            bool blocked, string blockReason, long timestamp)
        {
            Id = id;
            SharedId = sharedId;
            CharacterId = characterId;
            Friend = friend;
            Message = message;
            IsFriendRequest = isFriendRequest;
            IsPending = isPending;
            Blocked = blocked;
            BlockReason = blockReason;
            Timestamp = timestamp;
        }

        public Buddy(long id, long characterId, Player friend, string message, bool pending, bool accepted, bool blocked = false)
        {
            SharedId = id;
            CharacterId = characterId;
            Friend = friend;
            IsPending = pending;
            IsFriendRequest = accepted;
            Blocked = blocked;
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount;

            if (blocked == true)
            {
                BlockReason = message;
                Message = "";
            }
            else
            {
                Message = message;
                BlockReason = "";
            }

            Id = DatabaseManager.Buddies.Insert(this);
        }
    }
}
