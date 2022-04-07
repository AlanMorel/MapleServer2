using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ItemExchangeScrollMetadataStorage
{
    private static readonly Dictionary<int, ItemExchangeScrollMetadata> ItemExchangeScrollMetadatas = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.ItemExchangeScroll);
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
