using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class DefaultItemsMetadata
{
    [XmlElement(Order = 1)]
    public int JobCode;
    [XmlElement(Order = 2)]
    public List<DefaultItem> DefaultItems = new();
}

[XmlType]
public class DefaultItem
{
    [XmlElement(Order = 1)]
    public ItemSlot ItemSlot;
    [XmlElement(Order = 2)]
    public int ItemId;
}
