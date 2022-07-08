using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class ItemExchangeScrollMetadata
{
    [XmlElement(Order = 1)]
    public int ExchangeId;
    [XmlElement(Order = 2)]
    public string Type;
    [XmlElement(Order = 3)]
    public int RecipeId;
    [XmlElement(Order = 4)]
    public byte RecipeRarity;
    [XmlElement(Order = 5)]
    public short RecipeAmount;
    [XmlElement(Order = 6)]
    public int RewardId;
    [XmlElement(Order = 7)]
    public byte RewardRarity;
    [XmlElement(Order = 8)]
    public short RewardAmount;
    [XmlElement(Order = 9)]
    public int MesoCost;
    [XmlElement(Order = 10)]
    public List<ItemRequirementMetadata> ItemCost = new();

    public override string ToString()
    {
        return
            $"ItemExchangeScrollMetadata(ExchangeId:{ExchangeId},Type:{Type},RecipeId:{RecipeId},RecipeRarity:{RecipeRarity},RecipeAmount:{RecipeAmount},RewardId:{RewardId}," +
            $"RewardRarity:{RewardRarity},RewardAmount:{RewardAmount},MesoCost:{MesoCost},Content:{string.Join(",", ItemCost)}";
    }
}

[XmlType]
public class ItemRequirementMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public byte Rarity;
    [XmlElement(Order = 3)]
    public short Amount;
    [XmlElement(Order = 4)]
    public string StringTag = "";

    public override string ToString()
    {
        return $"ItemRequirement(Id:{Id},Rarity:{Rarity},Amount:{Amount})";
    }
}
