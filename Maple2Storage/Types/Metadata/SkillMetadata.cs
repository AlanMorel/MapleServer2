using System.Xml.Serialization;


namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class SkillMetadata
    {
        [XmlElement(Order = 1)]
        public readonly int SkillId;
        [XmlElement(Order = 2)]
        public readonly List<SkillLevel> SkillLevels;
        [XmlElement(Order = 3)]
        public int[] SubSkills = Array.Empty<int>();
        [XmlElement(Order = 4)]
        public int Job;
        [XmlElement(Order = 5)]
        public byte Learned = 0;
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

        public SkillMetadata()
        {
            SkillLevels = new List<SkillLevel>();
        }

        public SkillMetadata(int id, List<SkillLevel> skillLevels)
        {
            SkillId = id;
            SkillLevels = skillLevels;
        }

        public SkillMetadata(int id, List<SkillLevel> skillLevels, string state, byte damageType, byte type, byte subType, byte element, byte superArmor, bool isSpRecovery)
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

        public SkillMetadata(int id, List<SkillLevel> skillLevels, int[] subSkills, int job, string state, byte damageType, byte type, byte subType, byte element, byte superArmor, bool isSpRecovery)
            : this(id, skillLevels, state, damageType, type, subType, element, superArmor, isSpRecovery)
        {
            SubSkills = subSkills;
            Job = job;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SkillId, SkillLevels, Job, Learned);
        }

        public override string ToString() =>
            $"Skill:(Id:{SkillId},Job:{Job},SkillLevel:{string.Join(",", SkillLevels)}";
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
        public readonly SkillAttack SkillAttacks;
        [XmlElement(Order = 8)]
        public SkillAdditionalData SkillAdditionalData;

        // Required for deserialization
        public SkillLevel() { }

        public SkillLevel(int level, SkillAdditionalData data)
        {
            Level = level;
            SkillAdditionalData = data;
            SkillMotions = new SkillMotion();
        }

        public SkillLevel(int level, int spirit, int stamina, float damageRate, string feature, SkillMotion skillMotions, SkillAttack skillAttack)
        {
            Level = level;
            Spirit = spirit;
            Stamina = stamina;
            DamageRate = damageRate;
            Feature = feature;
            SkillMotions = skillMotions;
            SkillAttacks = skillAttack;
            SkillAdditionalData = new SkillAdditionalData();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Level, Spirit);
        }

        public override string ToString() =>
            $"SkillLevel(Level:{Level},Spirit:{Spirit},Stamina:{Stamina},DamageRate:{DamageRate},Feature:{Feature},SkillMotion:{SkillMotions})";
    }

    [XmlType] // TODO: More to implement from attack attribute.
    public class SkillAttack
    {
        [XmlElement(Order = 1)]
        public readonly List<int> ConditionSkillIds;

        public SkillAttack()
        {
            ConditionSkillIds = new List<int>();
        }

        public SkillAttack(List<int> conditionSkillsID)
        {
            ConditionSkillIds = conditionSkillsID;
        }

        public override string ToString() => $"Skills: {string.Join(",", ConditionSkillIds)}";
    }

    [XmlType]
    public class SkillMotion
    {
        [XmlElement(Order = 1)]
        public string SequenceName = "";
        [XmlElement(Order = 2)]
        public string MotionEffect = "";

        public SkillMotion()
        {

        }

        public SkillMotion(string sequenceName, string motionEffect)
        {
            SequenceName = sequenceName;
            MotionEffect = motionEffect;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SequenceName, MotionEffect);
        }

        public override string ToString() => $"SequenceName:{SequenceName},MotionEffect:{MotionEffect}";

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

        public SkillAdditionalData() { }

        public SkillAdditionalData(int duration, int buffType, int buffSubType, int buffCategory, int maxStack)
        {
            Duration = duration;
            BuffType = buffType;
            BuffSubType = buffSubType;
            BuffCategory = buffCategory;
            MaxStack = maxStack;
        }

        public override string ToString() => $"DurationTick: {Duration}; Type:{BuffType}; SubType:{BuffSubType};";
    }
}
