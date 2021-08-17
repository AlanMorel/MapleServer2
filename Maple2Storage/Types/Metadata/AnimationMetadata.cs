using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class AnimationMetadata
    {
        [XmlElement(Order = 1)]
        public string ActorId;
        [XmlElement(Order = 2)]
        public List<SequenceMetadata> Sequence;

        public AnimationMetadata()
        {
            Sequence = new List<SequenceMetadata>();
        }
    }

    [XmlType]
    public class SequenceMetadata
    {
        [XmlElement(Order = 1)]
        public short SequenceId;
        [XmlElement(Order = 2)]
        public string SequenceName;
        [XmlElement(Order = 3)]
        public List<KeyMetadata> Keys;

        public SequenceMetadata()
        {
            Keys = new List<KeyMetadata>();
        }
    }

    [XmlType]
    public class KeyMetadata
    {
        [XmlElement(Order = 1)]
        public string KeyName;
        [XmlElement(Order = 2)]
        public float KeyTime;
    }
}
