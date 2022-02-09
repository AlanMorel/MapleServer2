using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;

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
    public long SkillSn { get; }
    public int CasterObjectId { get; set; }
    public int SkillObjectId { get; set; }
    public int SkillId { get; }
    public short SkillLevel { get; }
    public int ClientTick { get; }
    public int ServerTick { get; }
    public byte MotionPoint { get; }
    public byte AttackPoint { get; }
    public int Duration;
    public int Interval;
    public SkillCast ParentSkill { get; }

    public SkillAttack SkillAttack;

    public CoordF Position;
    public CoordF Direction;
    public CoordF Rotation;

    public IFieldActor<NpcMetadata> Target;

    public bool MetadataExists => GetSkillMetadata() is not null;

    public List<CoordF> EffectCoords = new();

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

    public SkillCast(int id, short level, long skillSn, int serverTick) : this(id, level)
    {
        SkillSn = skillSn;
        ServerTick = serverTick;
    }

    public SkillCast(int id, short level, long skillSn, int serverTick, SkillCast parentSkill) : this(id, level, skillSn, serverTick)
    {
        ParentSkill = parentSkill;
    }

    public SkillCast(int id, short level, long skillSn, int serverTick, int casterObjectId, int clientTick) : this(id, level, skillSn, serverTick)
    {
        CasterObjectId = casterObjectId;
        ClientTick = clientTick;
    }

    public SkillCast(int id, short level, long skillSn, int serverTick, int casterObjectId, int clientTick, byte attackPoint) : this(id, level, skillSn,
        serverTick)
    {
        AttackPoint = attackPoint;
        CasterObjectId = casterObjectId;
        ClientTick = clientTick;
        SkillAttack = GetSkillMotions().First().SkillAttacks.First();
    }

    public float GetDamageRate() => SkillAttack.DamageProperty.DamageRate;

    public double GetCriticalDamage() => 2 * GetDamageRate();

    public int GetSpCost() => GetCurrentLevel()?.Spirit ?? 15;

    public int GetStaCost() => GetCurrentLevel()?.Stamina ?? 10;

    public DamageType GetSkillDamageType() => (DamageType) (GetSkillMetadata()?.DamageType ?? 0);

    public Element GetElement() => (Element) GetSkillMetadata().Element;

    public bool IsSpRecovery() => GetSkillMetadata().IsSpRecovery;

    public int DurationTick()
    {
        int? durationTick = GetCurrentLevel()?.SkillAdditionalData.Duration;
        return durationTick ?? 5000;
    }

    public int MaxStack() => GetCurrentLevel()?.SkillAdditionalData.MaxStack ?? 1;

    public bool IsRecovery() => VerifySkillTypeOf(SkillType.None, SkillSubType.Status, BuffType.Buff, BuffSubType.Recovery);

    public bool IsRecoveryFromBuff() => VerifySkillTypeOf(BuffType.Buff, BuffSubType.Recovery);

    public bool IsGM() => VerifySkillTypeOf(SkillType.GM, SkillSubType.GM, BuffType.Buff, BuffSubType.Recovery);

    public bool IsGlobal() => VerifySkillTypeOf(SkillType.None, SkillSubType.Global);

    public bool IsBuffToOwner() => VerifySkillTypeOf(SkillType.None, SkillSubType.Status, BuffType.Buff, BuffSubType.Owner);

    public bool IsBuffToEntity() => VerifySkillTypeOf(SkillType.None, SkillSubType.Status, BuffType.Buff, BuffSubType.Entity);

    public bool IsDebuffToEntity()
    {
        return VerifySkillTypeOf(SkillType.None, SkillSubType.Status, BuffType.Debuff, BuffSubType.Entity) ||
               VerifySkillTypeOf(BuffType.Debuff, BuffSubType.Entity);
    }

    public bool IsDebuffToOwner() => VerifySkillTypeOf(BuffType.Debuff, BuffSubType.Owner);

    public bool IsDebuffElement() => VerifySkillTypeOf(SkillType.None, SkillSubType.Status, BuffType.Debuff, BuffSubType.Element);

    public bool IsBuffShield() => VerifySkillTypeOf(SkillType.None, SkillSubType.Status, BuffType.Buff, BuffSubType.Shield);

    public bool IsChainSkill()
    {
        SkillMetadata skillData = GetSkillMetadata();
        return skillData.Type == SkillType.None && skillData.SubType == SkillSubType.None;
    }

    public List<SkillMotion> GetSkillMotions() => GetCurrentLevel().SkillMotions;

    private bool VerifySkillTypeOf(SkillType type, SkillSubType subType)
    {
        SkillMetadata skillData = GetSkillMetadata();
        if (skillData is null)
        {
            return false;
        }

        return skillData.Type == type && skillData.SubType == subType;
    }

    private bool VerifySkillTypeOf(SkillType type, SkillSubType subType, BuffType buffType, BuffSubType buffSubType)
    {
        SkillMetadata skillData = GetSkillMetadata();
        if (skillData is null)
        {
            return false;
        }

        if (skillData.Type != type || skillData.SubType != subType)
        {
            return false;
        }

        SkillAdditionalData skillAdditionalData = skillData.SkillLevels.Find(s => s.Level == SkillLevel)?.SkillAdditionalData;
        if (skillAdditionalData is null)
        {
            return false;
        }

        return skillAdditionalData.BuffType == buffType && skillAdditionalData.BuffSubType == buffSubType;
    }

    private bool VerifySkillTypeOf(BuffType buffType, BuffSubType buffSubType)
    {
        if (IsChainSkill())
        {
            return false;
        }

        SkillAdditionalData skillAdditionalData = GetCurrentLevel()?.SkillAdditionalData;
        if (skillAdditionalData is not null)
        {
            return skillAdditionalData.BuffType == buffType && skillAdditionalData.BuffSubType == buffSubType;
        }

        // Some skills don't have SkillAdditionalData for specific levels, try to use the first level
        skillAdditionalData = GetSkillMetadata().SkillLevels.FirstOrDefault()?.SkillAdditionalData;
        if (skillAdditionalData is null)
        {
            return false;
        }

        return skillAdditionalData.BuffType == buffType && skillAdditionalData.BuffSubType == buffSubType;
    }

    private SkillMetadata GetSkillMetadata() => SkillMetadataStorage.GetSkill(SkillId);

    private SkillLevel GetCurrentLevel() => GetSkillMetadata()?.SkillLevels.FirstOrDefault(s => s.Level == SkillLevel);
}
