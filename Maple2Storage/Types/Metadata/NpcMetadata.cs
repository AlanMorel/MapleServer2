using System;
using System.Xml;
using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class NpcMetadata
    {
        [XmlElement(Order = 1)]
        public int Id { get; set; }
        [XmlElement(Order = 2)]
        public string Name { get; set; }
        [XmlElement(Order = 3)]
        public string Model = string.Empty;
        [XmlElement(Order = 4)]
        public int TemplateId;
        [XmlElement(Order = 5)]
        public byte Friendly;
        [XmlElement(Order = 6)]
        public byte Level;
        [XmlElement(Order = 7)]
        public int[] SkillIds = Array.Empty<int>();
        [XmlElement(Order = 8)]
        public string AiInfo = string.Empty;  // This should be a deep structure, parsing the values in path to the XML referenced here.
        [XmlElement(Order = 9)]
        public int Experience;  // -1, 0, or some other number 6287481 (max)
        [XmlElement(Order = 10)]
        public int[] GlobalDropBoxIds = Array.Empty<int>();
        [XmlElement(Order = 11)]
        public CoordS Rotation; // In degrees * 10
        [XmlElement(Order = 12)]
        public CoordS Speed;  // Packet/Callers uses XYZ?
        [XmlElement(Order = 13)]
        public CoordS Coord;
        [XmlElement(Order = 14)]
        public byte Animation;
        [XmlElement(Order = 15)]
        public NpcMetadataBasic NpcMetadataBasic { get; set; }
        [XmlElement(Order = 16)]
        public NpcMetadataCombat NpcMetadataCombat { get; set; }
        [XmlElement(Order = 17)]
        public NpcMetadataDead NpcMetadataDead { get; set; }
        [XmlElement(Order = 18)]
        public NpcMetadataDistance NpcMetadataDistance { get; set; }  // combat related
        [XmlElement(Order = 19)]
        // Interacting with some NPCs performs an action on you, or something else.
        public NpcMetadataInteract NpcMetadataInteract { get; set; }
        [XmlElement(Order = 20)]
        public NpcStats Stats;
        [XmlElement(Order = 21)]
        public short Kind; // 13 = Shop
        [XmlElement(Order = 22)]
        public int ShopId;

        public NpcMetadata()
        {
            NpcMetadataBasic = new NpcMetadataBasic { };
            NpcMetadataCombat = new NpcMetadataCombat { };
            NpcMetadataDead = new NpcMetadataDead { };
            NpcMetadataDistance = new NpcMetadataDistance { };
            NpcMetadataInteract = new NpcMetadataInteract { };
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Coord, Rotation);
        }

        public override string ToString() =>
            $"Npc:(Id:{Id},Position:{Coord},Model:{Model}),Friendly:{Friendly},IsShop:{Kind == 13},ShopId:{ShopId})";
    }
}
