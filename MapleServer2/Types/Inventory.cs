using System.Diagnostics;
using Maple2Storage.Enums;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Serilog;

// TODO: make this class thread safe?
namespace MapleServer2.Types;

public sealed class Inventory : IInventory
{
    private static readonly ILogger Logger = Log.Logger.ForContext<Inventory>();

    public long Id { get; }

    // This contains ALL inventory Items regardless of tab
    private readonly Dictionary<long, Item> Items;
    public Dictionary<ItemSlot, Item> Equips { get; }
    public Dictionary<ItemSlot, Item> Cosmetics { get; }
    public Item[] Badges { get; }
    public Item[] LapenshardStorage { get; }

    // Map of Slot to Uid for each inventory
    private readonly Dictionary<short, long>[] SlotMaps;

    private readonly Dictionary<InventoryTab, short> DefaultSize = new()
    {
        { InventoryTab.Gear, 48 },
        { InventoryTab.Outfit, 150 },
        { InventoryTab.Mount, 48 },
        { InventoryTab.Catalyst, 48 },
        { InventoryTab.FishingMusic, 48 },
        { InventoryTab.Quest, 48 },
        { InventoryTab.Gemstone, 48 },
        { InventoryTab.Misc, 84 },
        { InventoryTab.LifeSkill, 126 },
        { InventoryTab.Pets, 60 },
        { InventoryTab.Consumable, 84 },
        { InventoryTab.Currency, 48 },
        { InventoryTab.Badge, 60 },
        { InventoryTab.Lapenshard, 48 },
        { InventoryTab.Fragment, 48 }
    };

    public Dictionary<InventoryTab, short> ExtraSize { get; } = new()
    {
        { InventoryTab.Gear, 0 },
        { InventoryTab.Outfit, 0 },
        { InventoryTab.Mount, 0 },
        { InventoryTab.Catalyst, 0 },
        { InventoryTab.FishingMusic, 0 },
        { InventoryTab.Quest, 0 },
        { InventoryTab.Gemstone, 0 },
        { InventoryTab.Misc, 0 },
        { InventoryTab.LifeSkill, 0 },
        { InventoryTab.Pets, 0 },
        { InventoryTab.Consumable, 0 },
        { InventoryTab.Currency, 0 },
        { InventoryTab.Badge, 0 },
        { InventoryTab.Lapenshard, 0 },
        { InventoryTab.Fragment, 0 }
    };

    // Only use to share information between handler functions. Should always be empty
    public Dictionary<long, Item> TemporaryStorage { get; } = new();

    #region Constructors
    public Inventory(bool addToDatabase)
    {
        Equips = new();
        Cosmetics = new();
        Badges = new Item[12];
        Items = new();
        LapenshardStorage = new Item[6];

        byte maxTabs = Enum.GetValues(typeof(InventoryTab)).Cast<byte>().Max();
        SlotMaps = new Dictionary<short, long>[maxTabs + 1];

        for (byte i = 0; i <= maxTabs; i++)
        {
            SlotMaps[i] = new();
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
                switch (item.InventoryTab)
                {
                    case InventoryTab.Outfit:
                        if (!Cosmetics.TryAdd(item.ItemSlot, item))
                        {
                            Logger.Error("Failed to add item {id} to inventory {id}, slot {itemSlot} was already taken.", item.Id, Id, item.ItemSlot);
                        }

                        continue;
                    case InventoryTab.Badge:
                        Badges[badgeIndex++] = item;
                        continue;
                    case InventoryTab.Lapenshard:
                        LapenshardStorage[item.Slot] = item;
                        continue;
                    case InventoryTab.Gear:
                        if (!Equips.TryAdd(item.ItemSlot, item))
                        {
                            Logger.Error("Failed to add item {id} to inventory {id}, slot {itemSlot} was already taken.", item.Id, Id, item.ItemSlot);
                        }

                        continue;
                }
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
            case ItemType.Medal:
                session.Player.Account.AddMedal(session, item);
                return;
        }

        if (item.TransferFlag.HasFlag(ItemTransferFlag.Binds) &&
            (item.TransferType is TransferType.BindOnLoot or TransferType.BindOnTrade))
        {
            item.OwnerCharacterId = session.Player.CharacterId;
            item.OwnerAccountId = session.Player.AccountId;
            item.OwnerCharacterName = session.Player.Name;
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
                if (existingItem.Amount + item.Amount <= existingItem.StackLimit)
                {
                    existingItem.Amount += item.Amount;

                    DatabaseManager.Items.Delete(item.Uid);

                    session.Send(ItemInventoryPacket.UpdateAmount(existingItem.Uid, existingItem.Amount));
                    if (isNew)
                    {
                        session.Send(ItemInventoryPacket.MarkItemNew(existingItem, item.Amount));
                    }
                    return;
                }

                // Updates inventory for item amount overflow
                int added = existingItem.StackLimit - existingItem.Amount;
                item.Amount -= added;
                existingItem.Amount = existingItem.StackLimit;

                session.Send(ItemInventoryPacket.UpdateAmount(existingItem.Uid, existingItem.Amount));
                if (isNew)
                {
                    session.Send(ItemInventoryPacket.MarkItemNew(existingItem, added));
                }
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
            Item newItem = new(item)
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

        if (amount == item.Amount || item.Amount - amount <= 0)
        {
            RemoveItem(session, uid, out Item _);
            return;
        }

        item.Amount -= amount;
        session.Send(ItemInventoryPacket.UpdateAmount(uid, item.Amount));
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

    public void DropItem(GameSession session, Item item, int amount)
    {
        // Drops item not bound
        if (!item.TransferFlag.HasFlag(ItemTransferFlag.Tradeable))
        {
            if (!session.Player.Inventory.RemoveItem(session, item.Uid, out item))
            {
                return; // Removal from inventory failed
            }

            session.Send(ItemInventoryPacket.Remove(item.Uid));
            DatabaseManager.Items.Delete(item.Uid);
            return;
        }

        // Drops tradeable items
        int remaining = Remove(item.Uid, out Item droppedItem, amount); // Returns remaining amount of item
        switch (remaining)
        {
            case < 0:
                return; // Removal failed
            case > 0: // Updates item amount
                session.Send(ItemInventoryPacket.UpdateAmount(item.Uid, remaining));
                DatabaseManager.Items.Update(Items[item.Uid]);
                break;
            default: // Removes item
                session.Send(ItemInventoryPacket.Remove(item.Uid));
                break;
        }

        session.FieldManager.AddItem(session, droppedItem); // Drops item onto floor
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
                if (item.Amount + srcItem.Amount <= item.StackLimit)
                {
                    item.Amount += srcItem.Amount;

                    DatabaseManager.Items.Delete(srcItem.Uid);

                    session.Send(ItemInventoryPacket.UpdateAmount(item.Uid, item.Amount));
                    session.Send(ItemInventoryPacket.Remove(srcItem.Uid));
                    return;
                }

                // Updates inventory for item amount overflow
                int added = item.StackLimit - item.Amount;
                srcItem.Amount -= added;
                item.Amount = item.StackLimit;

                session.Send(ItemInventoryPacket.UpdateAmount(srcItem.Uid, srcItem.Amount));
                session.Send(ItemInventoryPacket.UpdateAmount(item.Uid, item.Amount));
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

    public void ConsumeByTag(GameSession session, string tag, int amount)
    {
        IReadOnlyCollection<Item> ingredientTotal = GetAllByTag(tag);
        foreach (Item item in ingredientTotal)
        {
            if (item.Amount >= amount)
            {
                ConsumeItem(session, item.Uid, amount);
                break;
            }

            amount -= item.Amount;
            ConsumeItem(session, item.Uid, item.Amount);
        }
    }

    public bool HasItem(long uid) => Items.ContainsKey(uid);

    public bool HasItem(int id) => Items.Values.Any(i => i.Id == id);

    public Item GetByUid(long uid) => Items[uid];

    public Item GetById(int id) => Items.Values.FirstOrDefault(x => x.Id == id);

    public Item GetFromInventoryOrEquipped(GameSession session, long uid)
    {
        if (HasItem(uid))
        {
            return GetByUid(uid);
        }
        return session.Player.ItemIsEquipped(uid) ? session.Player.GetEquippedItem(uid) : null;
    }

    public IEnumerable<Item> GetItemsNotNull() => Items.Values.Where(x => x != null).ToArray();

    public IReadOnlyCollection<Item> GetAllById(int id) => Items.Values.Where(x => x.Id == id).ToArray();

    public IReadOnlyCollection<Item> GetAllByTag(string tag) => Items.Values.Where(i => i.Tag == tag).ToArray();

    public IEnumerable<Item> GetAllByFunctionId(int functionId) =>
        Items.Values.Where(x => x.Function.Id == functionId).ToArray();

    // Replaces an existing item with an updated copy of itself
    public bool Replace(Item item)
    {
        if (!HasItem(item.Uid))
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
        List<Item> tabItems = slots.Select(kvp => Items[kvp.Value]).ToList();

        // group items by item id and sum the amount, return a new list of items with updated amount (ty gh copilot)
        List<Item> groupedItems = tabItems.Where(x => x.StackLimit > 1).GroupBy(x => x.Id).Select(x => new Item(x.First())
        {
            Amount = x.Sum(y => y.Amount)
        }).ToList();

        // Add items that can't be grouped
        groupedItems.AddRange(tabItems.Where(x => x.StackLimit == 1));

        // Sort by item id
        groupedItems.Sort((x, y) => x.Id.CompareTo(y.Id));

        // Update the slot mapping
        slots.Clear();
        short slotId = 0;

        // Items that overflow stack limit
        List<Item> itemList = new();

        // Delete items that got grouped
        foreach (Item oldItem in tabItems)
        {
            Item newItem = groupedItems.FirstOrDefault(x => x.Uid == oldItem.Uid);
            if (newItem is null)
            {
                Items.Remove(oldItem.Uid);
                DatabaseManager.Items.Delete(oldItem.Uid);
                continue;
            }

            if (newItem.Amount > newItem.StackLimit)
            {
                // how many times can we split the item?
                int splitAmount = newItem.Amount / newItem.StackLimit;
                // how many items are left over?
                int remainder = newItem.Amount % newItem.StackLimit;

                // split the item
                for (int i = 0; i < splitAmount; i++)
                {
                    if (!newItem.TrySplit(newItem.StackLimit, out Item splitItem))
                    {
                        continue;
                    }

                    itemList.Add(splitItem);
                }

                // Delete the original item if remainder is 0
                if (remainder <= 0)
                {
                    Items.Remove(oldItem.Uid);
                    DatabaseManager.Items.Delete(oldItem.Uid);
                    continue;
                }

                // set the remainder
                newItem.Amount = remainder;
            }

            // Update the slot mapping
            slots.Add(slotId++, newItem.Uid);
            newItem.Slot = slotId;

            // Update the item
            Items[newItem.Uid] = newItem;
            DatabaseManager.Items.Update(newItem);
        }

        foreach (Item item in itemList)
        {
            AddNewItem(session, item, false);
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
        long meretPrice = long.Parse(ConstantsMetadataStorage.GetConstant("InventoryExpandPrice1Row"));
        const short ExpansionAmount = 6;

        if (!session.Player.Account.RemoveMerets(meretPrice))
        {
            return;
        }

        ExtraSize[tab] += ExpansionAmount;
        session.Send(ItemInventoryPacket.LoadTab(tab, ExtraSize[tab]));
        session.Send(ItemInventoryPacket.Expand());
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

    public IReadOnlyCollection<Item> GetItems(InventoryTab tab)
    {
        return GetSlots(tab).Select(kvp => Items[kvp.Value]).ToArray();
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

        if (amount >= 0 && item.Amount != amount)
        {
            return item.TrySplit(amount, out removedItem) ? item.Amount : -1;
        }

        // Remove All
        if (!RemoveInternal(uid, out removedItem))
        {
            return -1;
        }

        return 0;
    }

    // This REQUIRES item.Slot to be set appropriately
    private void AddInternal(Item item)
    {
        Debug.Assert(!HasItem(item.Uid), "Error adding an item that already exists");
        Items[item.Uid] = item;

        Debug.Assert(!GetSlots(item.InventoryTab).ContainsKey(item.Slot), "Error adding item to slot that is already taken.");
        GetSlots(item.InventoryTab)[item.Slot] = item.Uid;
    }

    private bool RemoveInternal(long uid, out Item item)
    {
        return Items.Remove(uid, out item) && GetSlots(item.InventoryTab).Remove(item.Slot);
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

        if (item.Ugc?.Type is UgcType.Furniture)
        {
            _ = home.AddWarehouseUgcItem(session, item);
            return;
        }

        _ = home.AddWarehouseItem(session, item.Id, item.Amount, item);
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
