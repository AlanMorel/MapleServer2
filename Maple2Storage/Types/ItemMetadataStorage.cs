using System.Collections.Generic;
using System.IO;
using Maple2Storage.Enums;
using ProtoBuf;

namespace Maple2Storage.Types {
    // This is an in-memory storage to help with determining some metadata of items
    public static class ItemMetadataStorage {
        private static readonly Dictionary<int, ItemMetadata> map = new Dictionary<int, ItemMetadata>();

        static ItemMetadataStorage() {
            using FileStream stream = File.OpenRead("Resources/ms2-item-metadata");
            List<ItemMetadata> items = Serializer.Deserialize<List<ItemMetadata>>(stream);
            foreach (ItemMetadata item in items) {
                map[item.Id] = item;
            }
        }

        public static bool IsValid(int itemId) {
            return map.ContainsKey(itemId);
        }

        public static ItemMetadata GetMetadata(int itemId) {
            return map.GetValueOrDefault(itemId);
        }

        public static EquipSlot GetSlot(int itemId) {
            return map.GetValueOrDefault(itemId).Slot;
        }

        public static InventoryType GetTab(int itemId) {
            return map.GetValueOrDefault(itemId).Tab;
        }

        public static int GetSlotMax(int itemId) {
            return map.GetValueOrDefault(itemId).SlotMax;
        }

        public static bool GetIsTemplate(int itemId) {
            return map.GetValueOrDefault(itemId).IsTemplate;
        }
    }
}
