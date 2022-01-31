using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Extensions;

namespace MapleServer2.Types;

public enum DamageType : byte
{
    None = 0x00,
    Physical = 0x01,
    Magic = 0x02
}
public enum Element : byte
{
    None = 0x00,
    Fire = 0x01,
    Ice = 0x02,
    Electric = 0x03,
    Holy = 0x04,
    Dark = 0x05,
    Poison = 0x06
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
    public SkillCast ParentSkill { get; private set; }

    public CoordF Position;
    public CoordF Direction;
    public CoordF Rotation;

    public SkillCast()
    {
        SkillId = 1;
        SkillLevel = 1;
    }

    public SkillCast(int id, short level)
    {
        SkillId = id;
        SkillLevel = level;
    }

    public SkillCast(int id, short level, long skillSN, int serverTick) : this(id, level)
    {
        SkillSN = skillSN;
        ServerTick = serverTick;
    }

    public SkillCast(int id, short level, long skillSN, int serverTick, SkillCast parentSkill) : this(id, level, skillSN, serverTick)
    {
        ParentSkill = parentSkill;
    }

    public SkillCast(int id, short level, long skillSN, int serverTick, int entityId, int clientTick) : this(id, level, skillSN, serverTick)
    {
        EntityId = entityId;
        ClientTick = clientTick;
    }

    public SkillCast(int id, short level, long skillSN, int serverTick, int entityId, int clientTick, byte attackPoint) : this(id, level, skillSN, serverTick)
    {
        AttackPoint = attackPoint;
        EntityId = entityId;
        ClientTick = clientTick;
    }

    public double GetDamageRate()
    {
        return GetCurrentLevel()?.DamageRate ?? 0.1f;
    }

    public double GetCriticalDamage()
    {
        return 2 * GetDamageRate();
    }

    public int GetSpCost()
    {
        return GetCurrentLevel()?.Spirit ?? 15;
    }

    public int GetStaCost()
    {
        return GetCurrentLevel()?.Stamina ?? 10;
    }

    public DamageType GetSkillDamageType()
    {
        return (DamageType) (GetSkillMetadata()?.DamageType ?? 0);
    }

    public Element GetElement()
    {
        return (Element) GetSkillMetadata().Element;
    }

    public bool IsSpRecovery()
    {
        return GetSkillMetadata().IsSpRecovery;
    }

    public int DurationTick()
    {
        return GetCurrentLevel()?.SkillAdditionalData.Duration ?? 5000;
    }

    public int MaxStack()
    {
        return GetCurrentLevel()?.SkillAdditionalData.MaxStack ?? 1;
    }

    public IEnumerable<SkillCondition> GetConditionSkill()
    {
        return GetCurrentLevel()?.SkillConditions;
    }

    public bool IsHeal()
    {
        return VerifySkillTypeOf(SkillType.None, SkillSubType.Status, BuffType.Buff, BuffSubType.Recovery);
    }

    public bool IsHealFromBuff()
    {
        return VerifySkillTypeOf(BuffType.Buff, BuffSubType.Recovery);
    }

    public bool IsGM()
    {
        return VerifySkillTypeOf(SkillType.GM, SkillSubType.GM, BuffType.Buff, BuffSubType.Recovery);
    }

    public bool IsGlobal()
    {
        return VerifySkillTypeOf(SkillType.None, SkillSubType.Global);
    }

    public bool IsBuffToOwner()
    {
        return VerifySkillTypeOf(SkillType.None, SkillSubType.Status, BuffType.Buff, BuffSubType.Owner);
    }

    public bool IsBuffToEntity()
    {
        return VerifySkillTypeOf(SkillType.None, SkillSubType.Status, BuffType.Buff, BuffSubType.Entity);
    }

    public bool IsDebuffToEntity()
    {
        return VerifySkillTypeOf(SkillType.None, SkillSubType.Status, BuffType.Debuff, BuffSubType.Entity) || VerifySkillTypeOf(BuffType.Debuff, BuffSubType.Entity);
    }

    public bool IsDebuffToOwner()
    {
        return VerifySkillTypeOf(BuffType.Debuff, BuffSubType.Owner);
    }

    public bool IsDebuffElement()
    {
        return VerifySkillTypeOf(SkillType.None, SkillSubType.Status, BuffType.Debuff, BuffSubType.Element);
    }

    public bool IsBuffShield()
    {
        return VerifySkillTypeOf(SkillType.None, SkillSubType.Status, BuffType.Buff, BuffSubType.Shield);
    }

    public bool IsChainSkill()
    {
        SkillMetadata skillData = GetSkillMetadata();
        return skillData.Type == SkillType.None.GetValue() && skillData.SubType == SkillType.None.GetValue();
    }

    private bool VerifySkillTypeOf(SkillType type, SkillSubType subType)
    {
        SkillMetadata skillData = GetSkillMetadata();
        if (skillData != null && skillData.Type == type.GetValue() && skillData.SubType == subType.GetValue())
        {
            return true;
        }
        return false;
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
            SkillAdditionalData skillAdditionalData = GetSkillMetadata()?.SkillLevels.Find(s => s.Level == SkillLevel)?.SkillAdditionalData;
            if (skillAdditionalData != null && skillAdditionalData.BuffType == buffType.GetValue() && skillAdditionalData.BuffSubType == buffSubType.GetValue())
            {
                return true;
            }
        }
        return false;
    }

    private SkillMetadata GetSkillMetadata()
    {
        return SkillMetadataStorage.GetSkill(SkillId);
    }

    private SkillLevel GetCurrentLevel()
    {
        return GetSkillMetadata()?.SkillLevels.Find(s => s.Level == SkillLevel);
    }

    public MagicPathMetadata GetMagicPaths()
    {
        long cubeMagicPath = GetCurrentLevel()?.SkillAttacks.FirstOrDefault().CubeMagicPathId ?? 0;
        return MagicPathMetadataStorage.GetMagicPath(cubeMagicPath);
    }
}
