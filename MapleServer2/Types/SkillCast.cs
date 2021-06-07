using MapleServer2.Data.Static;

namespace MapleServer2.Types
{
    public enum DamageTypeId : byte
    {
        None = 0x00,
        Physical = 0x01,
        Magic = 0x02,
    }

    public enum ElementId : byte
    {
        None = 0x00,
        Fire = 0x01,
        Ice = 0x02,
        Electric = 0x03,
        Holy = 0x04,
        Dark = 0x05,
        Poison = 0x06,
    }

    public class SkillCast
    {
        public long SkillSN { get; private set; }
        public int EntityId { get; private set; }
        public int SkillId { get; private set; }
        public short SkillLevel { get; private set; }
        public int UnkValue { get; private set; }

        public SkillCast()
        {
            SkillId = 1;
            SkillLevel = 1;
        }

        public SkillCast(int id, short level, long skillSN, int unkValue)
        {
            SkillSN = skillSN;
            SkillId = id;
            SkillLevel = level;
            UnkValue = unkValue;
        }

        public double GetDamageRate() => SkillMetadataStorage.GetSkill(SkillId).SkillLevels.Find(x => x.Level == SkillLevel).DamageRate;

        public double GetCriticalDamage() => 2 * GetDamageRate();

        public int GetSpCost() => SkillMetadataStorage.GetSkill(SkillId).SkillLevels.Find(s => s.Level == SkillLevel).Spirit;

        public int GetStaCost() => SkillMetadataStorage.GetSkill(SkillId).SkillLevels.Find(s => s.Level == SkillLevel).Stamina;

        public DamageTypeId GetSkillDamageType() => (DamageTypeId) SkillMetadataStorage.GetSkill(SkillId).DamageType;

        public ElementId GetElement() => (ElementId) SkillMetadataStorage.GetSkill(SkillId).Element;

        public bool IsSpRecovery() => SkillMetadataStorage.GetSkill(SkillId).IsSpRecovery;

        public bool IsBuff() => SkillMetadataStorage.GetSkill(SkillId).IsBuff;
    }
}
