using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class ItemExchangeScrollMetadataStorage
    {
        private static readonly Dictionary<int, ItemExchangeScrollMetadata> map = new Dictionary<int, ItemExchangeScrollMetadata>();

        public static void Init()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-item-exchange-scroll-metadata");
            List<ItemExchangeScrollMetadata> items = Serializer.Deserialize<List<ItemExchangeScrollMetadata>>(stream);
            foreach (ItemExchangeScrollMetadata item in items)
            {
                map[item.ExchangeId] = item;
            }
        }

        public static bool IsValid(int exchangeId)
        {
            return map.ContainsKey(exchangeId);
        }

        public static ItemExchangeScrollMetadata GetMetadata(int exchangeId)
        {
            return map.GetValueOrDefault(exchangeId);
        }

        public static int GetId(int exchangeId)
        {
            return map.GetValueOrDefault(exchangeId).ExchangeId;
        }
    }
}
