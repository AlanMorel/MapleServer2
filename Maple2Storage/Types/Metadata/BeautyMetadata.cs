using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class BeautyMetadata
{
    [XmlElement(Order = 1)]
    public int ShopId;
    [XmlElement(Order = 2)]
    public int UniqueId; // This needs to be correct to enable voucher use
    [XmlElement(Order = 3)]
    public BeautyCategory BeautyCategory;
    [XmlElement(Order = 4)]
    public BeautyShopType BeautyType;
    [XmlElement(Order = 5)]
    public int VoucherId;
    [XmlElement(Order = 6)]
    public ShopCurrencyType TokenType;
    [XmlElement(Order = 7)]
    public int RequiredItemId;
    [XmlElement(Order = 8)]
    public int TokenCost;
    [XmlElement(Order = 9)]
    public int SpecialCost;
    [XmlElement(Order = 10)]
    public List<BeautyItem> Items { get; set; }

    // Required for deserialization
    public BeautyMetadata() { }

    public override string ToString()
    {
        return $"BeautyMetadata(ShopId:{ShopId},UniqueId:{UniqueId},BeautyCategory:{BeautyCategory},BeautyType:{BeautyType},VoucherId:{VoucherId}," +
            $"TokenType:{TokenType},RequiredItemId:{RequiredItemId},TokenCost:{TokenCost},SpecialCost:{SpecialCost},Items: {Items})";
    }
}
[XmlType]
public class BeautyItem
{
    [XmlElement(Order = 1)]
    public int ItemId;
    [XmlElement(Order = 2)]
    public Gender Gender;
    [XmlElement(Order = 3)]
    public ShopItemFlag Flag = ShopItemFlag.None;
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

    public override string ToString()
    {
        return $"BeautyItem(ItemId:{ItemId},Gender:Flag:{Flag},Gender:{Gender},RequiredLevel:{RequiredLevel},RequiredAchievementId:{RequiredAchievementId},RequiredAchievementGrade:{RequiredAchievementGrade}," +
            $"TokenType:{TokenType},RequiredItemId:{RequiredItemId},TokenCost:{TokenCost})";
    }
}
