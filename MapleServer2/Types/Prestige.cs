using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class Prestige
{
    public long Id;
    public int Level;
    public long Exp;
    public List<int> RewardsClaimed;
    public List<PrestigeMission> Missions = new();

    public Prestige() { }

    public Prestige(int level, long exp, int id = 0)
    {
        Level = level;
        Exp = exp;
        RewardsClaimed = new();
        Missions = PrestigeLevelMissionMetadataStorage.GetPrestigeMissions;
        if (id == 0)
        {
            Id = DatabaseManager.Prestiges.Insert(this);
            return;
        }
        Id = id;
    }

    public Prestige(long id, int level, long exp, List<int> rewardsClaimed, List<PrestigeMission> missions)
    {
        Id = id;
        Level = level;
        Exp = exp;
        RewardsClaimed = rewardsClaimed;
        Missions = missions;
    }

    public void LevelUp(GameSession session)
    {
        Level++;
        foreach (PrestigeMission mission in Missions)
        {
            mission.LevelCount++;
        }

        session?.Send(PrestigePacket.LevelUp(session.Player.FieldPlayer.ObjectId, Level));
    }

    public void SetLevel(GameSession session, int level)
    {
        Level = level;
        Exp = 0;
        session?.Send(PrestigePacket.ExpUp(0, 0));
        session?.Send(PrestigePacket.LevelUp(session.Player.FieldPlayer.ObjectId, Level));
    }

    public void GainExp(GameSession session, long amount)
    {
        if (session.Player.Levels.Level < 50) // Can only gain prestige exp after level 50.
        {
            return;
        }
        // Prestige exp can only be earned 1M exp per day. 
        // TODO: After 1M exp, reduce the gain and reset the exp gained every midnight.

        long newPrestigeExp = Exp + amount;

        if (newPrestigeExp >= 1000000)
        {
            newPrestigeExp -= 1000000;
            LevelUp(session);
        }

        Exp = newPrestigeExp;
        session.Send(PrestigePacket.ExpUp(Exp, amount));
    }
}
