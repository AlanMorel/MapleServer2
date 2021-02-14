using System;
using MapleServer2.Data.Static;

namespace MapleServer2.Types
{
    public class SkillCast
    {
        public long SkillSN { get; private set; }
        public int EntityId { get; private set; }
        public int SkillId { get; private set; }
        public short SkillLevel { get; private set; }
        public int UnkValue { get; private set; }

        private double SkillDamage = 0;

        public double GetDamageRate()
        {
            return SkillMetadataStorage.GetSkill(SkillId).SkillLevels.Find(x => x.Level == SkillLevel).DamageRate;
        }

        public double GetDamage()
        {
            SkillCast skillCast = this;
            SkillDamage = DamageHandler.CalculateSkillDamage(skillCast).GetDamage();
            return SkillDamage;
        }

        public bool RollCrit()
        {
            // TODO: Critic base on Stats
            Random rnd = new Random();
            double roll = rnd.NextDouble();
            return roll > 0.5;
        }

        public double GetCriticalDamage()
        {
            // TODO: Critic Damage calculation
            return RollCrit() ? GetDamageRate() * 2 : GetDamageRate();
        }

        public SkillCast(int id, short level, long skillSN, int value)
        {
            SkillSN = skillSN;
            SkillId = id;
            SkillLevel = level;
            UnkValue = value;
        }

        // Required for first time enter the game
        public SkillCast()
        {
            SkillId = 1;
            SkillLevel = 1;
        }
    }
}
