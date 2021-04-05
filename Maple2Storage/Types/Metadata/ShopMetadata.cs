using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class ShopMetadata
    {
        [XmlElement(Order = 1)]
        public int TemplateId { get; set; }
        [XmlElement(Order = 2)]
        public int Id { get; set; }
        [XmlElement(Order = 3)]
        public int Category { get; set; }
        [XmlElement(Order = 4)]
        public string Name { get; set; }
        [XmlElement(Order = 5)]
        public ShopType ShopType { get; set; }
        [XmlElement(Order = 6)]
        public bool RestrictSales { get; set; }
        [XmlElement(Order = 7)]
        public bool CanRestock { get; set; }
        [XmlElement(Order = 8)]
        public long NextRestock { get; set; }
        [XmlElement(Order = 9)]
        public bool AllowBuyback { get; set; }
        [XmlElement(Order = 10)]
        public List<ShopItem> Items { get; set; }

        // Required for deserialization
        public ShopMetadata() { }

        public override string ToString() =>
            $"ShopMetadata(TemplateId:{TemplateId},Id:{Id},Category:{Category},Name:{Name},ShopType{ShopType},RestrictSales:{RestrictSales},CanRestock:{CanRestock},NextRestock:{NextRestock},AllowBuyback:{AllowBuyback},Items:{Items})";

        protected bool Equals(ShopMetadata other)
        {
            return TemplateId == other.TemplateId &&
                   Id == other.Id &&
                   Category == other.Category &&
                   Name == other.Name &&
                   ShopType == other.ShopType &&
                   RestrictSales == other.RestrictSales &&
                   CanRestock == other.CanRestock &&
                   NextRestock == other.NextRestock &&
                   AllowBuyback == other.AllowBuyback &&
                   Items.SequenceEqual(other.Items);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((ShopMetadata) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TemplateId, Id, Category);
        }

        public static bool operator ==(ShopMetadata left, ShopMetadata right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ShopMetadata left, ShopMetadata right)
        {
            return !Equals(left, right);
        }
    }

    [XmlType]
    public class ShopItem
    {
        [XmlElement(Order = 1)]
        public int UniqueId;
        [XmlElement(Order = 2)]
        public int ItemId;
        [XmlElement(Order = 3)]
        public ShopCurrencyType TokenType;
        [XmlElement(Order = 4)]
        public int RequiredItemId;
        [XmlElement(Order = 5)]
        public int Price;
        [XmlElement(Order = 6)]
        public int SalePrice;
        [XmlElement(Order = 7)]
        public byte ItemRank;
        [XmlElement(Order = 8)]
        public int StockCount;
        [XmlElement(Order = 9)]
        public int StockPurchased;
        [XmlElement(Order = 10)]
        public int GuildTrophy;
        [XmlElement(Order = 11)]
        public string Category;
        [XmlElement(Order = 12)]
        public int RequiredAchievementId;
        [XmlElement(Order = 13)]
        public int RequiredAchievementGrade;
        [XmlElement(Order = 14)]
        public byte RequiredChampionshipGrade;
        [XmlElement(Order = 15)]
        public short RequiredChampionshipJoinCount;
        [XmlElement(Order = 16)]
        public byte RequiredGuildMerchantType;
        [XmlElement(Order = 17)]
        public short RequiredGuildMerchantLevel;
        [XmlElement(Order = 18)]
        public short Quantity;
        [XmlElement(Order = 19)]
        public ShopItemFlag Flag;
        [XmlElement(Order = 20)]
        public string TokenName = "";
        [XmlElement(Order = 21)]
        public short RequiredQuestAlliance;
        [XmlElement(Order = 22)]
        public int RequiredFameGrade;
        [XmlElement(Order = 23)]
        public bool AutoPreviewEquip;

        // Required for deserialization
        public ShopItem() { }

        public override string ToString() =>
            $"ShopItem(UniqueId:{UniqueId},ItemId:{ItemId},TokenType:{TokenType},RequiredItemId:{RequiredItemId},Price:{Price},SalePrice:{SalePrice},ItemRank:{ItemRank},StockCount:{StockCount},StockPurchased:{StockPurchased},GuildTrophy:{GuildTrophy},Category:{Category},RequiredAchievementId:{RequiredAchievementId},RequiredAchievementGrade:{RequiredAchievementGrade},RequiredChampionshipGrade:{RequiredChampionshipGrade},RequiredChampionshipJoinCount:{RequiredChampionshipJoinCount},RequiredGuildMerchantType:{RequiredGuildMerchantType},RequiredGuildMerchantLevel:{RequiredGuildMerchantLevel},Quantity:{Quantity},Flag:{Flag},TokenName:{TokenName},RequiredQuestAlliance:{RequiredQuestAlliance},RequiredFameGrade:{RequiredFameGrade},AutoPreviewEquip{AutoPreviewEquip})";

        private bool Equals(ShopItem other)
        {
            return UniqueId == other.UniqueId &&
                   ItemId == other.ItemId &&
                   TokenType == other.TokenType &&
                   RequiredItemId == other.RequiredItemId &&
                   Price == other.Price &&
                   SalePrice == other.SalePrice &&
                   ItemRank == other.ItemRank &&
                   StockCount == other.StockCount &&
                   StockPurchased == other.StockPurchased &&
                   GuildTrophy == other.GuildTrophy &&
                   Category == other.Category &&
                   RequiredAchievementId == other.RequiredAchievementId &&
                   RequiredAchievementGrade == other.RequiredAchievementGrade &&
                   RequiredChampionshipGrade == other.RequiredChampionshipGrade &&
                   RequiredChampionshipJoinCount == other.RequiredChampionshipJoinCount &&
                   RequiredGuildMerchantType == other.RequiredGuildMerchantType &&
                   RequiredGuildMerchantLevel == other.RequiredGuildMerchantLevel &&
                   Quantity == other.Quantity &&
                   Flag == other.Flag &&
                   TokenName == other.TokenName &&
                   RequiredQuestAlliance == other.RequiredQuestAlliance &&
                   RequiredFameGrade == other.RequiredFameGrade &&
                   AutoPreviewEquip == other.AutoPreviewEquip;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((ShopItem) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                UniqueId,
                ItemId,
                RequiredItemId,
                Price,
                SalePrice,
                StockCount,
                StockPurchased,
                GuildTrophy);
        }

        public static bool operator ==(ShopItem left, ShopItem right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ShopItem left, ShopItem right)
        {
            return !Equals(left, right);
        }
    }
}
