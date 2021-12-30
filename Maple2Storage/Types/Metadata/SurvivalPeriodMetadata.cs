using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class SurvivalPeriodMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public int RewardId;
    [XmlElement(Order = 3)]
    public DateTime StartTime;
    [XmlElement(Order = 4)]
    public DateTime EndTime;
    [XmlElement(Order = 5)]
    public int PassPrice;
}
