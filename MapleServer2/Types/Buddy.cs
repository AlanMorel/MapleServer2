using System;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class Buddy
    {
        public long Id { get; }
        public long SharedId { get; }
        public Player Player { get; set; }
        public Player Friend { get; set; }
        public string Message { get; set; }
        public bool IsFriendRequest { get; set; }
        public bool IsPending { get; set; }
        public bool Blocked { get; set; }
        public string BlockReason { get; set; }
        public long Timestamp { get; }

        public Buddy() { }

        public Buddy(long id, Player friend, Player player, string message, bool pending, bool accepted, bool blocked = false)
        {
            Id = GuidGenerator.Long();
            SharedId = id;
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

            player.BuddyList.Add(this);
        }
    }
}
