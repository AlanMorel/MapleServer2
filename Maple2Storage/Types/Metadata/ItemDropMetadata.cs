using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class ItemDropMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public List<DropGroup> DropGroups = new();
}

[XmlType]
public class DropGroup
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public List<DropGroupContent> Contents = new();
}

[XmlType]
public class DropGroupContent
{
    [XmlElement(Order = 1)]
    public List<int> ItemIds = new();
    [XmlElement(Order = 2)]
    public int SmartDropRate;
    [XmlElement(Order = 3)]
    public bool SmartGender;
    [XmlElement(Order = 4)]
    public byte EnchantLevel;
    [XmlElement(Order = 5)]
    public float MinAmount;
    [XmlElement(Order = 6)]
    public float MaxAmount;
    [XmlElement(Order = 7)]
    public byte Rarity;
}
