using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class GuildHouseMetadata
{
    [XmlElement(Order = 1)]
    public int FieldId;
    [XmlElement(Order = 2)]
    public int Level;
    [XmlElement(Order = 3)]
    public int Theme;
    [XmlElement(Order = 4)]
    public int RequiredLevel;
    [XmlElement(Order = 5)]
    public int UpgradeCost;
    [XmlElement(Order = 6)]
    public int RethemeCost;

    public override string ToString()
    {
        return $"GuildBuff(FieldId:{FieldId},Level:{Level},Theme:{Theme},RequiredLevel:{RequiredLevel},UpgradeCost:{UpgradeCost},RethemeCost:{RethemeCost}";
    }
}
