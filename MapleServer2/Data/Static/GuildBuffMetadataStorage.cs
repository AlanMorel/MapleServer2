using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class GuildBuffMetadataStorage
{
    private static readonly Dictionary<int, GuildBuffMetadata> Buffs = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.GuildBuff);
        List<GuildBuffMetadata> items = Serializer.Deserialize<List<GuildBuffMetadata>>(stream);
        foreach (GuildBuffMetadata item in items)
        {
            Buffs[item.BuffId] = item;
        }
    }

    public static bool IsValid(int buffId)
    {
        return Buffs.ContainsKey(buffId);
    }

    public static List<int> GetBuffList()
    {
        List<int> buffIds = new();
        foreach (GuildBuffMetadata buffmetadata in Buffs.Values)
        {
            buffIds.Add(buffmetadata.BuffId);
        }
        return buffIds;
    }

    public static GuildBuffLevel? GetGuildBuffLevelData(int buffId, int buffLevel)
    {
        GuildBuffMetadata? metadata = Buffs.GetValueOrDefault(buffId);
        return metadata?.Levels.FirstOrDefault(x => x.Level == buffLevel);
    }
}
