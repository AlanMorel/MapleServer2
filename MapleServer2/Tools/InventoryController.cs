using System;
using Maple2Storage.Types;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;

public class InventoryController
{
    // Adds item
    public static void Add(GameSession session, Item item, bool isNew)
    {
        // Checks if item is stackable or not
        if (item.SlotMax > 1)
        {
            foreach (Item i in session.Player.Inventory.Items.Values)
            {
                // Checks to see if item exists in database (dictionary)
                if (i.Id != item.Id || i.Amount >= i.SlotMax)
                {
                    continue;
                }
                // Updates inventory for item amount overflow
                if ((i.Amount + item.Amount) > i.SlotMax)
                {
                    int added = i.SlotMax - i.Amount;
                    item.Amount -= added;
                    i.Amount = i.SlotMax;
                    session.Send(ItemInventoryPacket.Update(i.Uid, i.Amount));
                    session.Send(ItemInventoryPacket.MarkItemNew(i, added));
                }
                // Updates item amount
                else
                {
                    i.Amount += item.Amount;
                    session.Send(ItemInventoryPacket.Update(i.Uid, i.Amount));
                    session.Send(ItemInventoryPacket.MarkItemNew(i, item.Amount));
                    return;
                }
            }
        }
        session.Player.Inventory.Add(item); // Adds item into internal database
        session.Send(ItemInventoryPacket.Add(item)); // Sends packet to add item clientside
        if (isNew)
        {
            session.Send(ItemInventoryPacket.MarkItemNew(item, item.Amount)); // Marks Item as New
        }
    }

    // Removes Item from inventory by reference
    public static bool Remove(GameSession session, long uid, out Item item)
    {
        int amountRemoved = session.Player.Inventory.Remove(uid, out item);

        if (amountRemoved == -1)
        {
            return false;
        }

        session.Send(ItemInventoryPacket.Remove(uid));
        return true;
    }

    // Picks up item
    public static void PickUp(GameSession session, Item item)
    {
        session.Player.Inventory.Add(item); // Adds item into internal database
        session.Send(ItemInventoryPacket.Add(item)); // Sends packet to add item clientside
    }

    // Drops item with option to drop bound items
    public static void DropItem(GameSession session, long uid, int amount, bool isbound)
    {
        if (!isbound) // Drops Item
        {
            int remaining = session.Player.Inventory.Remove(uid, out Item droppedItem, amount); // Returns remaining amount of item
            if (remaining < 0)
            {
                return; // Removal failed
            }
            else if (remaining > 0) // Updates item
            {
                session.Send(ItemInventoryPacket.Update(uid, remaining));
            }
            else // Removes item
            {
                session.Send(ItemInventoryPacket.Remove(uid));
            }
            session.FieldManager.AddItem(session, droppedItem); // Drops item onto floor
        }
        else // Drops bound item.
        {
            if (session.Player.Inventory.Remove(uid, out Item droppedItem) != 0)
            {
                return; // Removal from inventory failed
            }
            session.Send(ItemInventoryPacket.Remove(uid));

            // Allow dropping bound items for now
            session.FieldManager.AddItem(session, droppedItem);
        }
    }

    // Sorts inventory items
    public static void SortInventory(GameSession session, Inventory inventory, InventoryTab tab)
    {
        inventory.Sort(tab);
        session.Send(ItemInventoryPacket.ResetTab(tab));
        session.Send(ItemInventoryPacket.LoadItemsToTab(tab, inventory.GetItems(tab)));
    }

    // Loads a Inventory Tab
    public static void LoadInventoryTab(GameSession session, InventoryTab tab)
    {
        session.Send(ItemInventoryPacket.ResetTab(tab));
        session.Send(ItemInventoryPacket.LoadTab(tab));
    }

    // Moves Item to destination slot
    public static void MoveItem(GameSession session, long uid, short dstSlot)
    {
        Tuple<long, short> srcSlot = session.Player.Inventory.Move(uid, dstSlot);

        if (srcSlot == null)
        {
            return;
        }
        session.Send(ItemInventoryPacket.Move(srcSlot.Item1, srcSlot.Item2, uid, dstSlot));
    }

    // Todo: implement when storage and trade is implemented
    public static void split(GameSession session, Item item)
    {

    }

    // Updates item information
    public static void Update(GameSession session, long uid, int amount)
    {
        if ((GetItemAmount(session, uid) + amount) >= GetItemMax(session, uid))
        {
            session.Player.Inventory.Items[uid].Amount = session.Player.Inventory.Items[uid].SlotMax;
            session.Send(ItemInventoryPacket.Update(uid, session.Player.Inventory.Items[uid].SlotMax));
        }
        session.Player.Inventory.Items[uid].Amount = amount;
        session.Send(ItemInventoryPacket.Update(uid, amount));
    }

    private static int GetItemAmount(GameSession session, long uid)
    {
        return session.Player.Inventory.Items[uid].Amount;
    }

    private static int GetItemMax(GameSession session, long uid)
    {
        return session.Player.Inventory.Items[uid].SlotMax;
    }
}
