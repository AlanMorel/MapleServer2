using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class InsigniaMetadataStorage
{
    private static readonly Dictionary<int, InsigniaMetadata> Insignias = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.Insignia);
        List<InsigniaMetadata> items = Serializer.Deserialize<List<InsigniaMetadata>>(stream);
        foreach (InsigniaMetadata item in items)
        {
            Insignias[item.InsigniaId] = item;
        }
    }

    public static bool IsValid(int insigniaId)
    {
        return Insignias.ContainsKey(insigniaId);
    }

    public static InsigniaMetadata? GetMetadata(int insigniaId)
    {
        return Insignias.GetValueOrDefault(insigniaId);
    }

    public static int? GetTitleId(int insigniaId)
    {
        return GetMetadata(insigniaId)?.TitleId;
    }

    public static string? GetConditionType(int insigniaId)
    {
        return GetMetadata(insigniaId)?.ConditionType;
    }
}
