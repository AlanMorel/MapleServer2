using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class InsigniaMetadata
{
    [XmlElement(Order = 1)]
    public short InsigniaId;
    [XmlElement(Order = 2)]
    public string ConditionType;
    [XmlElement(Order = 3)]
    public int TitleId;
}
