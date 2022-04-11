using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class ItemSocketMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public List<ItemSocketRarityData> RarityData = new();

    public ItemSocketMetadata() { }
}

[XmlType]
public class ItemSocketRarityData
{
    [XmlElement(Order = 1)]
    public int Rarity;
    [XmlElement(Order = 2)]
    public int MaxCount;
    [XmlElement(Order = 3)]
    public int FixedOpenCount;

    public ItemSocketRarityData() { }
}
