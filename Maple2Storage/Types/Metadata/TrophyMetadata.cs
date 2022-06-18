using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class TrophyMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public string Name = "";
    [XmlElement(Order = 3)]
    public string[] Categories;
    [XmlElement(Order = 4)]
    public bool AccountWide;
    [XmlElement(Order = 5)]
    public List<TrophyGradeMetadata> Grades = new();
    [XmlElement(Order = 6)]
    public string ConditionType = "";
    [XmlElement(Order = 7)]
    public string ConditionCodes = "";

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
    public string ConditionTargets;
    [XmlElement(Order = 4)]
    public RewardType RewardType;
    [XmlElement(Order = 5)]
    public int RewardCode;
    [XmlElement(Order = 6)]
    public int RewardValue;
    [XmlElement(Order = 7)]
    public int RewardRank;
    [XmlElement(Order = 8)]
    public int RewardSubJobLevel;

    public override string ToString()
    {
        return $"TrophyGradeMetadata(Grade:{Grade},Condition:{Condition},RewardType:{RewardType},RewardCode:{RewardCode},RewardValue:{RewardValue},RewardSubJobLevel:{RewardSubJobLevel})";
    }
}
