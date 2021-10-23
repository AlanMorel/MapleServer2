using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class GuildServiceMetadataStorage
    {
        private static readonly Dictionary<int, GuildServiceMetadata> services = new Dictionary<int, GuildServiceMetadata>();

        public static void Init()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-guild-service-metadata");
            List<GuildServiceMetadata> items = Serializer.Deserialize<List<GuildServiceMetadata>>(stream);
            foreach (GuildServiceMetadata item in items)
            {
                services[item.Level] = item;
            }
        }

        public static GuildServiceMetadata GetMetadata(int id, int level)
        {
            return services.Values.Where(x => x.Id == id && x.Level == level + 1).First();
        }
    }
}
