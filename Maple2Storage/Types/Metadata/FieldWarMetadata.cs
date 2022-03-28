using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class FieldWarMetadata
{
    [XmlElement(Order = 1)]
    public int FieldWarId;
    [XmlElement(Order = 2)]
    public int RewardId;
    [XmlElement(Order = 3)]
    public int MapId;
    [XmlElement(Order = 4)]
    public byte GroupId;
}
