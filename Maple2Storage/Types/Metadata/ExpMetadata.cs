using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class ExpMetadata
{
    [XmlElement(Order = 1)]
    public short Level;
    [XmlElement(Order = 2)]
    public long Experience;
}
