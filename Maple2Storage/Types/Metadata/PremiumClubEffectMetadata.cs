using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class PremiumClubEffectMetadata
{
    [XmlElement(Order = 1)]
    public int EffectId;
    [XmlElement(Order = 2)]
    public int EffectLevel;
}
