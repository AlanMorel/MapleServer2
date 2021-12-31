using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class SurvivalGoldPassRewardMetadata
{
    [XmlElement(Order = 1)]
    public int Level;
    [XmlElement(Order = 2)]
    public string Type1;
    [XmlElement(Order = 3)]
    public string Id1;
    [XmlElement(Order = 4)]
    public string Value1;
    [XmlElement(Order = 5)]
    public string Count1;
    [XmlElement(Order = 6)]
    public string Type2;
    [XmlElement(Order = 7)]
    public string Id2;
    [XmlElement(Order = 8)]
    public string Value2;
    [XmlElement(Order = 9)]
    public string Count2;
}
