using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class PremiumClubEffectMetadataStorage
{
    private static readonly Dictionary<int, PremiumClubEffectMetadata> PremiumClubDailyBenefitMetadatas = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.PremiumClubEffect);
        List<PremiumClubEffectMetadata> items = Serializer.Deserialize<List<PremiumClubEffectMetadata>>(stream);
        foreach (PremiumClubEffectMetadata item in items)
        {
            PremiumClubDailyBenefitMetadatas[item.EffectId] = item;
        }
    }

    public static List<PremiumClubEffectMetadata> GetBuffs()
    {
        return PremiumClubDailyBenefitMetadatas.Values.ToList();
    }
}
