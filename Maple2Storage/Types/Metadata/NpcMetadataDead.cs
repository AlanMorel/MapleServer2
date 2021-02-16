using System;
using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class NpcMetadataDead
    {
        [XmlElement]
        public float Time;
        [XmlElement]
        public string[] Actions = Array.Empty<string>();

        public NpcMetadataDead()
        {

        }
    }
}
