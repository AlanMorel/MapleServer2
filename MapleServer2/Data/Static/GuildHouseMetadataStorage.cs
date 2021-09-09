using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class GuildHouseMetadataStorage
    {
        private static readonly Dictionary<int, GuildHouseMetadata> houses = new Dictionary<int, GuildHouseMetadata>();

        public static void Init()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-guild-house-metadata");
            List<GuildHouseMetadata> items = Serializer.Deserialize<List<GuildHouseMetadata>>(stream);
            foreach (GuildHouseMetadata item in items)
            {
                houses[item.FieldId] = item;
            }
        }

        public static GuildHouseMetadata GetMetadataByThemeId(int level, int themeId)
        {
            return houses.Values.FirstOrDefault(x => x.Level == level && x.Theme == themeId);
        }

        public static int GetFieldId(int level, int themeId)
        {
            return houses.Values.FirstOrDefault(x => x.Level == level && x.Theme == themeId).FieldId;
        }
    }
}
