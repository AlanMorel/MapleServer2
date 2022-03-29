using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class ItemOptionRangeMetadata
{
    [XmlElement(Order = 1)]
    public ItemOptionRangeType RangeType;
    [XmlElement(Order = 2)]
    public Dictionary<StatAttribute, List<ParserStat>> Stats = new();
    [XmlElement(Order = 3)]
    public Dictionary<StatAttribute, List<ParserSpecialStat>> SpecialStats = new();
}

[XmlType]
public class ParserStat
{
    [XmlElement(Order = 1)]
    public StatAttribute Attribute;
    [XmlElement(Order = 2)]
    public float Value;
    [XmlElement(Order = 3)]
    public StatAttributeType AttributeType;

    public ParserStat() { }

    public ParserStat(StatAttribute attribute, float value, StatAttributeType type)
    {
        Attribute = attribute;
        Value = value;
        AttributeType = type;
    }

    public override string ToString()
    {
        return $"Id: {Attribute}, Value: {Value}";
    }
}

[XmlType]
public class ParserSpecialStat
{
    [XmlElement(Order = 1)]
    public StatAttribute Attribute;
    [XmlElement(Order = 2)]
    public float Value;
    [XmlElement(Order = 3)]
    public StatAttributeType AttributeType;

    public ParserSpecialStat() { }

    public ParserSpecialStat(StatAttribute attribute, float value, StatAttributeType type)
    {
        Attribute = attribute;
        Value = value;
        AttributeType = type;
    }

    public override string ToString()
    {
        return $"Id: {Attribute}, Value: {Value}";
    }
}
