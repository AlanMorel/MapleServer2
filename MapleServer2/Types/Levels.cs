using Maple2Storage.Enums;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Managers;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class Levels
{
    public readonly long Id;
    private readonly GameSession Session;
    private Player Player => Session.Player;
    private IFieldActor<Player> FieldPlayer => Player.FieldPlayer;

    public short Level { get; private set; }
    public long Exp { get; private set; }
    public long RestExp { get; private set; }
    public int PrestigeLevel { get; private set; }
    public long PrestigeExp { get; private set; }
    public List<MasteryExp> MasteryExp { get; }

    public Levels(short playerLevel, long exp, long restExp, int prestigeLevel, long prestigeExp,
        List<MasteryExp> masteryExp, GameSession gameSession, long id = 0)
    {
        Level = playerLevel;
        Exp = exp;
        RestExp = restExp;
        PrestigeLevel = prestigeLevel;
        PrestigeExp = prestigeExp;
        MasteryExp = masteryExp;
        Session = gameSession;

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
        Session.Send(ExperiencePacket.ExpUp(0, Exp, 0));
        Session.FieldManager.BroadcastPacket(LevelUpPacket.LevelUp(FieldPlayer.ObjectId, Level));
        Session.FieldManager.BroadcastPacket(FieldObjectPacket.UpdateCharacterLevel(Player));

        Player.UpdateSocials();
        QuestHelper.GetNewQuests(Player);
    }

    public bool LevelUp()
    {
        if (!ExpMetadataStorage.LevelExist((short) (Level + 1)))
        {
            return false;
        }

        Level++;

        Player.Stats.AddBaseStats(Player);
        Player.FieldPlayer.RecoverHp(FieldPlayer.Stats[StatAttribute.Hp].Bonus);

        Session.FieldManager.BroadcastPacket(RevivalConfirmPacket.Send(FieldPlayer.ObjectId, 0));
        Session.FieldManager.BroadcastPacket(LevelUpPacket.LevelUp(FieldPlayer.ObjectId, Level));
        Session.FieldManager.BroadcastPacket(FieldObjectPacket.UpdateCharacterLevel(Player));

        // Find all new skills for current level
        HashSet<int> newSkillIds = SkillMetadataStorage.GetJobSkills(Player.Job)
            .Where(x => x.SkillLevels.First().SkillUpgrade.LevelRequired == Level)
            .Select(x => x.SkillId).ToHashSet();
        Session.FieldManager.BroadcastPacket(JobPacket.UpdateSkillTab(FieldPlayer, newSkillIds));

        Session.Send(StatPacket.SetStats(FieldPlayer));
        Session.FieldManager.BroadcastPacket(StatPacket.UpdateFieldStats(FieldPlayer), Session);

        Session.Send(KeyTablePacket.SendFullOptions(Player.GameOptions));

        Player.UpdateSocials();
        TrophyManager.OnLevelUp(Player);
        QuestHelper.GetNewQuests(Player);

        DatabaseManager.Characters.Update(Player);
        return true;
    }

    public void SetPrestigeLevel(int level)
    {
        PrestigeLevel = level;
        PrestigeExp = 0;
        Session.Send(PrestigePacket.ExpUp(0, 0));
        Session.Send(PrestigePacket.LevelUp(FieldPlayer.ObjectId, PrestigeLevel));
    }

    public void PrestigeLevelUp()
    {
        PrestigeLevel++;
        foreach (PrestigeMission mission in Session.Player.PrestigeMissions)
        {
            mission.LevelCount++;
        }
        Session.Send(PrestigePacket.LevelUp(FieldPlayer.ObjectId, PrestigeLevel));
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
            if (LevelUp())
            {
                continue;
            }

            newExp = 0;
            break;
        }

        Exp = newExp;
        Session.Send(ExperiencePacket.ExpUp(amount, newExp, RestExp));
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
        Session.Send(PrestigePacket.ExpUp(PrestigeExp, amount));
    }

    public void GainMasteryExp(MasteryType type, long amount)
    {
        MasteryExp masteryExp = MasteryExp.FirstOrDefault(x => x.Type == type);

        if (masteryExp == null || amount <= 0)
        {
            return;
        }

        // user already has some exp in mastery, so simply update it
        Session.Send(MasteryPacket.SetExp(type, masteryExp.CurrentExp += amount));
        int currLevel = MasteryMetadataStorage.GetGradeFromXP(type, masteryExp.CurrentExp);

        if (currLevel <= masteryExp.Level)
        {
            return;
        }

        masteryExp.Level = currLevel;
        TrophyManager.OnGainMasteryLevel(Player);
    }
}
