using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class ItemSocketScrollMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public int MinLevel;
    [XmlElement(Order = 3)]
    public int MaxLevel;
    [XmlElement(Order = 4)]
    public List<ItemType> ItemTypes;
    [XmlElement(Order = 5)]
    public int Rarity;
    [XmlElement(Order = 6)]
    public bool MakeUntradeable;

    public ItemSocketScrollMetadata() { }
}
