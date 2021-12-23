using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class FishingRodMetadata
{
    [XmlElement(Order = 1)]
    public int RodId;
    [XmlElement(Order = 2)]
    public int ItemId;
    [XmlElement(Order = 3)]
    public short MasteryLimit;
    [XmlElement(Order = 4)]
    public int ReduceTime;
}
