using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class MapMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public string Name = "";
    [XmlElement(Order = 3)]
    public string XBlockName = "";
    [XmlElement(Order = 4)]
    public MapProperty Property;
    [XmlElement(Order = 5)]
    public Dictionary<CoordS, MapBlock> Blocks = new();
    [XmlElement(Order = 6)]
    public MapEntityMetadata Entities = new();
}

[XmlType]
public class MapBlock
{
    [XmlElement(Order = 1)]
    public CoordS Coord;
    [XmlElement(Order = 2)]
    public string Attribute;
    [XmlElement(Order = 3)]
    public string Type;
    [XmlElement(Order = 4)]
    public int SaleableGroup;
}

[XmlType]
public class MapProperty
{
    [XmlElement(Order = 1)]
    public int RevivalReturnMapId;
    [XmlElement(Order = 2)]
    public string EnterReturnMapId;
    [XmlElement(Order = 3)]
    public short Capacity;
}
