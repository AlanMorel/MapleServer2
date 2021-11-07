using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class TitleMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public string Name;
    [XmlElement(Order = 3)]
    public string Feature;

    public TitleMetadata() { }

    public TitleMetadata(int id, string name, string feature)
    {
        Id = id;
        Name = name;
        Feature = feature;
    }
}
