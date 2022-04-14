using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class PrestigeLevelMissionMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public int MissionCount;
    [XmlElement(Order = 3)]
    public int RewardItemId;
    [XmlElement(Order = 4)]
    public int RewardItemRarity;
    [XmlElement(Order = 5)]
    public int RewardItemAmount;

    public PrestigeLevelMissionMetadata() { }
}
