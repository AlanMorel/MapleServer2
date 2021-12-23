using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class GachaMetadata
{
    [XmlElement(Order = 1)]
    public int GachaId;
    [XmlElement(Order = 2)]
    public byte BoxGroup;
    [XmlElement(Order = 3)]
    public int DropBoxId;
    [XmlElement(Order = 4)]
    public int ShopId;
    [XmlElement(Order = 5)]
    public int CoinId;
    [XmlElement(Order = 6)]
    public byte CoinAmount;
    [XmlElement(Order = 7)]
    public List<GachaContent> Contents;

    public override string ToString()
    {
        return $"GachaMetadata:(GachaId:{GachaId},BoxGroup:{BoxGroup},DropBoxId:{DropBoxId},ShopId:{ShopId},CoinId:{CoinId},CoinAmount:{CoinAmount})";
    }
}

[XmlType]
public class GachaContent
{
    [XmlElement(Order = 1)]
    public int ItemId;
    [XmlElement(Order = 2)]
    public byte SmartDrop;
    [XmlElement(Order = 3)]
    public bool SmartGender;
    [XmlElement(Order = 4)]
    public short MinAmount;
    [XmlElement(Order = 5)]
    public short MaxAmount;
    [XmlElement(Order = 6)]
    public byte Rarity;

    public override string ToString()
    {
        return $"GachaContent:(ItemId:{ItemId},SmartDrop:{SmartDrop},SmartGender:{SmartGender},MinAmount:{MinAmount},MaxAmount:{MaxAmount},Rarity:{Rarity})";
    }
}
