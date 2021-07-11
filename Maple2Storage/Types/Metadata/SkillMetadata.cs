using System;
using System.Collections.Generic;
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
        [XmlElement(Order = 13)]
        public readonly bool IsBuff;

        public SkillMetadata()
        {
            SkillLevels = new List<SkillLevel>();
        }
        public SkillMetadata(int id, List<SkillLevel> skillLevels, string state, byte damageType, byte type, byte subType, byte element, byte superArmor, bool isSpRecovery, bool isBuff)
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
            IsBuff = isBuff;
        }

        public SkillMetadata(int id, List<SkillLevel> skillLevels, int[] subSkills, int job, string state, byte damageType, byte type, byte subType, byte element, byte superArmor, bool isSpRecovery, bool isBuff)
            : this(id, skillLevels, state, damageType, type, subType, element, superArmor, isSpRecovery, isBuff)
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
        public SkillAdditionalData SkillAdditionalData;

        // Required for deserialization
        public SkillLevel() { }

        public SkillLevel(int level, int spirit, int stamina, float damageRate, string feature, SkillMotion skillMotions)
        {
            Level = level;
            Spirit = spirit;
            Stamina = stamina;
            DamageRate = damageRate;
            Feature = feature;
            SkillMotions = skillMotions;
            SkillAdditionalData = new SkillAdditionalData();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Level, Spirit);
        }

        public override string ToString() =>
            $"SkillLevel(Level:{Level},Spirit:{Spirit},Stamina:{Stamina},DamageRate:{DamageRate},Feature:{Feature},SkillMotion:{SkillMotions})";
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
        public int Duration;

        public SkillAdditionalData() { }

        public SkillAdditionalData(int duration)
        {
            Duration = duration;
        }

        public override string ToString() => $"DurationTick: {Duration}";
    }
}
