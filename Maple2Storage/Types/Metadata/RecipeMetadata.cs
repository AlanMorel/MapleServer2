using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class RecipeMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public short MasteryType;
    [XmlElement(Order = 3)]
    public bool ExceptRewardExp;
    [XmlElement(Order = 4)]
    public long RequireMastery;
    [XmlElement(Order = 5)]
    public long RequireMeso;
    [XmlElement(Order = 6)]
    public List<int> RequireQuest = new();
    [XmlElement(Order = 7)]
    public long RewardExp;
    [XmlElement(Order = 8)]
    public long RewardMastery;
    [XmlElement(Order = 9)]
    public string GatheringTime;
    [XmlElement(Order = 10)]
    public int HighPropLimitCount;
    [XmlElement(Order = 11)]
    public int NormalPropLimitCount;
    [XmlElement(Order = 12)]
    public List<RecipeItem> RequiredItems = new();
    [XmlElement(Order = 17)]
    public int HabitatMapId;
    [XmlElement(Order = 18)]
    public List<RecipeItem> RewardItems = new();
}

[XmlType]
public class RecipeItem
{
    [XmlElement(Order = 1)]
    public int ItemId;
    [XmlElement(Order = 2)]
    public int Amount;
    [XmlElement(Order = 3)]
    public int Rarity;
}
