using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class UGCDesignMetadataStorage
{
    private static readonly Dictionary<int, UGCDesignMetadata> dict = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-ugc-design-metadata");
        List<UGCDesignMetadata> items = Serializer.Deserialize<List<UGCDesignMetadata>>(stream);
        foreach (UGCDesignMetadata item in items)
        {
            dict[item.ItemId] = item;
        }
    }

    public static UGCDesignMetadata GetItem(int itemId) => dict.TryGetValue(itemId, out UGCDesignMetadata item) ? item : null;
}
