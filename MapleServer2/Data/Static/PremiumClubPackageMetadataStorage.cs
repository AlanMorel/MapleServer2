using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class PremiumClubPackageMetadataStorage
{
    private static readonly Dictionary<int, PremiumClubPackageMetadata> PremiumClubPackage = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-premium-club-package-metadata");
        List<PremiumClubPackageMetadata> items = Serializer.Deserialize<List<PremiumClubPackageMetadata>>(stream);
        foreach (PremiumClubPackageMetadata item in items)
        {
            PremiumClubPackage[item.Id] = item;
        }
    }

    public static bool IsValid(int packageId)
    {
        return PremiumClubPackage.ContainsKey(packageId);
    }

    public static PremiumClubPackageMetadata GetMetadata(int packageId)
    {
        return PremiumClubPackage.GetValueOrDefault(packageId);
    }

    public static int GetId(int packageId)
    {
        return PremiumClubPackage.GetValueOrDefault(packageId).Id;
    }
}
