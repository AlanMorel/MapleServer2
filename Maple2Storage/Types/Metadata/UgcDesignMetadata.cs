using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class UgcDesignMetadata
{
    [XmlElement(Order = 1)]
    public int ItemId;
    [XmlElement(Order = 2)]
    public bool Visible;
    [XmlElement(Order = 3)]
    public byte Rarity;
    [XmlElement(Order = 4)]
    public CurrencyType CurrencyType;
    [XmlElement(Order = 5)]
    public long Price;
    [XmlElement(Order = 6)]
    public long SalePrice;
    [XmlElement(Order = 7)]
    public long MarketMinPrice;
    [XmlElement(Order = 8)]
    public long MarketMaxPrice;

    public UgcDesignMetadata() { }

    public UgcDesignMetadata(int itemId, bool visible, byte rarity, CurrencyType currencyType, long price, long salePrice, long marketMinPrice,
        long marketMaxPrice)
    {
        ItemId = itemId;
        Visible = visible;
        Rarity = rarity;
        CurrencyType = currencyType;
        Price = price;
        SalePrice = salePrice;
        MarketMinPrice = marketMinPrice;
        MarketMaxPrice = marketMaxPrice;
    }
}
