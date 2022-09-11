using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class UgcDesignMetadataStorage
{
    private static readonly Dictionary<int, UgcDesignMetadata> UgcDesign = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.UGCDesign);
        List<UgcDesignMetadata> items = Serializer.Deserialize<List<UgcDesignMetadata>>(stream);
        foreach (UgcDesignMetadata item in items)
        {
            UgcDesign[item.ItemId] = item;
        }
    }

    public static UgcDesignMetadata? GetItem(int itemId) => UgcDesign.TryGetValue(itemId, out UgcDesignMetadata? item) ? item : null;
}
