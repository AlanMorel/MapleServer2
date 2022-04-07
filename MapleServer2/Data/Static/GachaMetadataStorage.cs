using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class GachaMetadataStorage
{
    private static readonly Dictionary<int, GachaMetadata> Gacha = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.Gacha);
        List<GachaMetadata> items = Serializer.Deserialize<List<GachaMetadata>>(stream);
        foreach (GachaMetadata item in items)
        {
            Gacha[item.GachaId] = item;
        }
    }

    public static bool IsValid(int gachaId)
    {
        return Gacha.ContainsKey(gachaId);
    }

    public static GachaMetadata GetMetadata(int gachaId)
    {
        return Gacha.GetValueOrDefault(gachaId);
    }
}
