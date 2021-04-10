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
        public List<MapBlock> Blocks { get; set; }

        public MapMetadata()
        {
            Blocks = new List<MapBlock>();
        }

        protected bool Equals(MapMetadata other)
        {
            return Id == other.Id &&
                   Blocks == other.Blocks;
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

        public MapBlock() { }

        public MapBlock(CoordS coord, string attribute, string type)
        {
            Coord = coord;
            Attribute = attribute;
            Type = type;
        }

        protected bool Equals(MapBlock other)
        {
            return Coord == other.Coord &&
                   Attribute == other.Attribute &&
                   Type == other.Type;
        }
    }
}
