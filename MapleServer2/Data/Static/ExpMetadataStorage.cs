using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ExpMetadataStorage
{
    private static readonly Dictionary<int, ExpMetadata> ExpMetadatas = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.ExpTable);
        List<ExpMetadata> items = Serializer.Deserialize<List<ExpMetadata>>(stream);
        foreach (ExpMetadata item in items)
        {
            ExpMetadatas[item.Level] = item;
        }
    }

    public static ExpMetadata? GetMetadata(short level)
    {
        return ExpMetadatas.GetValueOrDefault(level);
    }

    public static bool LevelExist(short level)
    {
        return ExpMetadatas.ContainsKey(level);
    }

    public static long GetExpToLevel(short level)
    {
        return GetMetadata(level)?.Experience ?? 0;
    }
}
