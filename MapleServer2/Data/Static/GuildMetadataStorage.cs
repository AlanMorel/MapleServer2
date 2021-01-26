using System.Collections.Generic;
using System.IO;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class GuildMetadataStorage
    {
        private static readonly Dictionary<string, GuildContribution> contributions = new Dictionary<string, GuildContribution>();
        private static readonly Dictionary<int, GuildBuff> buffs = new Dictionary<int, GuildBuff>();

        static GuildMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-guild-metadata");
            GuildMetadata metadata = Serializer.Deserialize<GuildMetadata>(stream);
            foreach (GuildContribution contribution in metadata.Contribution)
            {
                contributions.Add(contribution.Type, contribution);
            }

            foreach (GuildBuff buff in metadata.Buff)
            {
                buffs.Add(buff.Id, buff);
            }
        }

        public static GuildContribution GetContribution(string type)
        {
            return contributions.GetValueOrDefault(type);
        }

        public static GuildBuff GetBuff(int id)
        {
            return buffs.GetValueOrDefault(id);
        }
    }
}
