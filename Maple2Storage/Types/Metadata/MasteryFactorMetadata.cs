using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class MasteryFactorMetadata
{
    [XmlElement(Order = 1)]
    public int Differential;
    [XmlElement(Order = 2)]
    public int Factor;

    // Required for deserialization
    public MasteryFactorMetadata() { }

    public MasteryFactorMetadata(int differential, int factor)
    {
        Differential = differential;
        Factor = factor;
    }

    public override string ToString()
    {
        return $"MasteryFactorMetadata(Differential:{Differential},Factor:{Factor})";
    }
}
