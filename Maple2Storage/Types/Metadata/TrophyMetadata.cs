using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class TrophyMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public string[] Categories;
    [XmlElement(Order = 3)]
    public bool AccountWide;
    [XmlElement(Order = 4)]
    public List<TrophyGradeMetadata> Grades = new();

    public override string ToString()
    {
        return $"TrophyMetadata(Id:{Id},Categories:{string.Join(",", Categories)},Grades:{string.Join(",", Grades)}";
    }
}

[XmlType]
public class TrophyGradeMetadata
{
    [XmlElement(Order = 1)]
    public int Grade;
    [XmlElement(Order = 2)]
    public long Condition;
    [XmlElement(Order = 3)]
    public string ConditionType;
    [XmlElement(Order = 4)]
    public string ConditionCodes;
    [XmlElement(Order = 5)]
    public string ConditionTargets;
    [XmlElement(Order = 6)]
    public RewardType RewardType;
    [XmlElement(Order = 7)]
    public int RewardCode;
    [XmlElement(Order = 8)]
    public int RewardValue;

    public override string ToString()
    {
        return $"TrophyGradeMetadata(Grade:{Grade},Condition:{Condition},RewardType:{RewardType},RewardCode:{RewardCode},RewardValue:{RewardValue})";
    }
}
