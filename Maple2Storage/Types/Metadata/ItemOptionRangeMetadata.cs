using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class ItemOptionRangeMetadata
{
    [XmlElement(Order = 1)]
    public ItemOptionRangeType RangeType;
    [XmlElement(Order = 2)]
    public Dictionary<StatId, List<ParserStat>> Stats = new();
    [XmlElement(Order = 3)]
    public Dictionary<SpecialStatId, List<ParserSpecialStat>> SpecialStats = new();
}

[XmlType]
public class ParserStat
{
    [XmlElement(Order = 1)]
    public StatId Id;
    [XmlElement(Order = 2)]
    public int Flat;
    [XmlElement(Order = 3)]
    public float Percent;

    public ParserStat() { }

    public ParserStat(StatId type, int flat)
    {
        Id = type;
        Flat = flat;
        Percent = 0;
    }

    public ParserStat(StatId type, float percent)
    {
        Id = type;
        Flat = 0;
        Percent = percent;
    }

    public override string ToString()
    {
        return $"Id: {Id}, Flat: {Flat}, Percent: {Percent}";
    }
}

[XmlType]
public class ParserSpecialStat
{
    [XmlElement(Order = 1)]
    public SpecialStatId Id;
    [XmlElement(Order = 2)]
    public float Percent;
    [XmlElement(Order = 3)]
    public float Flat;

    public ParserSpecialStat() { }

    public ParserSpecialStat(SpecialStatId id, float percent, float flat)
    {
        Id = id;
        Percent = percent;
        Flat = flat;
    }

    public override string ToString()
    {
        return $"Id: {Id}, Flat: {Flat}, Percent: {Percent}";
    }
}
