using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class PremiumClubPackageMetadataStorage
    {
        private static readonly Dictionary<int, PremiumClubPackageMetadata> package = new Dictionary<int, PremiumClubPackageMetadata>();

        public static void Init()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-premium-club-package-metadata");
            List<PremiumClubPackageMetadata> items = Serializer.Deserialize<List<PremiumClubPackageMetadata>>(stream);
            foreach (PremiumClubPackageMetadata item in items)
            {
                package[item.Id] = item;
            }
        }

        public static bool IsValid(int packageId)
        {
            return package.ContainsKey(packageId);
        }

        public static PremiumClubPackageMetadata GetMetadata(int packageId)
        {
            return package.GetValueOrDefault(packageId);
        }

        public static int GetId(int packageId)
        {
            return package.GetValueOrDefault(packageId).Id;
        }
    }
}
