using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class PremiumClubDailyBenefitMetadataStorage
{
    private static readonly Dictionary<int, PremiumClubDailyBenefitMetadata> PremiumClubDailyBenefitMetadatas = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.PremiumClubDailyBenefit}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
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

    public static PremiumClubDailyBenefitMetadata GetMetadata(int benefitId)
    {
        return PremiumClubDailyBenefitMetadatas.GetValueOrDefault(benefitId);
    }

    public static int GetId(int benefitId)
    {
        return PremiumClubDailyBenefitMetadatas.GetValueOrDefault(benefitId).BenefitId;
    }
}
