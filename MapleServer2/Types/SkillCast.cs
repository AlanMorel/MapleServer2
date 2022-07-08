using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;

namespace MapleServer2.Types;

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
    public byte AttackPoint { get; set; }
    public int Duration;
    public int Interval;
    public SkillCast ParentSkill { get; }

    public SkillAttack SkillAttack;

    public CoordF Position;
    public CoordF Direction;
    public CoordF Rotation;
    public short LookDirection;
    public float AimAngle;

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
        SkillAttack = GetSkillMotions()?.FirstOrDefault()?.SkillAttacks.FirstOrDefault();
    }

    public SkillCast(int id, short level, long skillSn, int serverTick) : this(id, level)
    {
        SkillSn = skillSn;
        ServerTick = serverTick;
    }

    public SkillCast(int id, short level, long skillSn, int serverTick, SkillCast parentSkill) : this(id, level, skillSn, serverTick)
    {
        ParentSkill = parentSkill;
        CasterObjectId = parentSkill.CasterObjectId;
        Position = parentSkill.Position;
        Rotation = parentSkill.Rotation;
        LookDirection = parentSkill.LookDirection;
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
    }

    public float GetDamageRate() => SkillAttack?.DamageProperty.DamageRate ?? 0;

    public double GetCriticalDamage() => 2 * GetDamageRate();

    public int GetSpCost() => GetCurrentLevel()?.Spirit ?? 15;

    public int GetStaCost() => GetCurrentLevel()?.Stamina ?? 10;

    public DamageType GetSkillDamageType() => (DamageType) (GetSkillMetadata()?.DamageType ?? 0);

    public Element GetElement() => (Element) GetSkillMetadata().Element;

    public bool IsSpRecovery() => GetSkillMetadata().IsSpRecovery;

    public bool IsGuaranteedCrit() => false;

    public int DurationTick()
    {
        int? durationTick = GetAdditionalData()?.Duration;
        return durationTick ?? 5000;
    }

    public int MaxStack() => GetAdditionalData()?.MaxStack ?? 1;

    public bool IsRecovery() => VerifySkillTypeOf(SkillType.Active, SkillSubType.Status, BuffType.Buff, BuffSubType.Recovery);

    public bool IsRecoveryFromBuff() => VerifySkillTypeOf(BuffType.Buff, BuffSubType.Recovery);

    public SkillRangeType GetRangeType() => GetSkillMetadata().RangeType;

    public int[] GetSkillGroups() => GetSkillMetadata().GroupIDs;

    public bool IsGM() => VerifySkillTypeOf(SkillType.GM, SkillSubType.GM, BuffType.Buff, BuffSubType.Recovery);

    public bool IsGlobal() => VerifySkillTypeOf(SkillType.Active, SkillSubType.Global);

    public bool IsBuff() => VerifySkillTypeOf(BuffType.Buff);

    public bool IsUnspecifiedBuff() => VerifySkillTypeOf(SkillType.Active, SkillSubType.Status, BuffType.Buff, BuffSubType.None);

    public bool IsBuffToOwner() => VerifySkillTypeOf(SkillType.Active, SkillSubType.Status, BuffType.Buff, BuffSubType.Owner);

    public bool IsBuffToEntity() => VerifySkillTypeOf(SkillType.Active, SkillSubType.Status, BuffType.Buff, BuffSubType.Entity);

    public bool IsDebuffToEntity()
    {
        return VerifySkillTypeOf(SkillType.Active, SkillSubType.Status, BuffType.Debuff, BuffSubType.Entity) ||
               VerifySkillTypeOf(BuffType.Debuff, BuffSubType.Entity);
    }

    public bool IsStatus() => VerifySkillTypeOf(SkillType.Active, SkillSubType.Status);

    public bool IsDebuffToOwner() => VerifySkillTypeOf(BuffType.Debuff, BuffSubType.Owner);

    public bool IsDebuffElement() => VerifySkillTypeOf(SkillType.Active, SkillSubType.Status, BuffType.Debuff, BuffSubType.Element);

    public bool IsBuffShield() => VerifySkillTypeOf(SkillType.Active, SkillSubType.Status, BuffType.Buff, BuffSubType.Shield);

    public bool IsChainSkill()
    {
        SkillMetadata skillData = GetSkillMetadata();
        return skillData.Type == SkillType.Active && skillData.SubType == SkillSubType.None;
    }

    public List<SkillMotion> GetSkillMotions() => GetCurrentLevel()?.SkillMotions;

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

        SkillAdditionalData skillAdditionalData = GetAdditionalData();
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

        SkillAdditionalData skillAdditionalData = GetAdditionalData();
        if (skillAdditionalData is null)
        {
            return false;
        }

        return skillAdditionalData.BuffType == buffType && skillAdditionalData.BuffSubType == buffSubType;
    }

    private bool VerifySkillTypeOf(BuffType buffType)
    {
        if (IsChainSkill())
        {
            return false;
        }

        SkillAdditionalData skillAdditionalData = GetAdditionalData();
        if (skillAdditionalData is null)
        {
            return false;
        }

        return skillAdditionalData.BuffType == buffType;
    }

    private SkillMetadata GetSkillMetadata() => SkillMetadataStorage.GetSkill(SkillId);

    private SkillLevel GetCurrentLevel() => GetSkillMetadata()?.SkillLevels.FirstOrDefault(s => s.Level == SkillLevel);

    // Some skills don't have SkillAdditionalData for specific levels, if its null try to use the first level
    private SkillAdditionalData GetAdditionalData() =>
        GetCurrentLevel()?.SkillAdditionalData ?? GetSkillMetadata()?.SkillLevels.FirstOrDefault()?.SkillAdditionalData;
}
