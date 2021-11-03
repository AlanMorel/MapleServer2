using System.Collections.Immutable;
using System.Diagnostics;
using Maple2Storage.Enums;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

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
        public Item[] Badges;

        // Map of Slot to Uid for each inventory
        private readonly Dictionary<short, long>[] SlotMaps;

        public readonly Dictionary<InventoryTab, short> DefaultSize = new Dictionary<InventoryTab, short> {
            { InventoryTab.Gear, 48}, { InventoryTab.Outfit, 150}, { InventoryTab.Mount, 48}, { InventoryTab.Catalyst, 48},
            { InventoryTab.FishingMusic, 48}, { InventoryTab.Quest, 48}, { InventoryTab.Gemstone, 48}, { InventoryTab.Misc, 84},
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

        #region Constructors

        public Inventory(bool addToDatabase)
        {
            Equips = new Dictionary<ItemSlot, Item>();
            Cosmetics = new Dictionary<ItemSlot, Item>();
            Badges = new Item[12];
            Items = new Dictionary<long, Item>();

            byte maxTabs = Enum.GetValues(typeof(InventoryTab)).Cast<byte>().Max();
            SlotMaps = new Dictionary<short, long>[maxTabs + 1];

            for (byte i = 0; i <= maxTabs; i++)
            {
                SlotMaps[i] = new Dictionary<short, long>();
            }

            if (addToDatabase)
            {
                Id = DatabaseManager.Inventories.Insert(this);
            }
        }

        public Inventory(long id, Dictionary<InventoryTab, short> extraSize, List<Item> items) : this(false)
        {
            Id = id;
            ExtraSize = extraSize;
            int badgeIndex = 0;

            foreach (Item item in items)
            {
                item.SetMetadataValues();
                if (item.IsEquipped)
                {
                    if (item.InventoryTab == InventoryTab.Outfit)
                    {
                        Cosmetics.Add(item.ItemSlot, item);
                        continue;
                    }

                    if (item.InventoryTab == InventoryTab.Badge)
                    {
                        Badges[badgeIndex++] = item;
                        continue;
                    }

                    Equips.Add(item.ItemSlot, item);
                    continue;
                }

                Add(item);
            }
        }

        #endregion

        #region Public Methods

        public void AddItem(GameSession session, Item item, bool isNew)
        {
            switch (item.Type)
            {
                case ItemType.Currency:
                    AddMoney(session, item);
                    return;
                case ItemType.Furnishing:
                    AddToWarehouse(session, item);
                    return;
            }

            // Checks if item is stackable or not
            if (item.StackLimit > 1)
            {
                // If item has slot defined, try to add to that slot
                if (item.Slot != -1 && !SlotTaken(item, item.Slot))
                {
                    AddNewItem(session, item, isNew);
                    return;
                }

                // If slot is occupied
                // Finds item in inventory with same id, rarity and a stack not full
                Item existingItem = Items.Values.FirstOrDefault(x => x.Id == item.Id && x.Amount < x.StackLimit && x.Rarity == item.Rarity);
                if (existingItem is not null)
                {
                    // Updates item amount
                    if ((existingItem.Amount + item.Amount) <= existingItem.StackLimit)
                    {
                        existingItem.Amount += item.Amount;

                        DatabaseManager.Items.Delete(item.Uid);

                        session.Send(ItemInventoryPacket.Update(existingItem.Uid, existingItem.Amount));
                        session.Send(ItemInventoryPacket.MarkItemNew(existingItem, item.Amount));
                        return;
                    }

                    // Updates inventory for item amount overflow
                    int added = existingItem.StackLimit - existingItem.Amount;
                    item.Amount -= added;
                    existingItem.Amount = existingItem.StackLimit;

                    session.Send(ItemInventoryPacket.Update(existingItem.Uid, existingItem.Amount));
                    session.Send(ItemInventoryPacket.MarkItemNew(existingItem, added));
                }

                // Add item to first free slot
                AddNewItem(session, item, isNew);
                return;
            }

            // If item is not stackable and amount is 1, add to inventory to next free slot
            if (item.Amount == 1)
            {
                AddNewItem(session, item, isNew);
                return;
            }

            // If item is not stackable and amount is greater than 1, add multiple times
            for (int i = 0; i < item.Amount; i++)
            {
                Item newItem = new Item(item)
                {
                    Amount = 1,
                    Uid = 0
                };
                newItem.Uid = DatabaseManager.Items.Insert(newItem);

                AddNewItem(session, newItem, isNew);
            }
        }

        public void ConsumeItem(GameSession session, long uid, int amount)
        {
            if (!Items.TryGetValue(uid, out Item item) || amount > item.Amount)
            {
                return;
            }

            if (amount == item.Amount)
            {
                RemoveItem(session, uid, out Item _);
                return;
            }

            item.Amount -= amount;
            session.Send(ItemInventoryPacket.Update(uid, item.Amount));
            return;
        }

        public bool RemoveItem(GameSession session, long uid, out Item item)
        {
            if (Remove(uid, out item) == -1)
            {
                return false;
            }

            session.Send(ItemInventoryPacket.Remove(uid));
            return true;
        }

        public void DropItem(GameSession session, long uid, int amount, bool isBound)
        {
            // Drops item not bound
            if (!isBound)
            {
                int remaining = Remove(uid, out Item droppedItem, amount); // Returns remaining amount of item
                if (remaining < 0)
                {
                    return; // Removal failed
                }

                // Updates item amount
                if (remaining > 0)
                {
                    session.Send(ItemInventoryPacket.Update(uid, remaining));
                    DatabaseManager.Items.Update(Items[uid]);
                }
                else // Removes item
                {
                    session.Send(ItemInventoryPacket.Remove(uid));
                }
                session.FieldManager.AddItem(session, droppedItem); // Drops item onto floor
                return;
            }

            // Drops bound item
            if (session.Player.Inventory.Remove(uid, out Item removedItem) != 0)
            {
                return; // Removal from inventory failed
            }
            session.Send(ItemInventoryPacket.Remove(uid));
            DatabaseManager.Items.Delete(removedItem.Uid);
        }

        public void MoveItem(GameSession session, long uid, short dstSlot)
        {
            if (!RemoveInternal(uid, out Item srcItem))
            {
                return;
            }

            short srcSlot = srcItem.Slot;

            if (SlotMaps[(int) srcItem.InventoryTab].TryGetValue(dstSlot, out long dstUid))
            {
                Item item = Items[dstUid];
                // If item is stackable and same id and rarity, try to increase the item amount instead of swapping slots
                if (item.Id == srcItem.Id && item.Amount < item.StackLimit && item.Rarity == srcItem.Rarity && item.StackLimit > 1)
                {
                    // Updates item amount
                    if ((item.Amount + srcItem.Amount) <= item.StackLimit)
                    {
                        item.Amount += srcItem.Amount;

                        DatabaseManager.Items.Delete(srcItem.Uid);

                        session.Send(ItemInventoryPacket.Update(item.Uid, item.Amount));
                        session.Send(ItemInventoryPacket.Remove(srcItem.Uid));
                        return;
                    }

                    // Updates inventory for item amount overflow
                    int added = item.StackLimit - item.Amount;
                    srcItem.Amount -= added;
                    item.Amount = item.StackLimit;

                    session.Send(ItemInventoryPacket.Update(srcItem.Uid, srcItem.Amount));
                    session.Send(ItemInventoryPacket.Update(item.Uid, item.Amount));
                    return;
                }
            }

            // Move dstItem to srcSlot if removed
            if (RemoveInternal(dstUid, out Item dstItem))
            {
                dstItem.Slot = srcSlot;
                AddInternal(dstItem);
            }

            // Move srcItem to dstSlot
            srcItem.Slot = dstSlot;
            AddInternal(srcItem);

            session.Send(ItemInventoryPacket.Move(dstUid, srcSlot, uid, dstSlot));
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

        public void SortInventory(GameSession session, InventoryTab tab)
        {
            // Get all items in tab
            Dictionary<short, long> slots = GetSlots(tab);
            IEnumerable<Item> tabItems = slots.Select(kvp => Items[kvp.Value]);

            // group items by item id and sum the amount, return a new list of items with updated amount (ty gh copilot)
            List<Item> groupedItems = tabItems.Where(x => x.StackLimit > 1).GroupBy(x => x.Id).Select(x => new Item(x.First())
            {
                Amount = x.Sum(y => y.Amount),
            }).ToList();

            // Add items that can't be grouped
            groupedItems.AddRange(tabItems.Where(x => x.StackLimit == 1));

            // Sort by item id
            groupedItems.Sort((x, y) => x.Id.CompareTo(y.Id));

            // Update the slot mapping
            slots.Clear();
            for (short i = 0; i < groupedItems.Count; i++)
            {
                groupedItems[i].Slot = i;
                slots[i] = groupedItems[i].Uid;
            }

            // Delete items that got grouped
            foreach (Item oldItem in tabItems)
            {
                Item newItem = groupedItems.FirstOrDefault(x => x.Uid == oldItem.Uid);
                if (newItem is null)
                {
                    Remove(oldItem.Uid, out _);
                    DatabaseManager.Items.Delete(oldItem.Uid);
                    continue;
                }

                Items[newItem.Uid] = newItem;
                DatabaseManager.Items.Update(newItem);
            }

            session.Send(ItemInventoryPacket.ResetTab(tab));
            session.Send(ItemInventoryPacket.LoadItemsToTab(tab, GetItems(tab)));
        }

        public void LoadInventoryTab(GameSession session, InventoryTab tab)
        {
            session.Send(ItemInventoryPacket.ResetTab(tab));
            session.Send(ItemInventoryPacket.LoadTab(tab, ExtraSize[tab]));
            session.Send(ItemInventoryPacket.LoadItem(GetItems(tab)));
        }

        public void ExpandInventory(GameSession session, InventoryTab tab)
        {
            long meretPrice = 390;
            short expansionAmount = 6;

            if (session.Player.Account.RemoveMerets(meretPrice))
            {
                ExtraSize[tab] += expansionAmount;
                session.Send(ItemInventoryPacket.LoadTab(tab, ExtraSize[tab]));
                session.Send(ItemInventoryPacket.Expand());
            }
        }

        public int GetFreeSlots(InventoryTab tab)
        {
            return DefaultSize[tab] + ExtraSize[tab] - GetSlots(tab).Count;
        }

        public bool CanHold(Item item, int amount = -1)
        {
            int remaining = amount > 0 ? amount : item.Amount;
            return CanHold(item.Id, remaining, item.InventoryTab);
        }

        public bool CanHold(int itemId, int amount)
        {
            return CanHold(itemId, amount, ItemMetadataStorage.GetTab(itemId));
        }

        public bool SlotTaken(Item item, short slot = -1)
        {
            return GetSlots(item.InventoryTab).ContainsKey(slot < 0 ? item.Slot : slot);
        }

        public ICollection<Item> GetItems(InventoryTab tab)
        {
            return GetSlots(tab).Select(kvp => Items[kvp.Value]).ToImmutableList();
        }

        #endregion

        #region Private Methods

        // TODO: precompute next free slot to avoid iteration on Add
        // Returns false if inventory is full
        private bool Add(Item item)
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
        private int Remove(long uid, out Item removedItem, int amount = -1)
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

        // This REQUIRES item.Slot to be set appropriately
        private void AddInternal(Item item)
        {
            Console.WriteLine($"Adding item uid {item.Uid}");
            Debug.Assert(!Items.ContainsKey(item.Uid), "Error adding an item that already exists");
            Items[item.Uid] = item;

            Debug.Assert(!GetSlots(item.InventoryTab).ContainsKey(item.Slot), "Error adding item to slot that is already taken.");
            GetSlots(item.InventoryTab)[item.Slot] = item.Uid;
        }

        private bool RemoveInternal(long uid, out Item item)
        {
            return Items.Remove(uid, out item) && GetSlots(item.InventoryTab).Remove(item.Slot);
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

        private Dictionary<short, long> GetSlots(InventoryTab tab)
        {
            return SlotMaps[(int) tab];
        }

        private bool CanHold(int itemId, int amount, InventoryTab tab)
        {
            if (GetFreeSlots(tab) > 0)
            {
                return true;
            }

            foreach (Item i in Items.Values.Where(x => x.InventoryTab == tab && x.Id == itemId && x.StackLimit > 0))
            {
                int available = i.StackLimit - i.Amount;
                amount -= available;
                if (amount <= 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void AddNewItem(GameSession session, Item item, bool isNew)
        {
            if (!Add(item)) // Adds item into internal database
            {
                return;
            }

            session.Send(ItemInventoryPacket.Add(item)); // Sends packet to add item clientside
            if (isNew)
            {
                session.Send(ItemInventoryPacket.MarkItemNew(item, item.Amount)); // Marks Item as New
            }
        }

        private static void AddToWarehouse(GameSession session, Item item)
        {
            if (session.Player.Account.Home == null)
            {
                return;
            }

            Home home = GameServer.HomeManager.GetHomeById(session.Player.Account.Home.Id);
            if (home == null)
            {
                return;
            }

            _ = home.AddWarehouseItem(session, item.Id, item.Amount, item);
            session.Send(WarehouseInventoryPacket.GainItemMessage(item, item.Amount));
        }

        private static void AddMoney(GameSession session, Item item)
        {
            switch (item.Id)
            {
                case 90000001: // Meso
                    session.Player.Wallet.Meso.Modify(item.Amount);
                    return;
                case 90000006: // Valor Token
                    session.Player.Wallet.ValorToken.Modify(item.Amount);
                    return;
                case 90000004: // Meret
                case 90000011: // Meret
                case 90000015: // Meret
                case 90000016: // Meret
                    session.Player.Account.Meret.Modify(item.Amount);
                    return;
                case 90000013: // Rue
                    session.Player.Wallet.Rue.Modify(item.Amount);
                    return;
                case 90000014: // Havi
                    session.Player.Wallet.HaviFruit.Modify(item.Amount);
                    return;
                case 90000017: // Treva
                    session.Player.Wallet.Treva.Modify(item.Amount);
                    return;
                case 90000021: // Guild Funds
                    if (session.Player.Guild == null)
                    {
                        return;
                    }

                    session.Player.Guild.Funds += item.Amount;
                    session.Player.Guild.BroadcastPacketGuild(GuildPacket.UpdateGuildFunds(session.Player.Guild.Funds));
                    DatabaseManager.Guilds.Update(session.Player.Guild);
                    return;
            }
        }

        #endregion
    }
}
