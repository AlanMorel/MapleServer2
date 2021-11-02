using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
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
        public List<BonusItem> BonusItem;

        public PremiumClubPackageMetadata()
        {
            BonusItem = new List<BonusItem>();
        }

        public PremiumClubPackageMetadata(int id, int vipPeriod, int price, byte buyLimit, List<BonusItem> bonusItems)
        {
            Id = id;
            VipPeriod = vipPeriod;
            Price = price;
            BuyLimit = buyLimit;
            BonusItem = bonusItems;
        }

        public override string ToString() => $"PremiumClubPackage(Id:{Id},VipPeriod:{VipPeriod},Price:{Price},BuyLimit:{BuyLimit},BonusItem:{BonusItem}}}";
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

        public BonusItem() { }

        public BonusItem(int id, byte rarity, short amount)
        {
            Id = id;
            Rarity = rarity;
            Amount = amount;
        }

        public override string ToString() => $"ItemRequirement(Id:{Id},Rarity:{Rarity},Amount:{Amount})";
    }
}
