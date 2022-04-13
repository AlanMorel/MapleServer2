using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class EnchantScrollMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public EnchantScrollType ScrollType;
    [XmlElement(Order = 3)]
    public short MinLevel;
    [XmlElement(Order = 4)]
    public short MaxLevel;
    [XmlElement(Order = 5)]
    public List<int> EnchantLevels;
    [XmlElement(Order = 6)]
    public List<ItemType> ItemTypes;
    [XmlElement(Order = 7)]
    public List<int> Rarities;

    public EnchantScrollMetadata() { }
}
