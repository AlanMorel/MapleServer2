using Maple2Storage.Enums;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Managers;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;

namespace MapleServer2.Types;

public class Levels
{
    public readonly long Id;
    public Player Player;
    public short Level { get; private set; }
    public long Exp { get; private set; }
    public long RestExp { get; private set; }
    public int PrestigeLevel { get; private set; }
    public long PrestigeExp { get; private set; }
    public List<MasteryExp> MasteryExp { get; private set; }

    public Levels() { }

    public Levels(short playerLevel, long exp, long restExp, int prestigeLevel, long prestigeExp,
        List<MasteryExp> masteryExp, long id = 0)
    {
        Level = playerLevel;
        Exp = exp;
        RestExp = restExp;
        PrestigeLevel = prestigeLevel;
        PrestigeExp = prestigeExp;
        MasteryExp = masteryExp;

        if (id == 0)
        {
            Id = DatabaseManager.Levels.Insert(this);
            return;
        }

        Id = id;
    }

    public void SetLevel(short level)
    {
        Level = level;
        Exp = 0;
        Player.Session.Send(ExperiencePacket.ExpUp(0, Exp, 0));
        Player.Session.Send(ExperiencePacket.LevelUp(Player.FieldPlayer, Level));

        QuestHelper.GetNewQuests(Player);
    }

    public bool LevelUp()
    {
        if (!ExpMetadataStorage.LevelExist((short) (Level + 1)))
        {
            return false;
        }

        Level++;

        TrophyManager.OnLevelUp(Player);

        Player.StatPointDistribution.AddTotalStatPoints(5);
        Player.Session.FieldManager.BroadcastPacket(ExperiencePacket.LevelUp(Player.FieldPlayer, Level));
        // TODO: Gain max HP
        Player.FieldPlayer.RecoverHp(Player.FieldPlayer.Stats[StatId.Hp].Bonus);
        Player.Session.Send(StatPointPacket.WriteTotalStatPoints(Player));

        QuestHelper.GetNewQuests(Player);
        return true;
    }

    public void SetPrestigeLevel(int level)
    {
        PrestigeLevel = level;
        PrestigeExp = 0;
        Player.Session.Send(PrestigePacket.ExpUp(Player, 0));
        Player.Session.Send(PrestigePacket.LevelUp(Player.FieldPlayer, PrestigeLevel));
    }

    public void PrestigeLevelUp()
    {
        PrestigeLevel++;
        Player.Session.Send(PrestigePacket.LevelUp(Player.FieldPlayer, PrestigeLevel));
    }

    public void GainExp(int amount)
    {
        if (amount <= 0 || !ExpMetadataStorage.LevelExist((short) (Level + 1)))
        {
            return;
        }

        long newExp = Exp + amount + RestExp;

        if (RestExp > 0)
        {
            RestExp -= amount;
        }
        else
        {
            RestExp = 0;
        }

        while (newExp >= ExpMetadataStorage.GetExpToLevel(Level))
        {
            newExp -= ExpMetadataStorage.GetExpToLevel(Level);
            if (!LevelUp()) // If can't level up because next level doesn't exist, exit the loop
            {
                newExp = 0;
                break;
            }
        }

        Exp = newExp;
        Player.Session.Send(ExperiencePacket.ExpUp(amount, newExp, RestExp));
    }

    public void GainPrestigeExp(long amount)
    {
        if (Level < 50) // Can only gain prestige exp after level 50.
        {
            return;
        }
        // Prestige exp can only be earned 1M exp per day. 
        // TODO: After 1M exp, reduce the gain and reset the exp gained every midnight.

        long newPrestigeExp = PrestigeExp + amount;

        if (newPrestigeExp >= 1000000)
        {
            newPrestigeExp -= 1000000;
            PrestigeLevelUp();
        }

        PrestigeExp = newPrestigeExp;
        Player.Session.Send(PrestigePacket.ExpUp(Player, amount));
    }

    public void GainMasteryExp(MasteryType type, long amount)
    {
        MasteryExp masteryExp = MasteryExp.FirstOrDefault(x => x.Type == type);

        if (masteryExp == null || amount <= 0)
        {
            return;
        }

        // user already has some exp in mastery, so simply update it
        Player.Session.Send(MasteryPacket.SetExp(type, masteryExp.CurrentExp += amount));
        int currLevel = MasteryMetadataStorage.GetGradeFromXP(type, masteryExp.CurrentExp);

        if (currLevel <= masteryExp.Level)
        {
            return;
        }

        masteryExp.Level = currLevel;
        Player.TrophyUpdate("mastery_grade", 1);
        Player.TrophyUpdate("set_mastery_grade", 1);
    }
}
