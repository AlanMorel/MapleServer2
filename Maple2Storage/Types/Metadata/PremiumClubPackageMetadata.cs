using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class PremiumClubPackageMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public int VipPeriod;
    [XmlElement(Order = 3)]
    public int Price;
    [XmlElement(Order = 4)]
    public byte BuyLimit;
    [XmlElement(Order = 5)]
    public List<BonusItem> BonusItem = new();

    public override string ToString()
    {
        return $"PremiumClubPackage(Id:{Id},VipPeriod:{VipPeriod},Price:{Price},BuyLimit:{BuyLimit},BonusItem:{BonusItem}}}";
    }
}

[XmlType]
public class BonusItem
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public byte Rarity;
    [XmlElement(Order = 3)]
    public short Amount;

    public override string ToString()
    {
        return $"ItemRequirement(Id:{Id},Rarity:{Rarity},Amount:{Amount})";
    }
}
