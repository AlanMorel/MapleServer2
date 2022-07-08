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

    public SkillLevel() { }

    public SkillLevel(int level, int spirit, int stamina, string feature, List<SkillMotion> skillMotions, SkillUpgrade skillUpgrade, float cooldownTime)
    {
        Level = level;
        Spirit = spirit;
        Stamina = stamina;
        Feature = feature;
        SkillMotions = skillMotions;
        SkillUpgrade = skillUpgrade;
        CooldownTime = cooldownTime;
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

    public SkillAttack() { }

    public SkillAttack(byte attackPoint, short targetCount, long magicPathId, long cubeMagicPathId, RangeProperty rangeProperty,
        List<SkillCondition> skillConditions,
        DamageProperty damageProperty)
    {
        AttackPoint = attackPoint;
        TargetCount = targetCount;
        MagicPathId = magicPathId;
        CubeMagicPathId = cubeMagicPathId;
        RangeProperty = rangeProperty;
        SkillConditions = skillConditions;
        DamageProperty = damageProperty;
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
    // TODO: Parse push attributes.

    public DamageProperty() { }

    public DamageProperty(float damageRate, float hitSpeedRate)
    {
        DamageRate = damageRate;
        HitSpeedRate = hitSpeedRate;
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
    public readonly int SkillId;
    [XmlElement(Order = 2)]
    public readonly short SkillLevel;
    [XmlElement(Order = 3)]
    public readonly bool IsSplash;
    [XmlElement(Order = 4)]
    public readonly byte Target;
    [XmlElement(Order = 5)]
    public readonly byte Owner;
    [XmlElement(Order = 6)]
    public readonly short FireCount;
    [XmlElement(Order = 7)]
    public readonly int Interval;
    [XmlElement(Order = 8)]
    public readonly bool ImmediateActive;

    public SkillCondition() { }

    public SkillCondition(int skillId, short skillLevel, bool isSplash, byte target, byte owner, short fireCount, int interval, bool immediateActive)
    {
        SkillId = skillId;
        SkillLevel = skillLevel;
        IsSplash = isSplash;
        Target = target;
        Owner = owner;
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

    public RangeProperty() { }

    public RangeProperty(bool includeCaster, string rangeType, int distance, CoordF rangeAdd, CoordF rangeOffset)
    {
        IncludeCaster = includeCaster;
        RangeType = rangeType;
        Distance = distance;
        RangeAdd = rangeAdd;
        RangeOffset = rangeOffset;
    }
}
