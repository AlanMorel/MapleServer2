using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class MeretMarketCategoryMetadata
{
    [XmlElement(Order = 1)]
    public int CategoryId;
    [XmlElement(Order = 2)]
    public MeretMarketSection Section;
    [XmlElement(Order = 3)]
    public List<string> ItemCategories = new();
}
