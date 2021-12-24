using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class ItemOptionRandomMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public List<ItemOptionRandom> ItemOptions = new();
}

[XmlType]
public class ItemOptionRandom
{
    [XmlElement(Order = 1)]
    public byte Rarity;
    [XmlElement(Order = 2)]
    public float MultiplyFactor;
    [XmlElement(Order = 3)]
    public byte[] Slots = Array.Empty<byte>();
    [XmlElement(Order = 4)]
    public List<ParserStat> Stats = new();
    [XmlElement(Order = 5)]
    public List<ParserSpecialStat> SpecialStats = new();
}
