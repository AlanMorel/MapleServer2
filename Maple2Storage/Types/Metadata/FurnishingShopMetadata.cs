using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class FurnishingShopMetadata
{
    [XmlElement(Order = 1)]
    public int ItemId;
    [XmlElement(Order = 2)]
    public bool Buyable;
    [XmlElement(Order = 3)]
    public byte FurnishingTokenType;
    [XmlElement(Order = 4)]
    public int Price;

    public override string ToString()
    {
        return $"FurnishingShopMetadata(ItemId:{ItemId},Buyable:{Buyable},FurnishingTokenType:{FurnishingTokenType},Price:{Price})";
    }
}
