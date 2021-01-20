using System.Collections.Generic;
using System.IO;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    // This is an in-memory storage to help with determining some metadata of items
    public static class ItemMetadataStorage
    {
        private static readonly Dictionary<int, ItemMetadata> map = new Dictionary<int, ItemMetadata>();

        static ItemMetadataStorage()
        {
            using FileStream stream = File.OpenRead("Maple2Storage/Resources/ms2-item-metadata");
            List<ItemMetadata> items = Serializer.Deserialize<List<ItemMetadata>>(stream);
            foreach (ItemMetadata item in items)
            {
                map[item.Id] = item;
            }
        }

        public static bool IsValid(int itemId)
        {
            return map.ContainsKey(itemId);
        }

        public static ItemMetadata GetMetadata(int itemId)
        {
            return map.GetValueOrDefault(itemId);
        }

        public static ItemSlot GetSlot(int itemId)
        {
            return map.GetValueOrDefault(itemId).Slot;
        }

        public static GemSlot GetGem(int itemId)
        {
            return map.GetValueOrDefault(itemId).Gem;
        }

        public static InventoryTab GetTab(int itemId)
        {
            return map.GetValueOrDefault(itemId).Tab;
        }

        public static int GetRarity(int itemId)
        {
            return map.GetValueOrDefault(itemId).Rarity;
        }

        public static int GetSlotMax(int itemId)
        {
            return map.GetValueOrDefault(itemId).SlotMax;
        }

        public static bool GetIsTemplate(int itemId)
        {
            return map.GetValueOrDefault(itemId).IsTemplate;
        }

        public static int GetPlayCount(int itemId)
        {
            return map.GetValueOrDefault(itemId).PlayCount;
        }

        public static List<int> GetRecommendJobs(int itemId)
        {
            return map.GetValueOrDefault(itemId).RecommendJobs;
        }

        public static List<ItemContent> GetContent(int itemId)
        {
            return map.GetValueOrDefault(itemId).Content;
        }
    }
}
