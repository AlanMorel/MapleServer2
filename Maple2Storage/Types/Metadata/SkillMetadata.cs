using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class SkillMetadata
{
    [XmlElement(Order = 1)]
    public readonly int SkillId;
    [XmlElement(Order = 2)]
    public readonly List<SkillLevel> SkillLevels = new();
    [XmlElement(Order = 3)]
    public int[] SubSkills = Array.Empty<int>();
    [XmlElement(Order = 4)]
    public int Job;
    [XmlElement(Order = 5)]
    public readonly string State;
    [XmlElement(Order = 6)]
    public readonly byte DamageType;
    [XmlElement(Order = 7)]
    public readonly SkillType Type;
    [XmlElement(Order = 8)]
    public readonly SkillSubType SubType;
    [XmlElement(Order = 9)]
    public readonly byte Element;
    [XmlElement(Order = 10)]
    public readonly byte SuperArmor;
    [XmlElement(Order = 11)]
    public readonly bool IsSpRecovery;
    [XmlElement(Order = 12)]
    public short MaxLevel;
    [XmlElement(Order = 13)]
    public SkillRangeType RangeType;
    [XmlElement(Order = 14)]
    public int[] GroupIDs;

    public SkillMetadata() { }

    public SkillMetadata(int id, List<SkillLevel> skillLevels)
    {
        SkillId = id;
        SkillLevels = skillLevels;
    }

    public SkillMetadata(int id, List<SkillLevel> skillLevels, string state, byte damageType, SkillType type, SkillSubType subType, byte element,
        byte superArmor, bool isSpRecovery, SkillRangeType rangeType, int[] groupIds)
    {
        SkillId = id;
        SkillLevels = skillLevels;
        State = state;
        DamageType = damageType;
        Type = type;
        SubType = subType;
        Element = element;
        SuperArmor = superArmor;
        IsSpRecovery = isSpRecovery;
        RangeType = rangeType;
        GroupIDs = groupIds;
    }

    public override string ToString()
    {
        return $"Skill:(Id:{SkillId},Job:{Job},SkillLevel:{string.Join(",", SkillLevels)}";
    }
}

[XmlType]
public class SkillLevel
{
    [XmlElement(Order = 1)]
    public readonly int Level;
    [XmlElement(Order = 2)]
    public readonly int Spirit;
    [XmlElement(Order = 3)]
    public readonly int Stamina;
    [XmlElement(Order = 4)]
    public readonly string Feature = "";
    [XmlElement(Order = 5)]
    public readonly List<SkillMotion> SkillMotions;
    [XmlElement(Order = 6)]
    public SkillAdditionalData SkillAdditionalData;
    [XmlElement(Order = 7)]
    public readonly SkillUpgrade SkillUpgrade;
    [XmlElement(Order = 8)]
    public readonly float CooldownTime;
    [XmlElement(Order = 9)]
    public readonly List<SkillCondition> ConditionSkills;
    [XmlElement(Order = 10)]
    public readonly SkillBeginCondition BeginCondition;

    public SkillLevel() { }

    public SkillLevel(int level, int spirit, int stamina, string feature, List<SkillCondition> conditionSkills, List<SkillMotion> skillMotions, SkillUpgrade skillUpgrade, float cooldownTime, SkillBeginCondition beginCondition)
    {
        Level = level;
        Spirit = spirit;
        Stamina = stamina;
        Feature = feature;
        ConditionSkills = conditionSkills;
        SkillMotions = skillMotions;
        SkillUpgrade = skillUpgrade;
        CooldownTime = cooldownTime;
        BeginCondition = beginCondition;
    }

    public override string ToString()
    {
        return $"SkillLevel(Level:{Level},Spirit:{Spirit},Stamina:{Stamina},Feature:{Feature}," +
               $"SkillMotion:{SkillMotions})";
    }
}

[XmlType]
public class SkillUpgrade
{
    [XmlElement(Order = 1)]
    public readonly int LevelRequired;
    [XmlElement(Order = 2)]
    public readonly int[] SkillIdsRequired = Array.Empty<int>();
    [XmlElement(Order = 3)]
    public readonly short[] SkillLevelsRequired = Array.Empty<short>();

    public SkillUpgrade() { }

    public SkillUpgrade(int levelRequired, int[] skillIds, short[] skillLevels)
    {
        LevelRequired = levelRequired;
        SkillIdsRequired = skillIds;
        SkillLevelsRequired = skillLevels;
    }

    public override string ToString()
    {
        return $"LevelRequired: {LevelRequired},SkillIds:{string.Join(",", SkillIdsRequired)}, SkillLevels: {string.Join(",", SkillLevelsRequired)}";
    }
}

[XmlType] // TODO: More to implement from attack attribute.
public class SkillAttack
{
    [XmlElement(Order = 1)]
    public readonly byte AttackPoint;
    [XmlElement(Order = 2)]
    public readonly short TargetCount;
    [XmlElement(Order = 3)]
    public readonly long MagicPathId;
    [XmlElement(Order = 4)]
    public readonly long CubeMagicPathId;
    [XmlElement(Order = 5)]
    public readonly RangeProperty RangeProperty;
    [XmlElement(Order = 6)]
    public readonly List<SkillCondition> SkillConditions;
    [XmlElement(Order = 7)]
    public readonly DamageProperty DamageProperty;
    [XmlElement(Order = 8)]
    public readonly int[] CompulsionType;
    [XmlElement(Order = 9)]
    public readonly SkillDirection Direction;

    public SkillAttack() { }

    public SkillAttack(byte attackPoint, short targetCount, long magicPathId, long cubeMagicPathId, RangeProperty rangeProperty,
        List<SkillCondition> skillConditions,
        DamageProperty damageProperty, int[] compulsionType, SkillDirection direction)
    {
        AttackPoint = attackPoint;
        TargetCount = targetCount;
        MagicPathId = magicPathId;
        CubeMagicPathId = cubeMagicPathId;
        RangeProperty = rangeProperty;
        SkillConditions = skillConditions;
        DamageProperty = damageProperty;
        CompulsionType = compulsionType;
        Direction = direction;
    }

    public override string ToString()
    {
        return $"Point:{AttackPoint}, TargetCount:{TargetCount}, MagicPathId:{MagicPathId}";
    }
}

[XmlType]
public class DamageProperty
{
    [XmlElement(Order = 1)]
    public readonly float DamageRate;
    [XmlElement(Order = 2)]
    public readonly float HitSpeedRate;
    [XmlElement(Order = 3)]
    public readonly int Count;
    // TODO: Parse push attributes.

    public DamageProperty() { }

    public DamageProperty(float damageRate, float hitSpeedRate, int count)
    {
        DamageRate = damageRate;
        HitSpeedRate = hitSpeedRate;
        Count = count;
    }
}

[XmlType]
public class SkillMotion
{
    // TODO: Move sequence and effect to a separate class as MotionProperty.
    [XmlElement(Order = 1)]
    public string SequenceName = "";
    [XmlElement(Order = 2)]
    public string MotionEffect = "";
    [XmlElement(Order = 3)]
    public List<SkillAttack> SkillAttacks = new();

    public SkillMotion() { }

    public SkillMotion(string sequenceName, string motionEffect, List<SkillAttack> skillAttacks)
    {
        SequenceName = sequenceName;
        MotionEffect = motionEffect;
        SkillAttacks = skillAttacks;
    }

    public override string ToString()
    {
        return $"SequenceName:{SequenceName},MotionEffect:{MotionEffect}";
    }
}

[XmlType] // TODO: More to implement, like skill sequences, stats power up, additional MotionEffects...
public class SkillAdditionalData
{
    [XmlElement(Order = 1)]
    public readonly int Duration;
    [XmlElement(Order = 2)]
    public readonly BuffType BuffType;
    [XmlElement(Order = 3)]
    public readonly BuffSubType BuffSubType;
    [XmlElement(Order = 4)]
    public readonly int BuffCategory;
    [XmlElement(Order = 5)]
    public readonly int EventBuffType;
    [XmlElement(Order = 6)]
    public readonly int MaxStack;
    [XmlElement(Order = 7)]
    public readonly byte KeepCondition;

    public SkillAdditionalData() { }

    public SkillAdditionalData(int duration, BuffType buffType, BuffSubType buffSubType, int buffCategory, int maxStack, byte keepCondition)
    {
        Duration = duration;
        BuffType = buffType;
        BuffSubType = buffSubType;
        BuffCategory = buffCategory;
        MaxStack = maxStack;
        KeepCondition = keepCondition;
    }

    public override string ToString()
    {
        return $"DurationTick: {Duration}; Type:{BuffType}; SubType:{BuffSubType};";
    }
}

[XmlType]
public class SkillCondition
{
    [XmlElement(Order = 1)]
    public readonly int[] SkillId;
    [XmlElement(Order = 2)]
    public readonly short[] SkillLevel;
    [XmlElement(Order = 3)]
    public readonly bool IsSplash;
    [XmlElement(Order = 4)]
    public readonly SkillTarget Target;
    [XmlElement(Order = 5)]
    public readonly SkillOwner Owner;
    [XmlElement(Order = 6)]
    public readonly short FireCount;
    [XmlElement(Order = 7)]
    public readonly int Interval;
    [XmlElement(Order = 8)]
    public readonly bool ImmediateActive;
    [XmlElement(Order = 9)]
    public uint Delay;
    [XmlElement(Order = 10)]
    public int RemoveDelay;
    [XmlElement(Order = 11)]
    public SkillBeginCondition BeginCondition;
    [XmlElement(Order = 12)]
    public bool UseDirection;
    [XmlElement(Order = 13)]
    public bool RandomCast;
    [XmlElement(Order = 14)]
    public int[] LinkSkillId;
    [XmlElement(Order = 15)]
    public int OverlapCount;
    [XmlElement(Order = 16)]
    public bool NonTargetActive;
    [XmlElement(Order = 17)]
    public bool OnlySensingActive;
    [XmlElement(Order = 18)]
    public bool DependOnCasterState;
    [XmlElement(Order = 19)]
    public bool ActiveByIntervalTick;
    [XmlElement(Order = 20)]
    public bool DependOnDamageCount;

    public SkillCondition() { }

    public SkillCondition(int[] skillId, short[] skillLevel, bool isSplash, byte target, byte owner, short fireCount, int interval, bool immediateActive)
    {
        SkillId = skillId;
        SkillLevel = skillLevel;
        IsSplash = isSplash;
        Target = (SkillTarget) target;
        Owner = (SkillOwner) owner;
        FireCount = fireCount;
        Interval = interval;
        ImmediateActive = immediateActive;
    }

    public override string ToString()
    {
        return $"Id: {SkillId}, Level:{SkillLevel}, Splash:{IsSplash}, Target:{Target}, Owner:{Owner}";
    }
}

[XmlType]
public class SkillBeginCondition
{
    [XmlElement(Order = 1)]
    public BeginConditionSubject Owner;

    [XmlElement(Order = 2)]
    public float Probability;

    [XmlElement(Order = 3)]
    public BeginConditionSubject Target;

    [XmlElement(Order = 4)]
    public BeginConditionSubject Caster;

    [XmlElement(Order = 5)]
    public float InvokeEffectFactor;

    [XmlElement(Order = 6)]
    public float CooldownTime;

    [XmlElement(Order = 7)]
    public float DefaultRechargingCooldownTime;

    [XmlElement(Order = 8)]
    public bool AllowDeadState;

    [XmlElement(Order = 9)]
    public float RequireDurationWithoutMove;

    [XmlElement(Order = 10)]
    public bool UseTargetCountFactor;

    [XmlElement(Order = 11)]
    public StatCondition? Stat;

    [XmlElement(Order = 12)]
    public List<RequireSkillCodeCondition> RequireSkillCodes;

    [XmlElement(Order = 13)]
    public List<RequireMapCodeCondition> RequireMapCodes;

    [XmlElement(Order = 14)]
    public List<RequireMapCategoryCodeCondition> RequireMapCategoryCodes;

    [XmlElement(Order = 15)]
    public List<RequireDungeonRoomCondition> RequireDungeonRooms;

    [XmlElement(Order = 16)]
    public List<JobCondition> Jobs;

    [XmlElement(Order = 17)]
    public List<RequireContinentCondition> MapContinents;
}

[XmlType]
public class BeginConditionSubject
{
    [XmlElement(Order = 1)]
    public int[] EventSkillIDs;

    [XmlElement(Order = 2)]
    public int[] EventEffectIDs;

    [XmlElement(Order = 3)]
    public int RequireBuffId;

    [XmlElement(Order = 4)]
    public int RequireBuffCount;

    [XmlElement(Order = 5)]
    public int HasNotBuffId;

    [XmlElement(Order = 6)]
    public EffectEvent EventCondition;

    [XmlElement(Order = 7)]
    public int IgnoreOwnerEvent;
    // 0: require specific ids with the event
    // 1: dont require specific ids with the event
    // ID: ignore specific event ids

    [XmlElement(Order = 8)]
    public int TargetCheckRange;

    [XmlElement(Order = 9)]
    public int TargetCheckMinRange;

    [XmlElement(Order = 10)]
    public int TargetInRangeCount;

    [XmlElement(Order = 11)]
    public TargetAllieganceType TargetFriendly;

    [XmlElement(Order = 12)]
    public ConditionOperator TargetCountSign;

    [XmlElement(Order = 13)]
    public CompareStatCondition? CompareStat;

    [XmlElement(Order = 14)]
    public ConditionOperator RequireBuffCountCompare;

    [XmlElement(Order = 15)]
    public int RequireBuffLevel;
}

[XmlType]
public class CompareStatCondition
{
    [XmlElement(Order = 1)]
    public int Hp;

    [XmlElement(Order = 2)]
    public ConditionOperator Func;
}

[XmlType]
public class StatCondition
{
    [XmlElement(Order = 1)]
    public int Hp;

    [XmlElement(Order = 2)]
    public int Sp;
}

[XmlType]
public class RequireSkillCodeCondition
{
    [XmlElement(Order = 1)]
    public int[] Code;
}

[XmlType]
public class RequireMapCodeCondition
{
    [XmlElement(Order = 1)]
    public int[] Code;
}

[XmlType]
public class RequireMapCategoryCodeCondition
{
    [XmlElement(Order = 1)]
    public int[] Code;
}

[XmlType]
public class RequireDungeonRoomCondition
{
    [XmlElement(Order = 1)]
    public DungeonRoomGroupType[] Code;
}

[XmlType]
public class JobCondition
{
    [XmlElement(Order = 1)]
    public int[] Code;
}

[XmlType]
public class RequireContinentCondition
{
    [XmlElement(Order = 1)]
    public ContinentCode Code;
}

[XmlType]
public class WeaponCondition
{
    [XmlElement(Order = 1)]
    public ItemPresetType LeftHand;

    [XmlElement(Order = 2)]
    public ItemPresetType RightHand;
}

[XmlType]
public class RangeProperty
{
    [XmlElement(Order = 1)]
    public readonly bool IncludeCaster;
    [XmlElement(Order = 2)]
    public readonly string RangeType;
    [XmlElement(Order = 3)]
    public readonly int Distance;
    [XmlElement(Order = 4)]
    public readonly CoordF RangeAdd;
    [XmlElement(Order = 5)]
    public readonly CoordF RangeOffset;
    [XmlElement(Order = 6)]
    public readonly ApplyTarget ApplyTarget;

    public RangeProperty() { }

    public RangeProperty(bool includeCaster, string rangeType, int distance, CoordF rangeAdd, CoordF rangeOffset, ApplyTarget applyTarget)
    {
        IncludeCaster = includeCaster;
        RangeType = rangeType;
        Distance = distance;
        RangeAdd = rangeAdd;
        RangeOffset = rangeOffset;
        ApplyTarget = applyTarget;
    }
}
