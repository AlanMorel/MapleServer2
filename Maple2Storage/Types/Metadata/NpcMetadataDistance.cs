using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class NpcMetadataDistance
    {
        [XmlElement]
        public short Avoid;
        [XmlElement]
        public short Sight;
        [XmlElement]
        public int SightHeightUp;
        [XmlElement]
        public int sightHeightDown;
        [XmlElement]
        public int CustomLastSightRadius;
        [XmlElement]
        public int CustomLastSightUp;
        [XmlElement]
        public int CustomLastSightDown;

        public NpcMetadataDistance()
        {

        }
    }

}
