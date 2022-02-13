using MapleServer2.Database;

namespace MapleServer2.Types;

public class ClubMember
{
    public Player Player;
    public long JoinTimestamp;
    public long ClubId;
    public ClubInviteResponse InviteResponse;

    public ClubMember() { }

    public ClubMember(Player player, long clubId)
    {
        Player = player;
        JoinTimestamp = TimeInfo.Now();
        ClubId = clubId;
        InviteResponse = ClubInviteResponse.Pending;
        DatabaseManager.ClubMembers.Insert(this);
    }

    public ClubMember(Player player, long clubId, long joinTimestamp)
    {
        Player = player;
        JoinTimestamp = joinTimestamp;
        ClubId = clubId;
    }


}
