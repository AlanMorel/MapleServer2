using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ItemSocketMetadataStorage
{
    private static readonly Dictionary<int, ItemSocketMetadata> ItemSocketMetadatas = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.ItemSocket}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<ItemSocketMetadata> items = Serializer.Deserialize<List<ItemSocketMetadata>>(stream);
        foreach (ItemSocketMetadata item in items)
        {
            ItemSocketMetadatas[item.Id] = item;
        }
    }

    public static ItemSocketMetadata GetMetadata(int socketDataId)
    {
        return ItemSocketMetadatas.GetValueOrDefault(socketDataId);
    }
}
