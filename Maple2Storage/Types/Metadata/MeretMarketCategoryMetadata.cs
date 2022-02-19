using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class MeretMarketCategoryMetadata
{
    [XmlElement(Order = 1)]
    public MeretMarketSection Section;
    [XmlElement(Order = 2)]
    public List<MeretMarketTab> Tabs = new();
}

[XmlType]
public class MeretMarketTab
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public List<string> ItemCategories = new();
}
