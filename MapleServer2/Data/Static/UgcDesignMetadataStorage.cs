using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class UgcDesignMetadataStorage
{
    private static readonly Dictionary<int, UgcDesignMetadata> dict = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-ugc-design-metadata");
        List<UgcDesignMetadata> items = Serializer.Deserialize<List<UgcDesignMetadata>>(stream);
        foreach (UgcDesignMetadata item in items)
        {
            dict[item.ItemId] = item;
        }
    }

    public static UgcDesignMetadata GetItem(int itemId) => dict.TryGetValue(itemId, out UgcDesignMetadata item) ? item : null;
}
