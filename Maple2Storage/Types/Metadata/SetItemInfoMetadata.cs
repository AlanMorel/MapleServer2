using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class SetItemInfoMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public int[] ItemIds;
    [XmlElement(Order = 3)]
    public int OptionId;
}
