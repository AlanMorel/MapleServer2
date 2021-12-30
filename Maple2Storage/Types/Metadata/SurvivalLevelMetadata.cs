using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class SurvivalLevelMetadata
{
    [XmlElement(Order = 1)]
    public int Level;
    [XmlElement(Order = 2)]
    public long RequiredExp;
    [XmlElement(Order = 3)]
    public byte Grade;
}
