using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class BeautyMetadata
    {
        [XmlElement(Order = 1)]
        public int ShopId;
        [XmlElement(Order = 2)]
        public BeautyCategory BeautyCategory;
        [XmlElement(Order = 3)]
        public BeautyShopType BeautyType;
        [XmlElement(Order = 4)]
        public int VoucherId;
        [XmlElement(Order = 5)]
        public ShopCurrencyType TokenType;
        [XmlElement(Order = 6)]
        public int RequiredItemId;
        [XmlElement(Order = 7)]
        public int TokenCost;
        [XmlElement(Order = 8)]
        public int SpecialCost;
        [XmlElement(Order = 9)]
        public List<BeautyItem> Items { get; set; }

        // Required for deserialization
        public BeautyMetadata() { }

        public override string ToString() =>
    $"BeautyMetadata(ShopId:{ShopId},BeautyCategory:{BeautyCategory},BeautyType:{BeautyType},VoucherId:{VoucherId},TokenType:{TokenType},RequiredItemId:{RequiredItemId}," +
            $"TokenCost:{TokenCost},SpecialCost:{SpecialCost},Items:{Items})";

        protected bool Equals(BeautyMetadata other)
        {
            return ShopId == other.ShopId &&
                   BeautyCategory == other.BeautyCategory &&
                   BeautyType == other.BeautyType &&
                   VoucherId == other.VoucherId &&
                   TokenType == other.TokenType &&
                   RequiredItemId == other.RequiredItemId &&
                   TokenCost == other.TokenCost &&
                   SpecialCost == other.SpecialCost &&
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

            return Equals((BeautyMetadata) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ShopId, BeautyCategory, BeautyType);
        }

        public static bool operator ==(BeautyMetadata left, BeautyMetadata right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BeautyMetadata left, BeautyMetadata right)
        {
            return !Equals(left, right);
        }
    }

    [XmlType]
    public class BeautyItem
    {
        [XmlElement(Order = 1)]
        public int ItemId;
        [XmlElement(Order = 2)]
        public byte Gender;
        [XmlElement(Order = 3)]
        public ShopItemFlag Flag;
        [XmlElement(Order = 4)]
        public short RequiredLevel;
        [XmlElement(Order = 5)]
        public int RequiredAchievementId;
        [XmlElement(Order = 6)]
        public byte RequiredAchievementGrade;
        [XmlElement(Order = 7)]
        public ShopCurrencyType TokenType;
        [XmlElement(Order = 8)]
        public int RequiredItemId;
        [XmlElement(Order = 9)]
        public int TokenCost;

        public BeautyItem() { }

        public override string ToString() =>
    $"BeautyItem(ItemId:{ItemId},Gender:Flag:{Flag},Gender:{Gender},RequiredLevel:{RequiredLevel},RequiredAchievementId:{RequiredAchievementId},RequiredAchievementGrade:{RequiredAchievementGrade}," +
            $"TokenType:{TokenType},RequiredItemId:{RequiredItemId},TokenCost:{TokenCost})";

        private bool Equals(BeautyItem other)
        {
            return ItemId == other.ItemId &&
                   Flag == other.Flag &&
                   Gender == other.Gender &&
                   RequiredLevel == other.RequiredLevel &&
                   RequiredItemId == other.RequiredItemId &&
                   RequiredAchievementId == other.RequiredAchievementId &&
                   RequiredAchievementGrade == other.RequiredAchievementGrade &&
                   TokenType == other.TokenType &&
                   RequiredItemId == other.RequiredItemId &&
                   TokenCost == other.TokenCost;
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

            return Equals((BeautyItem) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                ItemId,
                Flag,
                RequiredLevel,
                RequiredAchievementId,
                RequiredAchievementGrade,
                TokenType,
                RequiredItemId,
                TokenCost);
        }

        public static bool operator ==(BeautyItem left, BeautyItem right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BeautyItem left, BeautyItem right)
        {
            return !Equals(left, right);
        }
    }
}
