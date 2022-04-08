using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class GuildServiceMetadataStorage
{
    private static readonly Dictionary<int, GuildServiceMetadata> Services = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.GuildService);
        List<GuildServiceMetadata> items = Serializer.Deserialize<List<GuildServiceMetadata>>(stream);
        foreach (GuildServiceMetadata item in items)
        {
            Services[item.Level] = item;
        }
    }

    public static GuildServiceMetadata GetMetadata(int id, int level)
    {
        return Services.Values.First(x => x.Id == id && x.Level == level + 1);
    }
}
