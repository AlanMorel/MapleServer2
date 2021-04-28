using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            GuildPropertyMetadata metadata;
            foreach (GuildPropertyMetadata property in properties.Values)
            {
                if (guildExp < property.AccumExp)
                {
                    metadata = property;
                    return metadata;
                }
            }
            // otherwise guild is max level
            metadata = properties.Values.Last();
            return metadata;
        }
    }
}
