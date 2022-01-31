using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class JobMetadata
{
    [XmlElement(Order = 1)]
    public int JobId;
    [XmlElement(Order = 2)]
    public int StartMapId;
    [XmlElement(Order = 3)]
    public List<int> OpenTaxis = new();
    [XmlElement(Order = 4)]
    public List<int> OpenMaps = new();
    [XmlElement(Order = 5)]
    public List<TutorialItemMetadata> TutorialItems = new();
    [XmlElement(Order = 6)]
    public List<JobSkillMetadata> Skills = new();
    [XmlElement(Order = 7)]
    public List<JobLearnedSkillsMetadata> LearnedSkills = new();
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
    public List<int> SubSkillIds = new();
    [XmlElement(Order = 5)]
    public byte QuickSlotPriority;

    public JobSkillMetadata() { }

    public JobSkillMetadata(int skillId, short subJobCode, byte maxLevel, List<int> subSkillIds, byte quickSlotPriority)
    {
        SkillId = skillId;
        SubJobCode = subJobCode;
        MaxLevel = maxLevel;
        SubSkillIds = subSkillIds;
        QuickSlotPriority = quickSlotPriority;
    }
}

[XmlType]
public class JobLearnedSkillsMetadata
{
    [XmlElement(Order = 1)]
    public int Level;
    [XmlElement(Order = 2)]
    public List<int> SkillIds = new();
}
