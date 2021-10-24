using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class InsturmentInfoMetadata
    {
        [XmlElement(Order = 1)]
        public byte InstrumentId;
        [XmlElement(Order = 2)]
        public byte Category;
        [XmlElement(Order = 3)]
        public byte ScoreCount;

        public InsturmentInfoMetadata() { }

        public InsturmentInfoMetadata(byte instrumentId, byte category, byte scoreCount)
        {
            InstrumentId = instrumentId;
            Category = category;
            ScoreCount = scoreCount;
        }

        public override string ToString() => $"InstrumentInfo(InstrumentId:{InstrumentId},Category:{Category},ScoreCount:{ScoreCount})";
    }
}
