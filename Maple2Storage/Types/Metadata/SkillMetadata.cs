﻿using System;
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
        public SkillLevel SkillLevel;
        [XmlElement(Order = 3)]
        public int[] SubSkill = new int[0];
        [XmlElement(Order = 4)]
        public int Job;
        [XmlElement(Order = 5)]
        public byte Learned = 0;

        public SkillMetadata()
        {
            SkillLevel = new SkillLevel();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SkillId, SkillLevel, Job, Learned);
        }

        public override string ToString() =>
            $"Skill:(Id:{SkillId},Job:{Job},SkillLevel:{string.Join(",", SkillLevel.ToString())},Learned:{Learned}";
    }

    [XmlType]
    public class SkillLevel
    {
        [XmlElement(Order = 1)]
        public int Level;
        [XmlElement(Order = 2)]
        public int Spirit;
        [XmlElement(Order = 3)]
        public int UpgradeLevel;
        [XmlElement(Order = 4)]
        public int[] UpgradeSkillId;
        [XmlElement(Order = 5)]
        public int[] UpgradeSkillLevel;

        // Required for deserialization
        public SkillLevel()
        {

        }

        public SkillLevel(int _Level, int _Spirit, int _UpgradeLevel, int[] _UpgradeSkillId, int[] _UpgradeSkillLevel)
        {
            Level = _Level;
            Spirit = _Spirit;
            UpgradeLevel = _UpgradeLevel;
            UpgradeSkillId = _UpgradeSkillId;
            UpgradeSkillLevel = _UpgradeSkillLevel;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Level, Spirit, UpgradeLevel, UpgradeSkillId, UpgradeSkillLevel);
        }

        public override string ToString() =>
            $"SkillLevel(Level:{Level},Spirit:{Spirit},UpgradeLevel:{UpgradeLevel},SkillRequired:{UpgradeSkillId},SkillLevelRequired:{UpgradeSkillLevel})";
    }

    [XmlType]
    public class ListSubSkill
    {
        [XmlElement(Order = 1)]
        public int Id { get; set; }
        [XmlElement(Order = 2)]
        public int[] Sub { get; set; }
        [XmlElement(Order = 3)]
        public int[] DefaultSkillLearn { get; set; }

        public ListSubSkill()
        {

        }

        public ListSubSkill(int id, int[] sub, int[] defaultSkillLearn)
        {
            Id = id;
            Sub = sub;
            DefaultSkillLearn = defaultSkillLearn;
        }

        public override string ToString() => $"Skill(Id:{Id},Sub:{string.Join(",", Sub.ToString())},Default:{string.Join(",", DefaultSkillLearn.ToString())})";
    }
}
