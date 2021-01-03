using System.Collections.Generic;
using System.Linq;
using MapleServer2.Types;

namespace MapleServer2.Tools
{
    public class ClubManager
    {
        private readonly Dictionary<long, Club> clubList;

        public ClubManager()
        {
            clubList = new Dictionary<long, Club>();
        }

        public void AddClub(Club club)
        {
            clubList.Add(club.Id, club);
        }

        public void RemoveClub(Club club)
        {
            clubList.Remove(club.Id);
        }

        public Club GetClubById(long id)
        {
            return clubList.TryGetValue(id, out Club foundClub) ? foundClub : null;
        }

        public Club GetClubByLeader(Player leader)
        {
            return (from entry in clubList
                    where entry.Value.Leader.CharacterId == leader.CharacterId
                    select entry.Value).FirstOrDefault();
        }
    }
}
