using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Maple2Storage.Types;

// TODO: make this class thread safe?
namespace MapleServer2.Types
{
    public class Inventory
    {
        public short Size { get; }
        public IReadOnlyDictionary<long, Item> Items => items;
        // This contains ALL inventory items regardless of tab
        private readonly Dictionary<long, Item> items;

        // Map of Slot to Uid for each inventory
        private readonly Dictionary<short, long>[] slotMaps;

        public Inventory(short size)
        {
            this.Size = size;
            this.items = new Dictionary<long, Item>();
            byte maxTabs = Enum.GetValues(typeof(InventoryTab)).Cast<byte>().Max();
            this.slotMaps = new Dictionary<short, long>[maxTabs + 1];
            for (byte i = 0; i <= maxTabs; i++)
            {
                this.slotMaps[i] = new Dictionary<short, long>();
            }
        }

        public Inventory(short size, IEnumerable<Item> loadItems) : this(size)
        {
            foreach (Item item in loadItems)
            {
                Add(item);
            }
        }

        public ICollection<Item> GetItems(InventoryTab tab)
        {
            return GetSlots(tab).Select(kvp => items[kvp.Value])
                .ToImmutableList();
        }

        // TODO: precompute next free slot to avoid iteration on Add
        // TODO: Stack items when they are the same
        // Returns false if inventory is full
        public bool Add(Item item)
        {
            // Item has a slot set, try to use that slot
            if (item.Slot >= 0)
            {
                if (!SlotTaken(item, item.Slot))
                {
                    if (isStackable(item))
                    {
                        Stack(item);
                    }
                    else
                    {
                        AddInternal(item);
                    }
                    return true;
                }

                item.Slot = -1; // Reset slot
            }

            for (short i = 0; i < Size; i++)
            {
                if (!SlotTaken(item, i))
                {
                    item.Slot = i;
                    if (isStackable(item))
                    {
                        Stack(item);
                    }
                    else
                    {
                        AddInternal(item);
                    }
                    return true;
                }
            }
            return false;
        }

        // Checks if item is stackable.
        private bool isStackable(Item item)
        {
            return item.SlotMax > 1;
        }

        private void Stack(Item item) // Replace 10 with item.slotmax to test against actual max slot count.
        {
            foreach (Item i in items.Values)
            {
                if (i.Id != item.Id || i.Amount > i.SlotMax)
                {
                    continue;
                }
                if ((i.Amount + item.Amount) > i.SlotMax)
                {
                    item.Amount = item.Amount - (i.SlotMax - i.Amount);
                    i.Amount = i.SlotMax;
                }
                else
                {
                    i.Amount = i.Amount + item.Amount;
                    item.Amount = 0;
                    return;
                }
            }
            AddInternal(item);
        }

        // Returns false if item doesn't exist or removing more than available
        public int Remove(long uid, out Item removedItem, int amount = -1)
        {
            // Removing more than available
            if (!items.TryGetValue(uid, out Item item) || item.Amount < amount)
            {
                removedItem = null;
                return -1;
            }

            if (amount < 0 || item.Amount == amount)
            { // Remove All
                if (!RemoveInternal(uid, out removedItem))
                {
                    return -1;
                }

                removedItem.Slot = -1;
                return 0;
            }

            return item.TrySplit(amount, out removedItem) ? item.Amount : -1;
        }

        // Replaces an existing item with an updated copy of itself
        public bool Replace(Item item)
        {
            if (!items.ContainsKey(item.Uid))
            {
                return false;
            }

            RemoveInternal(item.Uid, out Item replacedItem);
            item.Slot = replacedItem.Slot;
            AddInternal(item);

            return true;
        }

        // Returns null if item doesn't exist
        // Returns the uid and slot of destItem (uid is 0 if empty)
        public Tuple<long, short> Move(long uid, short dstSlot)
        {
            bool srcResult = RemoveInternal(uid, out Item srcItem);
            if (!srcResult)
            {
                return null;
            }

            short srcSlot = srcItem.Slot;
            // Move dstItem to srcSlot if removed
            bool dstResult = RemoveInternal(srcItem.InventoryType, dstSlot, out Item dstItem);
            if (dstResult)
            {
                dstItem.Slot = srcSlot;
                AddInternal(dstItem);
            }

            // Move srcItem to dstSlot
            srcItem.Slot = dstSlot;
            AddInternal(srcItem);
            return new Tuple<long, short>(dstItem?.Uid ?? 0, srcSlot);
        }

        public void Sort(InventoryTab tab)
        {
            // Get all items in tab and sort by Id
            Dictionary<short, long> slots = GetSlots(tab);
            List<Item> tabItems = slots.Select(kvp => Items[kvp.Value]).ToList();
            tabItems.Sort((x, y) => x.Id.CompareTo(y.Id));

            // Update the slot mapping
            slots.Clear();
            for (short i = 0; i < tabItems.Count; i++)
            {
                tabItems[i].Slot = i;
                slots[i] = tabItems[i].Uid;
            }
        }

        // This REQUIRES item.Slot to be set appropriately
        private void AddInternal(Item item)
        {
            Debug.Assert(!items.ContainsKey(item.Uid),
                "Error adding an item that already exists");
            items[item.Uid] = item;

            Debug.Assert(!GetSlots(item.InventoryType).ContainsKey(item.Slot),
                "Error adding item to slot that is already taken.");
            GetSlots(item.InventoryType)[item.Slot] = item.Uid;
        }

        private bool RemoveInternal(long uid, out Item item)
        {
            return items.Remove(uid, out item)
                   && GetSlots(item.InventoryType).Remove(item.Slot);
        }

        private bool RemoveInternal(InventoryTab tab, short slot, out Item item)
        {
            if (!GetSlots(tab).TryGetValue(slot, out long uid))
            {
                item = null;
                return false;
            }

            return RemoveInternal(uid, out item);
        }

        private bool SlotTaken(Item item, short slot = -1)
        {
            return GetSlots(item.InventoryType).ContainsKey(slot < 0 ? item.Slot : slot);
        }

        private Dictionary<short, long> GetSlots(InventoryTab tab)
        {
            return slotMaps[(int)tab];
        }
    }
}