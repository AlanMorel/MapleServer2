using System.Collections.Generic;
using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class MeretMarketMetadata
    {
        [XmlElement(Order = 1)]
        public MeretMarketCategory Category;
        [XmlElement(Order = 2)]
        public int MarketItemId;
        [XmlElement(Order = 3)]
        public string ItemName = "";
        [XmlElement(Order = 4)]
        public int ItemId;
        [XmlElement(Order = 5)]
        public byte Rarity;
        [XmlElement(Order = 6)]
        public int Quantity;
        [XmlElement(Order = 7)]
        public int BonusQuantity;
        [XmlElement(Order = 8)]
        public MeretMarketItemFlag Flag;
        [XmlElement(Order = 9)]
        public MeretMarketCurrencyType TokenType;
        [XmlElement(Order = 10)]
        public long Price;
        [XmlElement(Order = 11)]
        public long SalePrice;
        [XmlElement(Order = 12)]
        public int Duration; // in days
        [XmlElement(Order = 13)]
        public long SellBeginTime;
        [XmlElement(Order = 14)]
        public long SellEndTime;
        [XmlElement(Order = 15)]
        public MeretMarketJobRequirement JobRequirement;
        [XmlElement(Order = 16)]
        public short MinLevelRequirement;
        [XmlElement(Order = 17)]
        public short MaxLevelRequirement;
        [XmlElement(Order = 18)]
        public int RequiredAchievementId;
        [XmlElement(Order = 19)]
        public int RequiredAchievementGrade;
        [XmlElement(Order = 20)]
        public bool PCCafe = false;
        [XmlElement(Order = 21)]
        public bool RestockUnavailable = false;
        [XmlElement(Order = 22)]
        public int ParentMarketItemId;
        [XmlElement(Order = 23)]
        public string PromoName = "";
        [XmlElement(Order = 24)]
        public string PromoImageName = "";
        [XmlElement(Order = 25)]
        public MeretMarketPromoFlag PromoFlag;
        [XmlElement(Order = 26)]
        public bool ShowSaleTime;
        [XmlElement(Order = 27)]
        public long PromoBannerBeginTime; // ?? rename?
        [XmlElement(Order = 28)]
        public long PromoBannerEndTime; // ?? rename?
        [XmlElement(Order = 29)]
        public List<MeretMarketMetadata> AdditionalQuantities = new List<MeretMarketMetadata>();

        public MeretMarketMetadata() { }

    }
}
