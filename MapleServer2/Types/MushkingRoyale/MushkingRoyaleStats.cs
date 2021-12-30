using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class MushkingRoyaleStats
{
    public long Id;
    public long Exp;
    public int Level;
    public int SilverLevelClaimedRewards;
    public int GoldLevelClaimedRewards;
    public bool IsGoldPassActive;

    public MushkingRoyaleStats()
    {
        Level = 1;
        Id = DatabaseManager.MushkingRoyaleStats.Insert(this);
    }

    public MushkingRoyaleStats(long id, int level, long exp, int silverClaimedRewards, int goldClaimedRewards, bool isGoldPassActive)
    {
        Id = id;
        Level = level;
        Exp = exp;
        SilverLevelClaimedRewards = silverClaimedRewards;
        GoldLevelClaimedRewards = goldClaimedRewards;
        IsGoldPassActive = isGoldPassActive;
    }

    public void AddExp(GameSession session, long amount)
    {
        if (amount <= 0 || !SurvivaLevelMetadataStorage.LevelExist(Level + 1))
        {
            return;
        }

        long newExp = Exp + amount;

        while (newExp >= SurvivaLevelMetadataStorage.GetExpToNextLevel(Level))
        {
            newExp -= SurvivaLevelMetadataStorage.GetExpToNextLevel(Level);
            if (!LevelUp()) // If can't level up because next level doesn't exist, exit the loop
            {
                newExp = 0;
                break;
            }
        }

        Exp = newExp;
        session.Send(MushkingRoyaleSystemPacket.LoadStats(this, newExp));
    }

    private bool LevelUp()
    {
        if (!SurvivaLevelMetadataStorage.LevelExist(Level + 1))
        {
            return false;
        }

        Level++;
        return true;
    }
}
