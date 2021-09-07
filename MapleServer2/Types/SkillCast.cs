using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Extensions;

namespace MapleServer2.Types
{
    public enum DamageType : byte
    {
        None = 0x00,
        Physical = 0x01,
        Magic = 0x02,
    }

    public enum Element : byte
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
        public int ClientTick { get; private set; }
        public int ServerTick { get; private set; }
        public byte MotionPoint { get; private set; }
        public byte AttackPoint { get; private set; }

        public SkillCast()
        {
            SkillId = 1;
            SkillLevel = 1;
        }

        public SkillCast(int id, short level, long skillSN, int serverTick)
        {
            SkillSN = skillSN;
            SkillId = id;
            SkillLevel = level;
            ServerTick = serverTick;
        }

        public SkillCast(int id, short level, long skillSN, int serverTick, int entityId, int clientTick, byte attackPoint) : this(id, level, skillSN, serverTick)
        {
            AttackPoint = attackPoint;
            EntityId = entityId;
            ClientTick = clientTick;
        }

        public double GetDamageRate() => GetSkillMetadata()?.SkillLevels.Find(x => x.Level == SkillLevel).DamageRate ?? 0.1f;

        public double GetCriticalDamage() => 2 * GetDamageRate();

        public int GetSpCost() => GetSkillMetadata()?.SkillLevels.Find(s => s.Level == SkillLevel).Spirit ?? 15;

        public int GetStaCost() => GetSkillMetadata()?.SkillLevels.Find(s => s.Level == SkillLevel).Stamina ?? 10;

        public DamageType GetSkillDamageType() => (DamageType) (GetSkillMetadata()?.DamageType ?? 0);

        public Element GetElement() => (Element) GetSkillMetadata().Element;

        public bool IsSpRecovery() => GetSkillMetadata().IsSpRecovery;

        public int DurationTick() => GetSkillMetadata()?.SkillLevels.Find(s => s.Level == SkillLevel).SkillAdditionalData.Duration ?? 5000;

        public int MaxStack() => GetSkillMetadata()?.SkillLevels.Find(s => s.Level == SkillLevel).SkillAdditionalData.MaxStack ?? 1;

        public IEnumerable<SkillAttack> GetConditionSkill() => GetSkillMetadata()?.SkillLevels.Find(s => s.Level == SkillLevel)?.SkillAttacks?.ToList();

        public bool IsHeal() => VerifySkillTypeOf(SkillType.None, SkillSubType.Status, BuffType.Buff, BuffSubType.Recovery);

        public bool IsBuffToOwner() => VerifySkillTypeOf(SkillType.None, SkillSubType.Status, BuffType.Buff, BuffSubType.Owner);

        public bool IsBuffToEntity() => VerifySkillTypeOf(SkillType.None, SkillSubType.Status, BuffType.Buff, BuffSubType.Entity);

        public bool IsDebuffToEntity() => VerifySkillTypeOf(SkillType.None, SkillSubType.Status, BuffType.Debuff, BuffSubType.Entity) || VerifySkillTypeOf(BuffType.Debuff, BuffSubType.Entity);

        public bool IsDebuffToOwner() => VerifySkillTypeOf(BuffType.Debuff, BuffSubType.Owner);

        public bool IsDebuffElement() => VerifySkillTypeOf(SkillType.None, SkillSubType.Status, BuffType.Debuff, BuffSubType.Element);

        public bool IsBuffShield() => VerifySkillTypeOf(SkillType.None, SkillSubType.Status, BuffType.Buff, BuffSubType.Shield);

        public bool IsChainSkill()
        {
            SkillMetadata skillData = GetSkillMetadata();
            return skillData.Type == SkillType.None.GetValue() && skillData.SubType == SkillType.None.GetValue();
        }

        private bool VerifySkillTypeOf(SkillType type, SkillSubType subType, BuffType buffType, BuffSubType buffSubType)
        {
            SkillMetadata skillData = GetSkillMetadata();
            if (skillData != null && skillData.Type == type.GetValue() && skillData.SubType == subType.GetValue())
            {
                SkillAdditionalData skillAdditionalData = skillData.SkillLevels.Find(s => s.Level == SkillLevel).SkillAdditionalData;
                if (skillAdditionalData != null && skillAdditionalData.BuffType == buffType.GetValue() && skillAdditionalData.BuffSubType == buffSubType.GetValue())
                {
                    return true;
                }
            }
            return false;
        }

        private bool VerifySkillTypeOf(BuffType buffType, BuffSubType buffSubType)
        {
            if (IsChainSkill())
            {
                SkillAdditionalData skillAdditionalData = GetSkillMetadata()?.SkillLevels.Find(s => s.Level == SkillLevel).SkillAdditionalData;
                if (skillAdditionalData != null && skillAdditionalData.BuffType == buffType.GetValue() && skillAdditionalData.BuffSubType == buffSubType.GetValue())
                {
                    return true;
                }
            }
            return false;
        }

        private SkillMetadata GetSkillMetadata() => SkillMetadataStorage.GetSkill(SkillId);
    }
}
