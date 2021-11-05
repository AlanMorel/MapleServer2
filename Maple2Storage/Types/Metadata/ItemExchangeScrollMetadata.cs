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
    public List<ItemRequirementMetadata> ItemCost;

    public ItemExchangeScrollMetadata()
    {
        ItemCost = new();
    }

    public ItemExchangeScrollMetadata(int exchangeId, string type, int recipeId, byte recipeRarity, short recipeAmount, int rewardId, byte rewardRarity, short rewardAmount, int mesoCost,
        List<ItemRequirementMetadata> items)
    {
        ExchangeId = exchangeId;
        Type = type;
        RecipeId = recipeId;
        RecipeRarity = recipeRarity;
        RecipeAmount = recipeAmount;
        RewardId = rewardId;
        RewardRarity = rewardRarity;
        RewardAmount = rewardAmount;
        MesoCost = mesoCost;
        ItemCost = items;
    }

    public override string ToString()
    {
        return $"ItemExchangeScrollMetadata(ExchangeId:{ExchangeId},Type:{Type},RecipeId:{RecipeId},RecipeRarity:{RecipeRarity},RecipeAmount:{RecipeAmount},RewardId:{RewardId}," +
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

    public ItemRequirementMetadata() { }

    public ItemRequirementMetadata(int id, byte rarity, short amount)
    {
        Id = id;
        Rarity = rarity;
        Amount = amount;
    }

    public override string ToString()
    {
        return $"ItemRequirement(Id:{Id},Rarity:{Rarity},Amount:{Amount})";
    }
}
