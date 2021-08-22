using System.Collections.Immutable;
using System.Diagnostics;
using Maple2Storage.Types;
using MapleServer2.Database;

// TODO: make this class thread safe?
namespace MapleServer2.Types
{
    public class Inventory
    {
        public readonly long Id;
        // This contains ALL inventory Items regardless of tab
        public readonly Dictionary<long, Item> Items;
        public Dictionary<ItemSlot, Item> Equips;
        public Dictionary<ItemSlot, Item> Cosmetics;
        public List<Item> Badges;

        // Map of Slot to Uid for each inventory
        private readonly Dictionary<short, long>[] SlotMaps;

        public readonly Dictionary<InventoryTab, short> DefaultSize = new Dictionary<InventoryTab, short> {
            { InventoryTab.Gear, 48}, { InventoryTab.Outfit, 150}, { InventoryTab.Mount, 48}, { InventoryTab.Catalyst, 48},
            { InventoryTab.FishingMusic, 48}, { InventoryTab.Quest, 48}, { InventoryTab.Gemstone, 48}, { InventoryTab.Misc, 48},
            { InventoryTab.LifeSkill, 126}, { InventoryTab.Pets, 60}, { InventoryTab.Consumable, 84}, { InventoryTab.Currency, 48},
            { InventoryTab.Badge, 60}, { InventoryTab.Lapenshard, 48}, { InventoryTab.Fragment, 48}
        };

        public Dictionary<InventoryTab, short> ExtraSize = new Dictionary<InventoryTab, short> {
            { InventoryTab.Gear, 0}, { InventoryTab.Outfit, 0}, { InventoryTab.Mount, 0}, { InventoryTab.Catalyst, 0},
            { InventoryTab.FishingMusic, 0}, { InventoryTab.Quest, 0}, { InventoryTab.Gemstone, 0}, { InventoryTab.Misc, 0},
            { InventoryTab.LifeSkill, 0}, { InventoryTab.Pets, 0}, { InventoryTab.Consumable, 0}, { InventoryTab.Currency, 0},
            { InventoryTab.Badge, 0}, { InventoryTab.Lapenshard, 0}, { InventoryTab.Fragment, 0}
        };

        // Only use to share information between handler functions. Should always be empty
        public Dictionary<long, Item> TemporaryStorage = new Dictionary<long, Item>();

        public Inventory()
        {
            Equips = new Dictionary<ItemSlot, Item>();
            Cosmetics = new Dictionary<ItemSlot, Item>();
            Badges = new List<Item>();
            Items = new Dictionary<long, Item>();
            byte maxTabs = Enum.GetValues(typeof(InventoryTab)).Cast<byte>().Max();
            SlotMaps = new Dictionary<short, long>[maxTabs + 1];
            for (byte i = 0; i <= maxTabs; i++)
            {
                SlotMaps[i] = new Dictionary<short, long>();
            }
            Id = DatabaseManager.Inventories.CreateInventory(this);
        }

        public Inventory(long id, Dictionary<InventoryTab, short> extraSize, List<Item> items)
        {
            Equips = new Dictionary<ItemSlot, Item>();
            Cosmetics = new Dictionary<ItemSlot, Item>();
            Badges = new List<Item>();
            Items = new Dictionary<long, Item>();
            byte maxTabs = Enum.GetValues(typeof(InventoryTab)).Cast<byte>().Max();
            SlotMaps = new Dictionary<short, long>[maxTabs + 1];
            for (byte i = 0; i <= maxTabs; i++)
            {
                SlotMaps[i] = new Dictionary<short, long>();
            }
            Id = id;
            ExtraSize = extraSize;
            foreach (Item item in items)
            {
                item.SetMetadataValues();
                if (item.IsEquipped)
                {
                    if (item.InventoryTab == InventoryTab.Outfit)
                    {
                        Cosmetics.Add(item.ItemSlot, item);
                    }
                    else if (item.InventoryTab == InventoryTab.Badge)
                    {
                        Badges.Add(item);
                    }
                    else
                    {
                        Equips.Add(item.ItemSlot, item);
                    }
                }
                else
                {
                    Add(item);
                }
            }
        }

        public ICollection<Item> GetItems(InventoryTab tab)
        {
            return GetSlots(tab).Select(kvp => Items[kvp.Value])
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
                    AddInternal(item);
                    return true;
                }

                item.Slot = -1; // Reset slot
            }

            short tabSize = (short) (DefaultSize[item.InventoryTab] + ExtraSize[item.InventoryTab]);
            for (short i = 0; i < tabSize; i++)
            {
                if (SlotTaken(item, i))
                {
                    continue;
                }
                item.Slot = i;

                AddInternal(item);
                return true;
            }
            return false;
        }

        // Returns false if item doesn't exist or removing more than available
        public int Remove(long uid, out Item removedItem, int amount = -1)
        {
            // Removing more than available
            if (!Items.TryGetValue(uid, out Item item) || item.Amount < amount)
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

                return 0;
            }

            return item.TrySplit(amount, out removedItem) ? item.Amount : -1;
        }

        // Replaces an existing item with an updated copy of itself
        public bool Replace(Item item)
        {
            if (!Items.ContainsKey(item.Uid))
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
            bool dstResult = RemoveInternal(srcItem.InventoryTab, dstSlot, out Item dstItem);
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
            Debug.Assert(!Items.ContainsKey(item.Uid),
                "Error adding an item that already exists");
            Items[item.Uid] = item;

            Debug.Assert(!GetSlots(item.InventoryTab).ContainsKey(item.Slot),
                "Error adding item to slot that is already taken.");
            GetSlots(item.InventoryTab)[item.Slot] = item.Uid;
        }

        private bool RemoveInternal(long uid, out Item item)
        {
            return Items.Remove(uid, out item)
                   && GetSlots(item.InventoryTab).Remove(item.Slot);
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

        public bool SlotTaken(Item item, short slot = -1)
        {
            return GetSlots(item.InventoryTab).ContainsKey(slot < 0 ? item.Slot : slot);
        }

        private Dictionary<short, long> GetSlots(InventoryTab tab)
        {
            return SlotMaps[(int) tab];
        }
    }
}
