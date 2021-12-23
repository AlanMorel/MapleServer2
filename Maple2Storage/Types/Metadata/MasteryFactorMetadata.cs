using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class MasteryFactorMetadata
{
    [XmlElement(Order = 1)]
    public int Differential;
    [XmlElement(Order = 2)]
    public int Factor;

    public override string ToString()
    {
        return $"MasteryFactorMetadata(Differential:{Differential},Factor:{Factor})";
    }
}
