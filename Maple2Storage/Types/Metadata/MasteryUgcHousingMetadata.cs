using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class MasteryUgcHousingMetadata
{
    [XmlElement(Order = 1)]
    public byte Grade;
    [XmlElement(Order = 2)]
    public short MasteryRequired;
    [XmlElement(Order = 3)]
    public int ItemId;
}
