using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ItemExchangeScrollMetadataStorage
{
    private static readonly Dictionary<int, ItemExchangeScrollMetadata> ItemExchangeScrollMetadatas = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-item-exchange-scroll-metadata");
        List<ItemExchangeScrollMetadata> items = Serializer.Deserialize<List<ItemExchangeScrollMetadata>>(stream);
        foreach (ItemExchangeScrollMetadata item in items)
        {
            ItemExchangeScrollMetadatas[item.ExchangeId] = item;
        }
    }

    public static bool IsValid(int exchangeId)
    {
        return ItemExchangeScrollMetadatas.ContainsKey(exchangeId);
    }

    public static ItemExchangeScrollMetadata GetMetadata(int exchangeId)
    {
        return ItemExchangeScrollMetadatas.GetValueOrDefault(exchangeId);
    }

    public static int GetId(int exchangeId)
    {
        return ItemExchangeScrollMetadatas.GetValueOrDefault(exchangeId).ExchangeId;
    }
}
