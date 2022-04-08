using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ItemSocketMetadataStorage
{
    private static readonly Dictionary<int, ItemSocketMetadata> ItemSocketMetadatas = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.ItemSocket);
        List<ItemSocketMetadata> items = Serializer.Deserialize<List<ItemSocketMetadata>>(stream);
        foreach (ItemSocketMetadata item in items)
        {
            ItemSocketMetadatas[item.Id] = item;
        }
    }

    public static ItemSocketRarityData GetMetadata(int socketDataId, int rarity)
    {
        return ItemSocketMetadatas.GetValueOrDefault(socketDataId)?.RarityData.FirstOrDefault(x => x.Rarity == rarity);
    }
}
