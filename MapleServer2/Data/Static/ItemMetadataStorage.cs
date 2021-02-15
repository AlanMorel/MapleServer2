using System;
using System.Collections.Generic;
using System.IO;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using MapleServer2.Enums;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    // This is an in-memory storage to help with determining some metadata of items
    public static class ItemMetadataStorage
    {
        private static readonly Dictionary<int, ItemMetadata> map = new Dictionary<int, ItemMetadata>();

        static ItemMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-item-metadata");
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

        public static int GetStackLimit(int itemId)
        {
            return map.GetValueOrDefault(itemId).StackLimit;
        }

        public static bool GetIsTwoHand(int itemId)
        {
            return map.GetValueOrDefault(itemId).IsTwoHand;
        }

        public static bool GetIsDress(int itemId)
        {
            return map.GetValueOrDefault(itemId).IsDress;
        }

        public static bool GetIsTemplate(int itemId)
        {
            return map.GetValueOrDefault(itemId).IsTemplate;
        }

        public static int GetPlayCount(int itemId)
        {
            return map.GetValueOrDefault(itemId).PlayCount;
        }

        public static string GetFileName(int itemId)
        {
            return map.GetValueOrDefault(itemId).FileName;
        }

        public static int GetSkillID(int itemId)
        {
            return map.GetValueOrDefault(itemId).SkillID;
        }

        public static List<Job> GetRecommendJobs(int itemId)
        {
            Converter<int, Job> converter = new Converter<int, Job>((integer) => (Job) integer);

            return map.GetValueOrDefault(itemId).RecommendJobs.ConvertAll(converter);
        }

        public static List<ItemContent> GetContent(int itemId)
        {
            return map.GetValueOrDefault(itemId).Content;
        }
    }
}
