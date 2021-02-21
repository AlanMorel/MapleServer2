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
        public string Name;
        [XmlElement(Order = 3)]
        public string Model = string.Empty;
        [XmlElement(Order = 4)]
        public int TemplateId;
        [XmlElement(Order = 5)]
        public byte Friendly;
        [XmlElement(Order = 6)]
        public short Level;
        [XmlElement(Order = 7)]
        public NpcStats Stats;
        [XmlElement(Order = 8)]
        public int[] SkillIds = Array.Empty<int>();
        [XmlElement(Order = 9)]
        public string AiInfo = string.Empty;
        [XmlElement(Order = 10)]
        public long Experience;
        [XmlElement(Order = 11)]
        public float DeadTime;
        [XmlElement(Order = 12)]
        public string[] DeadActions = Array.Empty<string>();
        [XmlElement(Order = 13)]
        public int[] GlobalDropBoxIds = Array.Empty<int>();
        [XmlElement(Order = 14)]
        public CoordS Rotation; // In degrees * 10
        [XmlElement(Order = 15)]
        public CoordS Speed;
        [XmlElement(Order = 16)]
        public CoordS Coord;
        [XmlElement(Order = 17)]
        public byte Animation;
        [XmlElement(Order = 18)]
        public short Kind; // 13 = Shop
        [XmlElement(Order = 19)]
        public int ShopId;

        public NpcMetadata()
        {

        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Coord, Rotation);
        }

        public override string ToString() =>
            $"NPC: Id: {Id}, Name: {Name}, Type: {Kind}, Position: {Coord}, Model:{Model}, TemplateID: {TemplateId}, Friendly: {Friendly}, IsShop: {Kind == 13}";
    }
}
