using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class PremiumClubDailyBenefitMetadataStorage
{
    private static readonly Dictionary<int, PremiumClubDailyBenefitMetadata> PremiumClubDailyBenefitMetadatas = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.PremiumClubDailyBenefit);
        List<PremiumClubDailyBenefitMetadata> items = Serializer.Deserialize<List<PremiumClubDailyBenefitMetadata>>(stream);
        foreach (PremiumClubDailyBenefitMetadata item in items)
        {
            PremiumClubDailyBenefitMetadatas[item.BenefitId] = item;
        }
    }

    public static bool IsValid(int benefitId)
    {
        return PremiumClubDailyBenefitMetadatas.ContainsKey(benefitId);
    }

    public static PremiumClubDailyBenefitMetadata? GetMetadata(int benefitId)
    {
        return PremiumClubDailyBenefitMetadatas.GetValueOrDefault(benefitId);
    }
}
