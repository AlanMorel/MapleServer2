using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class GuildContributionMetadata
{
    [XmlElement(Order = 1)]
    public string Type;
    [XmlElement(Order = 2)]
    public int Value;

    public override string ToString()
    {
        return $"GuildContribution(Type:{Type},Value:{Value})";
    }
}
