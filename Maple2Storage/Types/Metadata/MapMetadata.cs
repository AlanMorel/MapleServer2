using System.Collections.Generic;
using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
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
        public Dictionary<CoordS, MapBlock> Blocks;

        public MapMetadata()
        {
            Blocks = new Dictionary<CoordS, MapBlock>();
        }
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

        public MapBlock() { }

        public MapBlock(CoordS coord, string attribute, string type)
        {
            Coord = coord;
            Attribute = attribute;
            Type = type;
        }
    }
}
