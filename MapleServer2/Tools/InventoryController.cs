using System;
using System.Linq;
using Maple2Storage.Types;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Tools
{
    public class InventoryController
    {
        // Adds item
        public static void Add(GameSession session, Item item, bool isNew)
        {
            // Checks if item is stackable or not
            if (item.StackLimit > 1)
            {
                foreach (Item i in session.Player.Inventory.Items.Values)
                {
                    // Checks to see if item exists in database (dictionary)
                    if (i.Id != item.Id || i.Amount >= i.StackLimit || i.Rarity != item.Rarity)
                    {
                        continue;
                    }
                    // Updates inventory for item amount overflow
                    if ((i.Amount + item.Amount) > i.StackLimit)
                    {
                        int added = i.StackLimit - i.Amount;
                        item.Amount -= added;
                        i.Amount = i.StackLimit;
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

                if (!session.Player.Inventory.Add(item)) // Adds item into internal database
                {
                    return;
                }
                session.Send(ItemInventoryPacket.Add(item)); // Sends packet to add item clientside
                if (isNew)
                {
                    session.Send(ItemInventoryPacket.MarkItemNew(item, item.Amount)); // Marks Item as New
                }
                return;
            }

            if (item.Amount == 1)
            {
                if (!session.Player.Inventory.Add(item))
                {
                    return;
                }

                session.Send(ItemInventoryPacket.Add(item));
                if (isNew)
                {
                    session.Send(ItemInventoryPacket.MarkItemNew(item, item.Amount));
                }
                return;
            }

            for (int i = 0; i < item.Amount; i++)
            {
                Item newItem = new Item(item)
                {
                    Amount = 1,
                    Uid = 0
                };
                newItem.Uid = DatabaseManager.AddItem(newItem);

                if (!session.Player.Inventory.Add(newItem))
                {
                    continue;
                }

                session.Send(ItemInventoryPacket.Add(newItem));
                if (isNew)
                {
                    session.Send(ItemInventoryPacket.MarkItemNew(newItem, newItem.Amount));
                }
            }
        }

        // Removes item based on quantity
        public static void Consume(GameSession session, long uid, int amount)
        {
            Item item = session.Player.Inventory.Items[uid];
            if (amount > item.Amount)
            {
                return;
            }
            if (amount == item.Amount)
            {
                Remove(session, uid, out Item _);
                return;
            }

            item.Amount -= amount;
            session.Send(ItemInventoryPacket.Update(uid, item.Amount));
            return;
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
            if (!session.Player.Inventory.Add(item)) // Adds item into internal database
            {
                return;
            }
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
                    DatabaseManager.Update(session.Player.Inventory.Items.Values.First(x => x.Uid == uid));
                }
                else // Removes item
                {
                    session.Send(ItemInventoryPacket.Remove(uid));
                    DatabaseManager.Delete(droppedItem);
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
                DatabaseManager.Delete(droppedItem);

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
            Inventory inventory = session.Player.Inventory;
            session.Send(ItemInventoryPacket.ResetTab(tab));
            session.Send(ItemInventoryPacket.LoadTab(tab, inventory.ExtraSize[tab]));
            session.Send(ItemInventoryPacket.LoadItem(inventory.Items.Values.Where(x => x.InventoryTab == tab).ToList()));
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
        public static void Split()
        {

        }

        public static void ExpandInventory(GameSession session, InventoryTab tab)
        {
            Inventory inventory = session.Player.Inventory;
            long meretPrice = 390;
            short expansionAmount = 6;

            if (session.Player.Account.RemoveMerets(meretPrice))
            {
                inventory.ExtraSize[tab] += expansionAmount;
                session.Send(ItemInventoryPacket.LoadTab(tab, inventory.ExtraSize[tab]));
                session.Send(ItemInventoryPacket.Expand());
            }
        }
    }
}
