using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Maple2.Data.Types.Items;
using Maple2Storage.Enums;

namespace Maple2.Data.Types {
    // <summary>This class is thread-safe.</summary>
    public class InventoryState : IEnumerable<Item> {
        public readonly InventoryType Type;
        public short Size { get; private set; }
        public short Expansion { get; private set; }
        public short OpenSlots { get; private set; }
        public int Count => Size - OpenSlots;

        public static long GetOwnerId(long characterId, InventoryType type) => characterId * 1000 + (int) type;
        private static readonly Dictionary<InventoryType, short> defaultSizes = new Dictionary<InventoryType, short> {
            {InventoryType.Gear, 48},
            {InventoryType.Outfit, 150},
            {InventoryType.Mount, 48},
            {InventoryType.Catalyst, 48},
            {InventoryType.FishingMusic, 48},
            {InventoryType.Quest, 48},
            {InventoryType.Gemstone, 48},
            {InventoryType.Misc, 84},
            {InventoryType.LifeSkill, 126},
            {InventoryType.Pets, 60},
            {InventoryType.Consumable, 84},
            {InventoryType.Currency, 48},
            {InventoryType.Badge, 60},
            {InventoryType.Lapenshard, 48},
            {InventoryType.Fragment, 48},

            {InventoryType.GearEquip, 18},
            {InventoryType.OutfitEquip, 18},
            {InventoryType.LapenshardEquip, 7},
            {InventoryType.BadgeEquip, 12},

            {InventoryType.Trade, 5},
            {InventoryType.Storage, 36},
            {InventoryType.PetStorage, 14},
            {InventoryType.FurnishingStorage, 512},
        };

        private readonly ReaderWriterLockSlim mutex;
        private readonly Dictionary<long, short> uidToSlot;
        private Item[] slotToItem;

        public InventoryState(InventoryType type) : this(type, new Item[0]) { }

        public InventoryState(InventoryType type, IEnumerable<Item> loadItems, short expansion = 0) {
            Type = type;
            Size = (short) (defaultSizes.GetValueOrDefault(type, (short) 0) + expansion);
            Expansion = expansion;
            OpenSlots = Size;

            mutex = new ReaderWriterLockSlim();
            uidToSlot = new Dictionary<long, short>();
            slotToItem = new Item[Size];

            foreach (Item item in loadItems) {
                if (!TryPutSlot(item, item.Slot)) {
                    Console.WriteLine($"Failed to put item {item.Id} in slot {item.Slot}");
                }
            }
        }

        public bool ValidSlot(short slot) {
            mutex.EnterReadLock();
            try {
                return ValidSlotNoLock(slot);
            } finally {
                mutex.ExitReadLock();
            }
        }

        public bool SlotTaken(short slot) {
            mutex.EnterReadLock();
            try {
                return SlotTakenNoLock(slot);
            } finally {
                mutex.ExitReadLock();
            }
        }

        public bool Contains(long uid) {
            mutex.EnterReadLock();
            try {
                return uidToSlot.ContainsKey(uid);
            } finally {
                mutex.ExitReadLock();
            }
        }

        public bool TryGet(long uid, out Item item) {
            short slot;
            mutex.EnterReadLock();
            try {
                if (!uidToSlot.TryGetValue(uid, out slot)) {
                    item = default;
                    return false;
                }
            } finally {
                mutex.ExitReadLock();
            }

            item = GetSlot(slot);
            return true;
        }

        public bool TryRemove(long uid, out Item item) {
            mutex.EnterUpgradeableReadLock();
            try {
                if (!uidToSlot.TryGetValue(uid, out short slot)) {
                    item = null;
                    return false;
                }

                return TryRemoveSlot(slot, out item);
            } finally {
                mutex.ExitUpgradeableReadLock();
            }
        }

        public Item GetSlot(short slot) {
            mutex.EnterReadLock();
            try {
                return ValidSlotNoLock(slot) ? slotToItem[slot] : default;
            } finally {
                mutex.ExitReadLock();
            }
        }

        public bool TryPutSlot(Item item, short slot) {
            mutex.EnterUpgradeableReadLock();
            try {
                if (uidToSlot.ContainsKey(item.Id) || SlotTakenNoLock(slot)) {
                    return false;
                }

                mutex.EnterWriteLock();
                try {
                    item.Slot = slot;
                    uidToSlot[item.Id] = slot;
                    slotToItem[slot] = item;
                    --OpenSlots;
                    return true;
                } finally {
                    mutex.ExitWriteLock();
                }
            } finally {
                mutex.ExitUpgradeableReadLock();
            }
        }

        public bool TryRemoveSlot(short slot, out Item removedItem) {
            if (!SlotTaken(slot)) {
                removedItem = null;
                return false;
            }

            mutex.EnterWriteLock();
            try {
                removedItem = slotToItem[slot];
                slotToItem[slot] = null;
                uidToSlot.Remove(removedItem.Id, out _);
                return true;
            } finally {
                mutex.ExitWriteLock();
            }
        }

        public void Sort() {
            // Don't write lock this? (Enumerator is a read)
            List<Item> items = this.ToList();

            mutex.EnterWriteLock();
            try {
                items.Sort();

                // Update the slot mapping
                Array.Fill(slotToItem, null);
                uidToSlot.Clear();
                for (short i = 0; i < items.Count; i++) {
                    Item item = items[i];
                    uidToSlot[item.Id] = i;
                    slotToItem[i] = item;
                }
            } finally {
                mutex.ExitWriteLock();
            }
        }

        public bool Resize(short newSize) {
            if (newSize < Size) {
                return false;
            }

            mutex.EnterWriteLock();
            try {
                short diff = (short) (newSize - Size);
                Array.Resize(ref slotToItem, newSize);
                Size = newSize;
                Expansion += diff;
                OpenSlots = (short) (OpenSlots + diff);
                return true;
            } finally {
                mutex.ExitWriteLock();
            }
        }

        // Enumerate array while ignoring nulls
        public IEnumerator<Item> GetEnumerator() {
            mutex.EnterReadLock();
            try {
                for (int i = 0; i < Size; i++) {
                    if (slotToItem[i] == null) continue;
                    yield return slotToItem[i];
                }
            } finally {
                mutex.ExitReadLock();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool ValidSlotNoLock(short slot) {
            return slot >= 0 && slot < Size;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool SlotTakenNoLock(short slot) {
            return !ValidSlotNoLock(slot) || slotToItem[slot] != null;
        }
    }
}