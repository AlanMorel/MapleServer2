using System.Collections.Generic;
using MapleServer2.Enums;
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

        public double GetDamageRate() => SkillMetadataStorage.GetSkill(SkillId)?.SkillLevels.Find(x => x.Level == SkillLevel).DamageRate ?? 0.1f;

        public double GetCriticalDamage() => 2 * GetDamageRate();

        public int GetSpCost() => SkillMetadataStorage.GetSkill(SkillId)?.SkillLevels.Find(s => s.Level == SkillLevel).Spirit ?? 15;

        public int GetStaCost() => SkillMetadataStorage.GetSkill(SkillId)?.SkillLevels.Find(s => s.Level == SkillLevel).Stamina ?? 10;

        public DamageTypeId GetSkillDamageType()
        {
            if (SkillMetadataStorage.GetSkill(SkillId) != null)
            {
                return (DamageTypeId) SkillMetadataStorage.GetSkill(SkillId).DamageType;
            }
            else
            {
                return DamageTypeId.None;
            }
        }

        public ElementId GetElement() => (ElementId) SkillMetadataStorage.GetSkill(SkillId).Element;

        public bool IsSpRecovery() => SkillMetadataStorage.GetSkill(SkillId).IsSpRecovery;

        public int DurationTick() => SkillMetadataStorage.GetSkill(SkillId)?.SkillLevels.Find(s => s.Level == SkillLevel).SkillAdditionalData.Duration ?? 5000;

        public int MaxStack() => SkillMetadataStorage.GetSkill(SkillId)?.SkillLevels.Find(s => s.Level == SkillLevel).SkillAdditionalData.MaxStack ?? 1;

        public IEnumerable<int> GetConditionSkill() => SkillMetadataStorage.GetSkill(SkillId)?.SkillLevels.Find(s => s.Level == SkillLevel).SkillAttacks.ConditionSkillIds ?? null;

        public bool IsHeal() => VerifySkillTypeOf(SkillType.None, SkillSubType.Status, BuffType.Buff, BuffSubType.Recovery);

        private bool VerifySkillTypeOf(SkillType type, SkillSubType subType, BuffType buffType, BuffSubType buffSubType)
        {
            Maple2Storage.Types.Metadata.SkillMetadata skillData = SkillMetadataStorage.GetSkill(SkillId);
            if (skillData.Type == (int) type && skillData.SubType == (int) subType && skillData != null)
            {
                Maple2Storage.Types.Metadata.SkillAdditionalData skillAdditionalData = skillData.SkillLevels.Find(s => s.Level == SkillLevel).SkillAdditionalData;
                if (skillAdditionalData.BuffType == (int) buffType && skillAdditionalData.BuffSubType == (int) buffSubType && skillAdditionalData != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
