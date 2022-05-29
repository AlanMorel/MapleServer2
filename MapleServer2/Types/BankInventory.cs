using Maple2Storage.Enums;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class BankInventory
{
    public readonly long Id;
    private readonly int DEFAULT_SIZE = 36;
    public int ExtraSize;
    public Currency Mesos;

    public Item[] Items = new Item[36];

    public BankInventory()
    {
        Mesos = new(CurrencyType.BankMesos, 0);
        Id = DatabaseManager.BankInventories.Insert(this);
    }

    public BankInventory(long id, int extraSize, List<Item> items, long bankMesos)
    {
        Id = id;
        ExtraSize = extraSize;
        Items = new Item[DEFAULT_SIZE + ExtraSize];
        Mesos = new(CurrencyType.BankMesos, bankMesos);

        for (int i = 0; i < items.Count; i++)
        {
            Item item = items[i];
            item.SetMetadataValues();
            Items[i] = item;
        }
    }

    public void Add(GameSession session, long uid, int amount, short slot)
    {
        Item item = session.Player.Inventory.GetByUid(uid);

        if (ItemMetadataStorage.IsTradeDisabledWithinAccount(item.Id))
        {
            return;
        }

        if (amount < item.Amount)
        {
            item.TrySplit(amount, out Item splitItem);
            session.Send(ItemInventoryPacket.UpdateAmount(uid, item.Amount));
            item = splitItem;
        }
        else
        {
            session.Player.Inventory.RemoveItem(session, uid, out Item removedItem);
            item = removedItem;
        }

        // If slot is free, add item to it
        if (Items[slot] is null)
        {
            item.Slot = slot;
            Items[slot] = item;
            session.Send(StorageInventoryPacket.Add(item));
            return;
        }

        // Find first item with the same id, if true update the amount
        Item existingItem = Items.FirstOrDefault(x => x is not null && x.Id == item.Id && x.Rarity == item.Rarity);
        if (existingItem is not null)
        {
            if (existingItem.Amount + item.Amount <= existingItem.StackLimit)
            {
                existingItem.Amount += item.Amount;
                session.Send(StorageInventoryPacket.UpdateItem(existingItem));
                return;
            }

            existingItem.Amount = existingItem.StackLimit;
            item.Amount = existingItem.Amount + item.Amount - existingItem.StackLimit;
            session.Send(StorageInventoryPacket.UpdateItem(existingItem));
        }

        // Find first free slot
        for (short i = 0; i < Items.Length; i++)
        {
            if (Items[i] is not null)
            {
                continue;
            }

            item.Slot = i;
            Items[i] = item;
            session.Send(StorageInventoryPacket.Add(item));
            return;
        }
    }

    public bool Remove(GameSession session, long uid, int amount, out Item outItem)
    {
        outItem = null;
        if (!session.Player.Account.BankInventory.Items.Any(x => x is not null && x.Uid == uid))
        {
            return false;
        }

        int outItemIndex = Array.FindIndex(Items, 0, Items.Length, x => x is not null && x.Uid == uid);
        outItem = Items[outItemIndex];
        if (amount >= outItem.Amount)
        {
            Items[outItemIndex] = null;
            session.Send(StorageInventoryPacket.Remove(uid));
            return true;
        }

        if (outItem.TrySplit(amount, out Item splitItem))
        {
            session.Send(StorageInventoryPacket.UpdateItem(outItem));

            outItem = splitItem;
            return true;
        }

        return false;
    }

    public void Move(GameSession session, long dstUid, short dstSlot)
    {
        Item dstItem = Items[dstSlot];
        long srcUid = 0;
        short srcSlot = 0;

        if (dstItem is not null)
        {
            srcUid = dstItem.Uid;
            srcSlot = (short) Array.FindIndex(Items, 0, Items.Length, x => x is not null && x.Uid == dstUid);
            Item temp = Items[srcSlot];
            Items[srcSlot] = dstItem;
            Items[dstSlot] = temp;
        }
        else
        {
            short oldSlot = (short) Array.FindIndex(Items, 0, Items.Length, x => x is not null && x.Uid == dstUid);
            Items[dstSlot] = Items.FirstOrDefault(x => x is not null && x.Uid == dstUid);
            Items[oldSlot] = null;
        }

        session.Send(StorageInventoryPacket.Move(srcUid, srcSlot, dstUid, dstSlot));
    }

    public void LoadItems(GameSession session)
    {
        session.Send(StorageInventoryPacket.LoadItems(Items));
    }

    public void Sort(GameSession session)
    {
        IEnumerable<Item> items = Items.Where(x => x is not null);

        // group items by item id and sum the amount, return a new list of items with updated amount (ty gh copilot)
        List<Item> groupedItems = items.Where(x => x.StackLimit > 1).GroupBy(x => x.Id).Select(x => new Item(x.First())
        {
            Amount = x.Sum(y => y.Amount),
            BankInventoryId = Id
        }).ToList();

        // Add items that can't be grouped
        groupedItems.AddRange(items.Where(x => x.StackLimit == 1));

        // sort items by id
        groupedItems.Sort((x, y) => x.Id.CompareTo(y.Id));

        // Delete items that got grouped
        foreach (Item oldItem in items)
        {
            Item newItem = groupedItems.FirstOrDefault(x => x.Uid == oldItem.Uid);
            if (newItem is null)
            {
                DatabaseManager.Items.Delete(oldItem.Uid);
            }
        }

        Items = new Item[DEFAULT_SIZE + ExtraSize];
        for (short i = 0; i < groupedItems.Count; i++)
        {
            Item item = groupedItems[i];

            item.Slot = i;
            Items[i] = item;

            DatabaseManager.Items.Update(item);
        }

        session.Send(StorageInventoryPacket.Update());
        session.Send(StorageInventoryPacket.Sort(Items));
        session.Send(StorageInventoryPacket.ExpandAnim());
    }

    public void Expand(GameSession session)
    {
        long meretPrice = 330;
        int expansionAmount = 6;
        if (!session.Player.Account.RemoveMerets(meretPrice))
        {
            return;
        }

        ExtraSize += expansionAmount;
        session.Send(StorageInventoryPacket.Expand(ExtraSize));
        session.Send(StorageInventoryPacket.ExpandAnim());
        UpdateInventorySize();
    }

    public void LoadBank(GameSession session)
    {
        session.Send(StorageInventoryPacket.Expand(ExtraSize));
        session.Send(StorageInventoryPacket.Update());
        session.Send(StorageInventoryPacket.Expand(ExtraSize));
        session.Send(StorageInventoryPacket.UpdateMesos(Mesos.Amount));
        session.Send(StorageInventoryPacket.ItemCount((short) Items.Length));
        LoadItems(session);
    }

    private void UpdateInventorySize()
    {
        Item[] temp = Items;
        Items = new Item[DEFAULT_SIZE + ExtraSize];
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i] is null)
            {
                continue;
            }

            Items[i] = temp[i];
        }
    }
}
