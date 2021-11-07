using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class FishingSpotMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public short MinMastery;
    [XmlElement(Order = 3)]
    public short MaxMastery;
    [XmlElement(Order = 4)]
    public List<string> LiquidType = new();
}
