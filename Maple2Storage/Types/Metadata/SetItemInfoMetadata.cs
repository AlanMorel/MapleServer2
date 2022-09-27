using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class SetItemInfoMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public int[] ItemIds;
    [XmlElement(Order = 3)]
    public int OptionId;
    [XmlElement(Order = 4)]
    public string Feature;
    [XmlElement(Order = 5)]
    public bool ShowEffectIfItsSetItemMotion;
    [XmlElement(Order = 6)]
    public bool IsDisableTooltip;
}
