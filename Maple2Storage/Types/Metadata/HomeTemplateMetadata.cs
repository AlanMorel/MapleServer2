using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class HomeTemplateMetadata
{
    [XmlElement(Order = 1)]
    public string Id;
    [XmlElement(Order = 2)]
    public byte Size;
    [XmlElement(Order = 3)]
    public byte Height;
    [XmlElement(Order = 4)]
    public List<CubeTemplate> Cubes;
}

[XmlType]
public class CubeTemplate
{
    [XmlElement(Order = 1)]
    public int ItemId;
    [XmlElement(Order = 2)]
    public CoordF CoordF;
    [XmlElement(Order = 3)]
    public CoordF Rotation;

    public CubeTemplate() { }

    public CubeTemplate(int itemId, CoordF coordF, CoordF rotation)
    {
        ItemId = itemId;
        CoordF = coordF;
        Rotation = rotation;
    }
}
