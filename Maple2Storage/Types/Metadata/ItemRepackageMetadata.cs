using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class ItemRepackageMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public int MinLevel;
    [XmlElement(Order = 3)]
    public int MaxLevel;
    [XmlElement(Order = 4)]
    public List<int> Slots = new();
    [XmlElement(Order = 5)]
    public List<int> Rarities = new();
    [XmlElement(Order = 6)]
    public int PetType;
}
