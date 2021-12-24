using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class ItemExtractionMetadata
{
    [XmlElement(Order = 1)]
    public int SourceItemId;
    [XmlElement(Order = 2)]
    public byte TryCount;
    [XmlElement(Order = 3)]
    public byte ScrollCount;
    [XmlElement(Order = 4)]
    public int ResultItemId;

    public override string ToString()
    {
        return $"ItemExtractionMetadata(SourceItemId:{SourceItemId}, TryCount:{TryCount}, ScrollCount:{ScrollCount}, ResultItemId:{ResultItemId})";
    }
}
