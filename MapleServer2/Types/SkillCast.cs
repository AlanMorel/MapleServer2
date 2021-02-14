using System;
using MapleServer2.Data.Static;

namespace MapleServer2.Types
{
    public class SkillCast
    {
        #region Declaration
        private long SkillSN;
        private int EntityId;
        private int SkillId;
        private short SkillLevel;
        private int UnkValue; // unknown value
        private double SkillDamage = 0;

        public long GetSkillSN()
        {
            return SkillSN;
        }

        public int GetSkillId()
        {
            return SkillId;
        }

        public short GetSkillLevel()
        {
            return SkillLevel;
        }

        public int GetUnkValue()
        {
            return UnkValue;
        }

        public double GetDamageRate()
        {
            return SkillMetadataStorage.GetSkill(GetSkillId()).SkillLevels.Find(x => x.Level == GetSkillLevel()).DamageRate;
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
            if (roll > 0.5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public double GetCriticalDamage()
        {
            // TODO: Critic Damage calculation
            if (RollCrit() == true)
            {
                return GetDamageRate() + GetDamageRate();
            }
            else
            {
                return GetDamageRate();
            }
        }

        public int GetEntityId()
        {
            return EntityId;
        }
        #endregion

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
