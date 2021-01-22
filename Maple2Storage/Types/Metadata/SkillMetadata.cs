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
        public int[] SubSkills = new int[0];
        [XmlElement(Order = 4)]
        public int Job;
        [XmlElement(Order = 5)]
        public byte Learned = 0;

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

        // Required for deserialization
        public SkillLevel()
        {

        }

        public SkillLevel(int level, int spirit, float damageRate, string feature)
        {
            Level = level;
            Spirit = spirit;
            DamageRate = damageRate;
            Feature = feature;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Level, Spirit);
        }

        public override string ToString() =>
            $"SkillLevel(Level:{Level},Spirit:{Spirit},DamageRate:{DamageRate},Feature:{Feature})";
    }

    [XmlType]
    public class ListSubSkill
    {
        [XmlElement(Order = 1)]
        public int Id { get; set; }
        [XmlElement(Order = 2)]
        public int[] Subs { get; set; }
        [XmlElement(Order = 3)]
        public int[] DefaultSkillLearns { get; set; }

        public ListSubSkill()
        {

        }

        public ListSubSkill(int id, int[] sub, int[] defaultSkillLearn)
        {
            Id = id;
            Subs = sub;
            DefaultSkillLearns = defaultSkillLearn;
        }

        public override string ToString() => $"Skill(Id:{Id},Sub:{string.Join(",", Subs.ToString())},Default:{string.Join(",", DefaultSkillLearns.ToString())})";
    }
}
