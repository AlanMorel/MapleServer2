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
        public readonly byte Type;
        [XmlElement(Order = 8)]
        public readonly bool IsSpRecovery;

        public SkillMetadata()
        {
            SkillLevels = new List<SkillLevel>();
        }
        public SkillMetadata(int id, List<SkillLevel> skillLevels, string state, byte type, bool isSpRecovery)
        {
            SkillId = id;
            SkillLevels = skillLevels;
            State = state;
            Type = type;
            IsSpRecovery = isSpRecovery;
        }

        public SkillMetadata(int id, List<SkillLevel> skillLevels, int[] subSkills, int job, string state, byte type, bool isSpRecovery)
        {
            SkillId = id;
            SkillLevels = skillLevels;
            SubSkills = subSkills;
            Job = job;
            State = state;
            Type = type;
            IsSpRecovery = isSpRecovery;
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
}
