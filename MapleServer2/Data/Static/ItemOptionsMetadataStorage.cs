﻿using System.Collections.Generic;
using System.IO;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class ItemOptionsMetadataStorage
    {
        private static readonly Dictionary<int, List<ItemOption>> Basic = new Dictionary<int, List<ItemOption>>();
        private static readonly Dictionary<int, List<ItemOption>> RandomBonus = new Dictionary<int, List<ItemOption>>();
        private static readonly Dictionary<int, List<ItemOption>> StaticBonus = new Dictionary<int, List<ItemOption>>();

        static ItemOptionsMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-item-options-metadata");
            List<ItemOptionsMetadata> items = Serializer.Deserialize<List<ItemOptionsMetadata>>(stream);
            foreach (ItemOptionsMetadata item in items)
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

        public static bool GetBasic(int itemId, out List<ItemOption> list)
        {
            list = null;
            if (HasBasic(itemId))
            {
                list = Basic[itemId];
                return true;
            }
            return false;
        }

        public static bool GetStaticBonus(int itemId, out List<ItemOption> list)
        {
            list = null;
            if (HasStaticBonus(itemId))
            {
                list = StaticBonus[itemId];
                return true;
            }
            return false;
        }
        public static bool GetRandomBonus(int itemId, out List<ItemOption> list)
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
