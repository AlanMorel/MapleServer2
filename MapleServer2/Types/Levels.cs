using System.Collections.Generic;
using System.Linq;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;

namespace MapleServer2.Types
{
    public class Levels
    {
        public readonly long Id;
        public readonly Player Player;
        public short Level { get; private set; }
        public long Exp { get; private set; }
        public long RestExp { get; private set; }
        public int PrestigeLevel { get; private set; }
        public long PrestigeExp { get; private set; }
        public List<MasteryExp> MasteryExp { get; private set; }

        public Levels() { }

        public Levels(Player player, short playerLevel, long exp, long restExp, int prestigeLevel, long prestigeExp,
            List<MasteryExp> masteryExp, long id = 0)
        {
            Player = player;
            Level = playerLevel;
            Exp = exp;
            RestExp = restExp;
            PrestigeLevel = prestigeLevel;
            PrestigeExp = prestigeExp;
            MasteryExp = masteryExp;
            Id = id;
        }

        public void SetLevel(short level)
        {
            Level = level;
            Exp = 0;
            Player.Session.Send(ExperiencePacket.ExpUp(0, Exp, 0));
            Player.Session.Send(ExperiencePacket.LevelUp(Player.Session.FieldPlayer, Level));

            QuestHelper.GetNewQuests(Player.Session, Level);
        }

        public bool LevelUp()
        {
            if (!ExpMetadataStorage.LevelExist((short) (Level + 1)))
            {
                return false;
            }

            Level++;
            Player.StatPointDistribution.AddTotalStatPoints(5);
            Player.Session.Send(ExperiencePacket.LevelUp(Player.Session.FieldPlayer, Level));
            // TODO: Gain max HP
            Player.RecoverHp(Player.Stats[PlayerStatId.Hp].Max);
            Player.Session.Send(StatPointPacket.WriteTotalStatPoints(Player));

            QuestHelper.GetNewQuests(Player.Session, Level);
            return true;
        }

        public void SetPrestigeLevel(int level)
        {
            PrestigeLevel = level;
            PrestigeExp = 0;
            Player.Session.Send(PrestigePacket.ExpUp(Player, 0));
            Player.Session.Send(PrestigePacket.LevelUp(Player.Session.FieldPlayer, PrestigeLevel));
        }

        public void PrestigeLevelUp()
        {
            PrestigeLevel++;
            Player.Session.Send(PrestigePacket.LevelUp(Player.Session.FieldPlayer, PrestigeLevel));
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

            if (masteryExp == null) // add mastery to list
            {
                masteryExp = new MasteryExp(type);
                MasteryExp.Add(masteryExp);
            }
            if (amount <= 0)
            {
                return;
            }
            // user already has some exp in mastery, so simply update it
            Player.Session.Send(MasteryPacket.SetExp(type, masteryExp.CurrentExp += amount));
            int currLevel = MasteryMetadataStorage.GetGradeFromXP(type, masteryExp.CurrentExp);

            if (currLevel > masteryExp.Level)
            {
                masteryExp.Level = currLevel;
                switch (type)
                {
                    case MasteryType.Mining:
                        Player.TrophyUpdate(23100238, 1);
                        Player.TrophyUpdate(23100239, 1);
                        Player.TrophyUpdate(23100240, 1);
                        Player.TrophyUpdate(23100241, 1);
                        Player.TrophyUpdate(23100330, 1);
                        break;
                    case MasteryType.Foraging:
                        Player.TrophyUpdate(23100257, 1);
                        Player.TrophyUpdate(23100258, 1);
                        Player.TrophyUpdate(23100259, 1);
                        Player.TrophyUpdate(23100260, 1);
                        Player.TrophyUpdate(23100334, 1);
                        break;
                    case MasteryType.Ranching:
                        Player.TrophyUpdate(23100242, 1);
                        Player.TrophyUpdate(23100243, 1);
                        Player.TrophyUpdate(23100244, 1);
                        Player.TrophyUpdate(23100245, 1);
                        Player.TrophyUpdate(23100331, 1);
                        break;
                    case MasteryType.Farming:
                        Player.TrophyUpdate(23100261, 1);
                        Player.TrophyUpdate(23100262, 1);
                        Player.TrophyUpdate(23100263, 1);
                        Player.TrophyUpdate(23100264, 1);
                        Player.TrophyUpdate(23100335, 1);
                        break;
                    case MasteryType.Smithing:
                        Player.TrophyUpdate(23100246, 1);
                        Player.TrophyUpdate(23100247, 1);
                        Player.TrophyUpdate(23100248, 1);
                        Player.TrophyUpdate(23100249, 1);
                        Player.TrophyUpdate(23100332, 1);
                        break;
                    case MasteryType.Handicraft:
                        Player.TrophyUpdate(23100250, 1);
                        Player.TrophyUpdate(23100251, 1);
                        Player.TrophyUpdate(23100252, 1);
                        Player.TrophyUpdate(23100253, 1);
                        Player.TrophyUpdate(23100333, 1);
                        break;
                    case MasteryType.Alchemy:
                        Player.TrophyUpdate(23100265, 1);
                        Player.TrophyUpdate(23100266, 1);
                        Player.TrophyUpdate(23100267, 1);
                        Player.TrophyUpdate(23100268, 1);
                        Player.TrophyUpdate(23100336, 1);
                        break;
                    case MasteryType.Cooking:
                        Player.TrophyUpdate(23100269, 1);
                        Player.TrophyUpdate(23100270, 1);
                        Player.TrophyUpdate(23100271, 1);
                        Player.TrophyUpdate(23100272, 1);
                        Player.TrophyUpdate(23100337, 1);
                        break;
                    case MasteryType.PetTaming:
                        Player.TrophyUpdate(23100273, 1);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
