using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class MasteryMetadata
{
    [XmlElement(Order = 1)]
    public int Type;
    [XmlElement(Order = 2)]
    public List<MasteryGrade> Grades = new();

    public override string ToString()
    {
        return $"MasteryMetadata(Type:{Type},Grades:{string.Join(",", Grades)})";
    }
}

[XmlType]
public class MasteryGrade
{
    [XmlElement(Order = 1)]
    public int Grade;
    [XmlElement(Order = 2)]
    public long Value;
    [XmlElement(Order = 3)]
    public int RewardJobItemID;
    [XmlElement(Order = 4)]
    public int RewardJobItemRank;
    [XmlElement(Order = 5)]
    public int RewardJobItemCount;
    [XmlElement(Order = 6)]
    public string Feature;

    public override string ToString()
    {
        return $"MasteryGradeMetadata(Grade:{Grade},Value:{Value},RewardJobItemID:{RewardJobItemID},RewardJobItemRank:{RewardJobItemRank}," +
               $"RewardJobItemCount:{RewardJobItemCount},Feature:{Feature})";
    }
}
