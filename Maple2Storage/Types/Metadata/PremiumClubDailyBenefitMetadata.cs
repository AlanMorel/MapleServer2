using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class PremiumClubDailyBenefitMetadata
{
    [XmlElement(Order = 1)]
    public int BenefitId;
    [XmlElement(Order = 2)]
    public int ItemId;
    [XmlElement(Order = 3)]
    public byte ItemRarity;
    [XmlElement(Order = 4)]
    public short ItemAmount;

    public override string ToString()
    {
        return $"ItemRequirement(BenefitId:{BenefitId},ItemId:{ItemId},ItemRarity:{ItemRarity},ItemAmount:{ItemAmount})";
    }
}
