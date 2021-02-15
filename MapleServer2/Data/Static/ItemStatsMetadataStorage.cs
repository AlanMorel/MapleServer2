using System.Collections.Generic;
using System.IO;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class ItemStatsMetadataStorage
    {
        private static readonly Dictionary<int, List<ItemOptions>> Basic = new Dictionary<int, List<ItemOptions>>();
        private static readonly Dictionary<int, List<ItemOptions>> RandomBonus = new Dictionary<int, List<ItemOptions>>();
        private static readonly Dictionary<int, List<ItemOptions>> StaticBonus = new Dictionary<int, List<ItemOptions>>();

        static ItemStatsMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-item-stats-metadata");
            List<ItemStatsMetadata> items = Serializer.Deserialize<List<ItemStatsMetadata>>(stream);
            foreach (ItemStatsMetadata item in items)
            {
                if (!Basic.ContainsKey(item.ItemId))
                {
                    Basic[item.ItemId] = item.Basic;
                }
                else
                {
                    Basic[item.ItemId].AddRange(item.Basic);
                }
                if (!RandomBonus.ContainsKey(item.ItemId))
                {
                    RandomBonus[item.ItemId] = item.RandomBonus;
                }
                else
                {
                    RandomBonus[item.ItemId].AddRange(item.RandomBonus);
                }
                if (!StaticBonus.ContainsKey(item.ItemId))
                {
                    StaticBonus[item.ItemId] = item.StaticBonus;
                }
                else
                {
                    StaticBonus[item.ItemId].AddRange(item.StaticBonus);
                }
            }

        }

        public static bool HasBasic(int itemId) => Basic.ContainsKey(itemId);

        public static bool HasRandomBonus(int itemId) => RandomBonus.ContainsKey(itemId);

        public static bool HasStaticBonus(int itemId) => StaticBonus.ContainsKey(itemId);

        public static bool GetBasic(int itemId, out List<ItemOptions> list)
        {
            list = null;
            if (HasBasic(itemId))
            {
                list = Basic[itemId];
                return true;
            }
            return false;
        }

        public static bool GetStaticBonus(int itemId, out List<ItemOptions> list)
        {
            list = null;
            if (HasStaticBonus(itemId))
            {
                list = StaticBonus[itemId];
                return true;
            }
            return false;
        }
        public static bool GetRandomBonus(int itemId, out List<ItemOptions> list)
        {
            list = null;
            if (HasRandomBonus(itemId))
            {
                list = RandomBonus[itemId];
                return true;
            }
            return false;
        }
    }
}
