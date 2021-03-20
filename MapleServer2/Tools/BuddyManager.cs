using System.Collections.Generic;
using System.Linq;
using MapleServer2.Types;

namespace MapleServer2.Tools
{
    public class BuddyManager
    {
        private readonly Dictionary<long, Buddy> BuddyList;

        public BuddyManager()
        {
            BuddyList = new Dictionary<long, Buddy>();
        }

        public void AddBuddy(Buddy buddyList)
        {
            BuddyList.Add(buddyList.Id, buddyList);
        }

        public void RemoveBuddy(Buddy buddyList)
        {
            BuddyList.Remove(buddyList.Id);
        }

        public Buddy GetBuddyByPlayerAndId(Player player, long id)
        {
            List<Buddy> allBuddyList = BuddyList.Values.ToList();
            return allBuddyList.Where(o => o.Friend.CharacterId != player.CharacterId && o.SharedId == id).FirstOrDefault();
        }

        public bool IsFriend(Player player1, Player player2)
        {
            return player1.BuddyList.Any(o => o.Friend.CharacterId == player2.CharacterId && o.Blocked == false);
        }

        public bool IsBlocked(Player player, Player otherPlayer)
        {
            return otherPlayer.BuddyList.Any(o => o.Friend.CharacterId == player.CharacterId && o.Blocked == true);
        }
    }
}
