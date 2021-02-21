using System.Xml;
using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class NpcMetadataInteract
    {
        [XmlElement]
        public string InteractFunction;  // UseSkill,50100501,1
        [XmlElement]
        public ushort InteractCastingTime;  // 0, 500, 800, 1000, 2000, 4000
        [XmlElement]
        public ushort InteractCooldownTime;  // 0, 5400, 7000, 8000, 12000

        public NpcMetadataInteract()
        {

        }

    }
}
