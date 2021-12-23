using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class ItemOptionsConstantMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public byte Rarity;
    [XmlElement(Order = 3)]
    public List<ParserStat> Stats = new();
    [XmlElement(Order = 4)]
    public List<ParserSpecialStat> SpecialStats = new();
}
