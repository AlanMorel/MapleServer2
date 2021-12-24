using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class InstrumentInfoMetadata
{
    [XmlElement(Order = 1)]
    public byte InstrumentId;
    [XmlElement(Order = 2)]
    public byte Category;
    [XmlElement(Order = 3)]
    public byte ScoreCount;

    public override string ToString()
    {
        return $"InstrumentInfo(InstrumentId:{InstrumentId},Category:{Category},ScoreCount:{ScoreCount})";
    }
}
