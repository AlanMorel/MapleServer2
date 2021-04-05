using System.Collections.Generic;
using System.IO;
using System.Linq;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class MeretMarketMetadataStorage
    {
        private static readonly Dictionary<int, MeretMarketMetadata> market = new Dictionary<int, MeretMarketMetadata>();

        static MeretMarketMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-meret-market-metadata");
            List<MeretMarketMetadata> items = Serializer.Deserialize<List<MeretMarketMetadata>>(stream);
            foreach (MeretMarketMetadata item in items)
            {
                market[item.MarketItemId] = item;
            }
        }

        public static bool IsValid(int sectionId)
        {
            return market.ContainsKey(sectionId);
        }

        public static MeretMarketMetadata GetMetadata(int marketItemId)
        {
            return market.GetValueOrDefault(marketItemId);
        }

        public static List<MeretMarketMetadata> GetCategoryItems(MeretMarketCategory category)
        {
            return (from entry in market
                    where entry.Value.Category == category
                    select entry.Value).ToList();
        }
    }
}
