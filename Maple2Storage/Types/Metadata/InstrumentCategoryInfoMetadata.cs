using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class InstrumentCategoryInfoMetadata
    {
        [XmlElement(Order = 1)]
        public byte CategoryId;
        [XmlElement(Order = 2)]
        public byte GMId;
        [XmlElement(Order = 3)]
        public string Octave;
        [XmlElement(Order = 4)]
        public byte PercussionId;

        public InstrumentCategoryInfoMetadata() { }

        public InstrumentCategoryInfoMetadata(byte categoryId, byte gmId, string octave, byte perucssionId)
        {
            CategoryId = categoryId;
            GMId = gmId;
            Octave = octave;
            PercussionId = perucssionId;
        }

        public override string ToString() =>
    $"InstrumentCategoryInfo(CategoryId:{CategoryId},GMId:{GMId},Octave:{Octave},PercussionId{PercussionId})";
    }
}
