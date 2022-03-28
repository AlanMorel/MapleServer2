using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class ItemOptionPickMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public List<ItemOptionPick> ItemOptions = new();
}

[XmlType]
public class ItemOptionPick
{
    [XmlElement(Order = 1)]
    public byte Rarity;
    [XmlElement(Order = 2)]
    public List<ConstantPick> Constants = new();
    [XmlElement(Order = 3)]
    public List<StaticPick> StaticValues = new();
    [XmlElement(Order = 4)]
    public List<StaticPick> StaticRates = new();
}

[XmlType]
public class ConstantPick
{
    [XmlElement(Order = 1)]
    public StatAttribute Stat;
    [XmlElement(Order = 2)]
    public int DeviationValue;
}

[XmlType]
public class StaticPick
{
    [XmlElement(Order = 1)]
    public StatAttribute Stat;
    [XmlElement(Order = 2)]
    public int DeviationValue;
}
