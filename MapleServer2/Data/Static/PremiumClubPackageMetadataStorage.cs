using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class PremiumClubPackageMetadataStorage
{
    private static readonly Dictionary<int, PremiumClubPackageMetadata> PremiumClubPackage = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.PremiumClubPackage}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
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
