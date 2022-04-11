using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class EnchantLimitMetadata
{
    [XmlElement(Order = 1)]
    public ItemType ItemType;
    [XmlElement(Order = 2)]
    public int MinLevel;
    [XmlElement(Order = 3)]
    public int MaxLevel;
    [XmlElement(Order = 4)]
    public int MaxEnchantLevel;

    public EnchantLimitMetadata() { }
}
