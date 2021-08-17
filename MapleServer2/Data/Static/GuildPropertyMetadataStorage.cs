using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class GuildPropertyMetadataStorage
    {
        private static readonly Dictionary<int, GuildPropertyMetadata> properties = new Dictionary<int, GuildPropertyMetadata>();

        static GuildPropertyMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-guild-property-metadata");
            List<GuildPropertyMetadata> items = Serializer.Deserialize<List<GuildPropertyMetadata>>(stream);
            foreach (GuildPropertyMetadata item in items)
            {
                properties[item.Level] = item;
            }
        }

        public static GuildPropertyMetadata GetMetadata(int guildExp)
        {
            foreach (GuildPropertyMetadata property in properties.Values)
            {
                if (guildExp < property.AccumExp)
                {
                    return property;
                }
            }
            // otherwise guild is max level
            return properties.Values.Last();
        }
    }
}
