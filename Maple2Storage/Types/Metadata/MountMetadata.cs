using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public record MountMetadata
{
    [XmlElement(Order = 1)]
    public int Id { get; init; }

    [XmlElement(Order = 2)]
    public int RunConsumeEp { get; init; }

    [XmlElement(Order = 3)]
    public XmlStats MountStats { get; init; }
}

