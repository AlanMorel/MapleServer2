using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class UgcMapMetadata
{
    [XmlElement(Order = 1)]
    public int MapId;
    [XmlElement(Order = 2)]
    public List<UgcMapGroup> Groups = new();

    public override string ToString()
    {
        return $"UgcMapMetadata(MapId:{MapId},Groups:{Groups})";
    }
}

[XmlType]
public class UgcMapGroup
{
    [XmlElement(Order = 1)]
    public byte Id;
    [XmlElement(Order = 2)]
    public int Price;
    [XmlElement(Order = 3)]
    public int PriceItemCode;
    [XmlElement(Order = 4)]
    public int ExtensionPrice;
    [XmlElement(Order = 5)]
    public int ExtensionPriceItemCode;
    [XmlElement(Order = 6)]
    public short ContractDate;
    [XmlElement(Order = 7)]
    public short ExtensionDate;
    [XmlElement(Order = 8)]
    public byte HeightLimit;
    [XmlElement(Order = 9)]
    public short BuildingCount;
    [XmlElement(Order = 10)]
    public byte ReturnPlaceId;
    [XmlElement(Order = 11)]
    public short Area;
    [XmlElement(Order = 12)]
    public byte SellType;
    [XmlElement(Order = 13)]
    public byte BlockCode;
    [XmlElement(Order = 14)]
    public short HouseNumber;

    public override string ToString()
    {
        return
            $"UgcMapGroup(Id:{Id},Price:{Price},PriceItemCode:{PriceItemCode},ExtensionPrice:{ExtensionPrice},ExtensionPriceItemCode:{ExtensionPriceItemCode}," +
            $"ContractDate:{ContractDate}, ExtensionDate:{ExtensionDate},HeightLimit:{HeightLimit},BuildingCount:{BuildingCount}," +
            $"ReturnPlaceId:{ReturnPlaceId},Area:{Area},SellType:{SellType}," +
            $"BlockCode:{BlockCode},HouseNumber:{HouseNumber})";
    }
}
