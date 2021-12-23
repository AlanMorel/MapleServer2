using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class ItemSocketMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public int MaxCount;
    [XmlElement(Order = 3)]
    public int FixedOpenCount;

    public override string ToString()
    {
        return $"ItemSocket(Id:{Id},MaxCount:{MaxCount},FixedOpenCount:{FixedOpenCount})";
    }
}
