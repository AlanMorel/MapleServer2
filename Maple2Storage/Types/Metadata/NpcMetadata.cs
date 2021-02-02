using System;
using System.Xml;
using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class NpcMetadata
    {
        [XmlElement(Order = 1)]
        public int Id;
        [XmlElement(Order = 2)]
        public string Model = string.Empty;
        [XmlElement(Order = 3)]
        public byte Friendly;
        [XmlElement(Order = 4)]
        public short Level;
        [XmlElement(Order = 5)]
        public NpcStats Stats;
        [XmlElement(Order = 6)]
        public int[] SkillIds = Array.Empty<int>();
        [XmlElement(Order = 7)]
        public string AiInfo = string.Empty;
        [XmlElement(Order = 8)]
        public long Experience;
        [XmlElement(Order = 9)]
        public float DeadTime;
        [XmlElement(Order = 10)]
        public string[] DeadActions = Array.Empty<string>();
        [XmlElement(Order = 11)]
        public int[] GlobalDropBoxIds = Array.Empty<int>();
        [XmlElement(Order = 12)]
        public CoordS Rotation; // In degrees * 10
        [XmlElement(Order = 13)]
        public CoordS Speed;
        [XmlElement(Order = 14)]
        public CoordS Coord;
        [XmlElement(Order = 15)]
        public byte Animation;

        public NpcMetadata()
        {

        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Coord, Rotation);
        }

        public override string ToString() =>
            $"Npc:(Id:{Id},Position:{Coord},Model:{Model}),Fiendly:{Friendly})";

    }
}
