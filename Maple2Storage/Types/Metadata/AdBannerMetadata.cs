using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class AdBannerMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public int MapId;
    [XmlElement(Order = 3)]
    public List<int> Prices;

    public AdBannerMetadata() { }
}
