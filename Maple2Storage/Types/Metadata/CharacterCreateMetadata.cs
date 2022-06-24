using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class CharacterCreateMetadata
{
    [XmlElement(Order = 1)]
    public List<int> DisabledJobs;
}
