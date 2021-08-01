using System.Collections.Generic;
using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class JobMetadata
    {
        [XmlElement(Order = 1)]
        public int JobId;
        [XmlElement(Order = 2)]
        public int StartMapId;
        [XmlElement(Order = 3)]
        public List<int> OpenTaxis = new List<int>();
        [XmlElement(Order = 4)]
        public List<int> OpenMaps = new List<int>();
        [XmlElement(Order = 5)]
        public List<TutorialItemMetadata> TutorialItems = new List<TutorialItemMetadata>();
        [XmlElement(Order = 6)]
        public List<JobSkillMetadata> Skills = new List<JobSkillMetadata>();
        [XmlElement(Order = 7)]
        public List<JobLearnedSkillsMetadata> LearnedSkills = new List<JobLearnedSkillsMetadata>();

        public JobMetadata() { }
    }

    [XmlType]
    public class TutorialItemMetadata
    {
        [XmlElement(Order = 1)]
        public int ItemId;
        [XmlElement(Order = 2)]
        public byte Rarity;
        [XmlElement(Order = 3)]
        public byte Amount;

        public TutorialItemMetadata() { }

        public TutorialItemMetadata(int itemId, byte rarity, byte amount)
        {
            ItemId = itemId;
            Rarity = rarity;
            Amount = amount;
        }
    }

    [XmlType]
    public class JobSkillMetadata
    {
        [XmlElement(Order = 1)]
        public int SkillId;
        [XmlElement(Order = 2)]
        public short SubJobCode;
        [XmlElement(Order = 3)]
        public byte MaxLevel;
        [XmlElement(Order = 4)]
        public List<int> SubSkillIds = new List<int>();

        public JobSkillMetadata() { }

        public JobSkillMetadata(int skillId, short subJobCode, byte maxLevel, List<int> subSkillIds)
        {
            SkillId = skillId;
            SubJobCode = subJobCode;
            MaxLevel = maxLevel;
            SubSkillIds = subSkillIds;
        }
    }

    [XmlType]
    public class JobLearnedSkillsMetadata
    {
        [XmlElement(Order = 1)]
        public int Level;
        [XmlElement(Order = 2)]
        public List<int> SkillIds = new List<int>();

        public JobLearnedSkillsMetadata() { }

        public JobLearnedSkillsMetadata(int level, List<int> skillIds)
        {
            Level = level;
            SkillIds = skillIds;
        }
    }
}
