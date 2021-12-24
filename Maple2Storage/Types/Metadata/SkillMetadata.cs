using System.Xml.Serialization;

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
    public short CurrentLevel;
    [XmlElement(Order = 6)]
    public readonly string State;
    [XmlElement(Order = 7)]
    public readonly byte DamageType;
    [XmlElement(Order = 8)]
    public readonly byte Type;
    [XmlElement(Order = 9)]
    public readonly byte SubType;
    [XmlElement(Order = 10)]
    public readonly byte Element;
    [XmlElement(Order = 11)]
    public readonly byte SuperArmor;
    [XmlElement(Order = 12)]
    public readonly bool IsSpRecovery;
    [XmlElement(Order = 13)]
    public short MaxLevel;

    public SkillMetadata() { }

    public SkillMetadata(int id, List<SkillLevel> skillLevels)
    {
        SkillId = id;
        SkillLevels = skillLevels;
    }

    public SkillMetadata(int id, List<SkillLevel> skillLevels, string state, byte damageType, byte type, byte subType, byte element, byte superArmor,
        bool isSpRecovery)
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
    public readonly float DamageRate;
    [XmlElement(Order = 5)]
    public readonly string Feature = "";
    [XmlElement(Order = 6)]
    public readonly SkillMotion SkillMotions;
    [XmlElement(Order = 7)]
    public readonly List<SkillAttack> SkillAttacks = new();
    [XmlElement(Order = 8)]
    public readonly List<SkillCondition> SkillConditions = new();
    [XmlElement(Order = 9)]
    public SkillAdditionalData SkillAdditionalData;
    [XmlElement(Order = 10)]
    public readonly SkillUpgrade SkillUpgrade;

    public SkillLevel() { }

    public SkillLevel(int level, SkillAdditionalData data)
    {
        Level = level;
        SkillAdditionalData = data;
        SkillMotions = new();
    }

    public SkillLevel(int level, int spirit, int stamina, float damageRate, string feature,
        SkillMotion skillMotions, List<SkillAttack> skillAttacks, List<SkillCondition> skillConditions, SkillUpgrade skillUpgrade)
    {
        Level = level;
        Spirit = spirit;
        Stamina = stamina;
        DamageRate = damageRate;
        Feature = feature;
        SkillMotions = skillMotions;
        SkillAttacks = skillAttacks;
        SkillConditions = skillConditions;
        SkillAdditionalData = new();
        SkillUpgrade = skillUpgrade;
    }

    public override string ToString()
    {
        return $"SkillLevel(Level:{Level},Spirit:{Spirit},Stamina:{Stamina},DamageRate:{DamageRate},Feature:{Feature}," +
               $"SkillMotion:{SkillMotions},SkillAttacks:{SkillAttacks},SkillConditions: {SkillConditions})";
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

    public SkillAttack() { }

    public SkillAttack(byte attackPoint, short targetCount, long magicPathId, long cubeMagicPathId)
    {
        AttackPoint = attackPoint;
        TargetCount = targetCount;
        MagicPathId = magicPathId;
        CubeMagicPathId = cubeMagicPathId;
    }

    public override string ToString()
    {
        return $"Point:{AttackPoint}, TargetCount:{TargetCount}, MagicPathId:{MagicPathId}";
    }
}

[XmlType]
public class SkillMotion
{
    [XmlElement(Order = 1)]
    public string SequenceName = "";
    [XmlElement(Order = 2)]
    public string MotionEffect = "";

    public SkillMotion() { }

    public SkillMotion(string sequenceName, string motionEffect)
    {
        SequenceName = sequenceName;
        MotionEffect = motionEffect;
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
    public readonly int BuffType;
    [XmlElement(Order = 3)]
    public readonly int BuffSubType;
    [XmlElement(Order = 4)]
    public readonly int BuffCategory;
    [XmlElement(Order = 5)]
    public readonly int EventBuffType;
    [XmlElement(Order = 6)]
    public readonly int MaxStack;
    [XmlElement(Order = 7)]
    public readonly byte KeepCondition;

    public SkillAdditionalData() { }

    public SkillAdditionalData(int duration, int buffType, int buffSubType, int buffCategory, int maxStack, byte keepCondition)
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
    public readonly int Id;
    [XmlElement(Order = 2)]
    public readonly short Level;
    [XmlElement(Order = 3)]
    public readonly bool Splash;
    [XmlElement(Order = 4)]
    public readonly byte Target;
    [XmlElement(Order = 5)]
    public readonly byte Owner;

    public SkillCondition() { }

    public SkillCondition(int id, short level, bool splash, byte target, byte owner)
    {
        Id = id;
        Level = level;
        Splash = splash;
        Target = target;
        Owner = owner;
    }

    public override string ToString()
    {
        return $"Id: {Id}, Level:{Level}, Splash:{Splash}, Target:{Target}, Owner:{Owner}";
    }
}
