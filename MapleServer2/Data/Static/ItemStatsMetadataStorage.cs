using System.Collections.Generic;
using System.IO;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class ItemStatsMetadataStorage
    {
        private static readonly Dictionary<int, List<ItemOptions>> Constant = new Dictionary<int, List<ItemOptions>>();
        private static readonly Dictionary<int, List<ItemOptions>> Random = new Dictionary<int, List<ItemOptions>>();
        private static readonly Dictionary<int, List<ItemOptions>> Static = new Dictionary<int, List<ItemOptions>>();

        static ItemStatsMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-item-stats-metadata");
            List<ItemStatsMetadata> items = Serializer.Deserialize<List<ItemStatsMetadata>>(stream);
            foreach (ItemStatsMetadata item in items)
            {
                if (!Constant.ContainsKey(item.ItemId))
                {
                    Constant[item.ItemId] = item.Constant;
                }
                else
                {
                    Constant[item.ItemId].AddRange(item.Constant);
                }
                if (!Random.ContainsKey(item.ItemId))
                {
                    Random[item.ItemId] = item.Random;
                }
                else
                {
                    Random[item.ItemId].AddRange(item.Random);
                }
                if (!Static.ContainsKey(item.ItemId))
                {
                    Static[item.ItemId] = item.Static;
                }
                else
                {
                    Static[item.ItemId].AddRange(item.Static);
                }
            }

        }

        public static bool HasConstantStats(int itemId) => Constant.ContainsKey(itemId);

        public static bool HasRandomStats(int itemId) => Random.ContainsKey(itemId);

        public static bool HasStaticStats(int itemId) => Static.ContainsKey(itemId);

        public static bool GetConstantStat(int itemId, out List<ItemOptions> list)
        {
            list = null;
            if (HasConstantStats(itemId))
            {
                list = Constant[itemId];
                return true;
            }
            return false;
        }

        public static bool GetStaticStat(int itemId, out List<ItemOptions> list)
        {
            list = null;
            if (HasStaticStats(itemId))
            {
                list = Static[itemId];
                return true;
            }
            return false;
        }
        public static bool GetRandomStat(int itemId, out List<ItemOptions> list)
        {
            list = null;
            if (HasRandomStats(itemId))
            {
                list = Random[itemId];
                return true;
            }
            return false;
        }
    }
}
