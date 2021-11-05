using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class DungeonMetadata
{
    [XmlElement(Order = 1)]
    public int DungeonRoomId;
    [XmlElement(Order = 2)]
    public int DungeonLevelRequirement;
    [XmlElement(Order = 3)]
    public string GroupType;
    [XmlElement(Order = 4)]
    public string CooldownType;
    [XmlElement(Order = 5)]
    public int UnionRewardId;
    [XmlElement(Order = 6)]
    public short RewardCount;
    [XmlElement(Order = 7)]
    public int RewardExp;
    [XmlElement(Order = 8)]
    public int RewardMeso;
    [XmlElement(Order = 9)]
    public int LobbyFieldId;
    [XmlElement(Order = 10)]
    public List<int> FieldIds = new();
    [XmlElement(Order = 11)]
    public byte MaxUserCount;
    [XmlElement(Order = 12)]
    public byte LimitPlayerLevel;
}
