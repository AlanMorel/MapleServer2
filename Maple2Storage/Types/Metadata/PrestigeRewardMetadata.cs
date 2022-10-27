using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class PrestigeRewardMetadata
{
    [XmlElement(Order = 1)]
    public readonly int Level;
    [XmlElement(Order = 2)]
    public readonly string Type;
    [XmlElement(Order = 3)]
    public readonly int Id;
    [XmlElement(Order = 4)]
    public readonly int Rarity;
    [XmlElement(Order = 5)]
    public readonly int Amount;
    
    public PrestigeRewardMetadata(){}

    public PrestigeRewardMetadata(int level, string type, int id, int rarity, int amount)
    {
        Level = level;
        Type = type;
        Id = id;
        Rarity = rarity;
        Amount = amount;
    }
}
