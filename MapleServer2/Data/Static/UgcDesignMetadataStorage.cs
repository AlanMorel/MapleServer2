using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class UgcDesignMetadataStorage
{
    private static readonly Dictionary<int, UgcDesignMetadata> UgcDesign = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.UGCDesign}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<UgcDesignMetadata> items = Serializer.Deserialize<List<UgcDesignMetadata>>(stream);
        foreach (UgcDesignMetadata item in items)
        {
            UgcDesign[item.ItemId] = item;
        }
    }

    public static UgcDesignMetadata GetItem(int itemId) => UgcDesign.TryGetValue(itemId, out UgcDesignMetadata item) ? item : null;
}
