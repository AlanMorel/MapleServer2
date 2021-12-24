using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class BlackMarketTableMetadata
{
    [XmlElement(Order = 1)]
    public int CategoryId;
    [XmlElement(Order = 2)]
    public List<string> ItemCategories = new();
}
