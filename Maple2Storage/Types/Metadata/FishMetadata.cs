using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class FishMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public string Habitat;
    [XmlElement(Order = 3)]
    public int CompanionId;
    [XmlElement(Order = 4)]
    public short Mastery;
    [XmlElement(Order = 5)]
    public byte Rarity;
    [XmlElement(Order = 6)]
    public short[] SmallSize = Array.Empty<short>();
    [XmlElement(Order = 7)]
    public short[] BigSize = Array.Empty<short>();
    [XmlElement(Order = 8)]
    public bool IgnoreMastery;
    [XmlElement(Order = 9)]
    public List<int> HabitatMapId = new();
}
