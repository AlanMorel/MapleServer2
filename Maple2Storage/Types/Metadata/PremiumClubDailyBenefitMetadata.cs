using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
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

        public PremiumClubDailyBenefitMetadata() { }

        public PremiumClubDailyBenefitMetadata(int benefitId, int itemId, byte itemRarity, short itemAmount)
        {
            BenefitId = benefitId;
            ItemId = itemId;
            ItemRarity = itemRarity;
            ItemAmount = itemAmount;
        }

        public override string ToString() =>
    $"ItemRequirement(BenefitId:{BenefitId},ItemId:{ItemId},ItemRarity:{ItemRarity},ItemAmount:{ItemAmount})";
    }
}
