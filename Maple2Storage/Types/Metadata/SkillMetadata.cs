using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class SkillMetadata
    {
        [XmlElement(Order = 1)]
        public int SkillId;
        [XmlElement(Order = 2)]
        public List<SkillLevel> SkillLevels;
        [XmlElement(Order = 3)]
        public int[] SubSkills = Array.Empty<int>();
        [XmlElement(Order = 4)]
        public int Job;
        [XmlElement(Order = 5)]
        public byte Learned = 0;
        [XmlElement(Order = 6)]
        public string State;
        [XmlElement(Order = 7)]
        public byte Type;

        public SkillMetadata()
        {
            SkillLevels = new List<SkillLevel>();
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
        public int Level;
        [XmlElement(Order = 2)]
        public int Spirit;
        [XmlElement(Order = 3)]
        public float DamageRate;
        [XmlElement(Order = 4)]
        public string Feature = "";
        [XmlElement(Order = 5)]
        public SkillMotion SkillMotions;

        // Required for deserialization
        public SkillLevel()
        {

        }

        public SkillLevel(int level, int spirit, float damageRate, string feature, SkillMotion skillMotions)
        {
            Level = level;
            Spirit = spirit;
            DamageRate = damageRate;
            Feature = feature;
            SkillMotions = skillMotions;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Level, Spirit);
        }

        public override string ToString() =>
            $"SkillLevel(Level:{Level},Spirit:{Spirit},DamageRate:{DamageRate},Feature:{Feature},SkillMotion:{SkillMotions})";
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
