using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class PremiumClubDailyBenefitMetadataStorage
    {
        private static readonly Dictionary<int, PremiumClubDailyBenefitMetadata> benefit = new Dictionary<int, PremiumClubDailyBenefitMetadata>();

        static PremiumClubDailyBenefitMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-premium-club-daily-benefit-metadata");
            List<PremiumClubDailyBenefitMetadata> items = Serializer.Deserialize<List<PremiumClubDailyBenefitMetadata>>(stream);
            foreach (PremiumClubDailyBenefitMetadata item in items)
            {
                benefit[item.BenefitId] = item;
            }
        }

        public static bool IsValid(int benefitId)
        {
            return benefit.ContainsKey(benefitId);
        }

        public static PremiumClubDailyBenefitMetadata GetMetadata(int benefitId)
        {
            return benefit.GetValueOrDefault(benefitId);
        }

        public static int GetId(int benefitId)
        {
            return benefit.GetValueOrDefault(benefitId).BenefitId;
        }
    }
}
