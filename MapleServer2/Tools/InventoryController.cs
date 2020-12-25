using System;
using MapleServer2.Types;
using MapleServer2.Packets;
using MaplePacketLib2.Tools;
using MapleServer2.Servers.Game;
using Maple2Storage.Types;

public class InventoryController
{
    // Adds item
    public static void Add(GameSession session, Item item, Boolean isNew)
    {
        // Checks if item is stackable or not.
        if (item.SlotMax > 1)
        {
            foreach (Item i in session.Player.Inventory.Items.Values)
            {
                // Checks to see if item exists in database (dictionary)
                if (i.Id != item.Id || i.Amount >= i.SlotMax)
                {
                    continue;
                }
                // Updates inventory for item amount overflow.
                if ((i.Amount + item.Amount) > i.SlotMax)
                {
                    item.Amount = item.Amount - (i.SlotMax - i.Amount);
                    i.Amount = i.SlotMax;
                    session.Send(ItemInventoryPacket.Update(i.Uid, i.Amount));
                }
                // Updates item amount
                else
                {
                    i.Amount = i.Amount + item.Amount;
                    session.Send(ItemInventoryPacket.Update(i.Uid, i.Amount));
                    return;
                }
            }
        }
        session.Player.Inventory.Add(item); // adds item into internal database
        session.Send(ItemInventoryPacket.Add(item)); // sends packet to add item clientside.

        if (isNew)
        {
            session.Send(ItemInventoryPacket.MarkItemNew(item, item.Amount)); // Marks Item as New
        }
    }

    // Removes Item from inventory by reference
    public static void Remove(GameSession session, out Item item, long uid)
    {
        session.Player.Inventory.Remove(uid, out item);
        session.Send(ItemInventoryPacket.Remove(uid));
    }

    // Removes Item from inventory
    public static void Remove(GameSession session, Item item, long uid)
    {
        session.Player.Inventory.Remove(uid, out item);
        session.Send(ItemInventoryPacket.Remove(uid));
    }

    // Picks up item
    public static void PickUp(GameSession session, Item item)
    {
        session.Player.Inventory.Add(item); // adds item into internal database
        session.Send(ItemInventoryPacket.Add(item)); // sends packet to add item clientside.
    }

    // Drops item with option to drop bound items.
    public static void DropItem(GameSession session, PacketReader packet, Boolean isbound)
    {
        if (!isbound) // Drops Item.
        {
            long uid = packet.ReadLong(); // Grabs incoming item packet uid
            int amount = packet.ReadInt(); // Grabs incoming item packet amount
            int remaining = session.Player.Inventory.Remove(uid, out Item droppedItem, amount); // returns remaining amount of item.
            if (remaining < 0)
            {
                return; // Removal failed
            }
            // Updates item amount and removes item.
            session.Send(remaining > 0
                            ? ItemInventoryPacket.Update(uid, remaining)
                            : ItemInventoryPacket.Remove(uid));
            session.FieldManager.AddItem(session, droppedItem); // Drops item onto floor.
        }
        else // Drops bound item.
        {
            long uid = packet.ReadLong(); // Grabs incoming item packet uid
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
    public static void SortInventory(GameSession session, Inventory inventory, InventoryType tab)
    {
        inventory.Sort(tab);
        session.Send(ItemInventoryPacket.ResetTab(tab));
        session.Send(ItemInventoryPacket.LoadItemsToTab(tab, inventory.GetItems(tab)));
    }

    // Loads all inventory tabs
    public static void LoadInventoryTabs(GameSession session, InventoryType tab)
    {
        session.Send(ItemInventoryPacket.ResetTab(tab));
        session.Send(ItemInventoryPacket.LoadTab(tab));
    }

    // Moves Item to destination slot
    public static void MoveItem(GameSession session, PacketReader packet)
    {
        long uid = packet.ReadLong();
        short dstSlot = packet.ReadShort();
        Tuple<long, short> srcSlot = session.Player.Inventory.Move(uid, dstSlot);

        if (srcSlot == null)
        {
            return;
        }
        session.Send(ItemInventoryPacket.Move(srcSlot.Item1, srcSlot.Item2, uid, dstSlot));
    }

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
