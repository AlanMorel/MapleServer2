using System;
using System.Collections.Generic;
using System.Linq;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class BankInventory
    {
        public Item[] Items;
        private readonly int DEFAULT_SIZE = 36;
        public int ExtraSize;

        public BankInventory()
        {
            Items = new Item[DEFAULT_SIZE + ExtraSize];
            ExtraSize = 0;
        }

        public void Add(GameSession session, long uid, int amount, short slot)
        {
            Item item = session.Player.Inventory.Items[uid];

            if (amount < item.Amount)
            {
                item.TrySplit(amount, out Item splitItem);
                session.Send(ItemInventoryPacket.Update(uid, item.Amount));
                item = splitItem;
            }
            else
            {
                InventoryController.Remove(session, uid, out Item removedItem);
                item = removedItem;
            }

            if (slot >= 0)
            {
                if (Items[slot] == null)
                {
                    Items[slot] = item;
                    session.Send(StorageInventory.Add(item, slot));
                    return;
                }
                else
                {
                    slot = -1;
                }
            }

            if (slot == -1)
            {
                for (slot = 0; slot < Items.Length; slot++)
                {
                    if (Items[slot] != null)
                    {
                        continue;
                    }
                    Items[slot] = item;
                    session.Send(StorageInventory.Add(item, slot));
                    return;
                }
            }
        }

        public bool Remove(GameSession session, long uid, short slot, int amount, out Item outItem)
        {
            outItem = null;
            if (session.Player.Inventory.Items.ContainsKey(slot))
            {
                return false;
            }

            int outItemIndex = Array.FindIndex(Items, 0, Items.Length, x => x != null && x.Uid == uid);
            outItem = Items[outItemIndex];
            if (amount >= outItem.Amount)
            {
                Items[outItemIndex] = null;
                session.Send(StorageInventory.Remove(uid));
                return true;
            }

            if (outItem.TrySplit(amount, out Item splitItem))
            {
                outItem.Amount -= amount;
                session.Send(StorageInventory.Add(outItem, slot));
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

            if (dstItem != null)
            {
                srcUid = dstItem.Uid;
                srcSlot = (short) Array.FindIndex(Items, 0, Items.Length, x => x != null && x.Uid == dstUid);
                Item temp = Items[srcSlot];
                Items[srcSlot] = dstItem;
                Items[dstSlot] = temp;
            }
            else
            {
                short oldSlot = (short) Array.FindIndex(Items, 0, Items.Length, x => x != null && x.Uid == dstUid);
                Items[dstSlot] = Items.FirstOrDefault(x => x != null && x.Uid == dstUid);
                Items[oldSlot] = null;
            }
            session.Send(StorageInventory.Move(srcUid, srcSlot, dstUid, dstSlot));
        }

        public void LoadItems(GameSession session)
        {
            Items = new Item[DEFAULT_SIZE + ExtraSize];
            session.Send(StorageInventory.LoadItems(Items));
        }

        public void Sort(GameSession session)
        {
            List<Item> tempItems = Items.Where(x => x != null).ToList();
            tempItems.Sort((x, y) => x.Id.CompareTo(y.Id));
            Items = new Item[DEFAULT_SIZE + ExtraSize];
            for (int i = 0; i < tempItems.Count; i++)
            {
                Items[i] = tempItems[i];
            }
            session.Send(StorageInventory.Sort(Items));
        }

        public void Expand(GameSession session)
        {
            long meretPrice = 330;
            int expansionAmount = 6;
            if (!session.Player.Wallet.RemoveMerets(meretPrice))
            {
                return;
            }
            ExtraSize += expansionAmount;
            session.Send(StorageInventory.Expand(ExtraSize));
            session.Send(StorageInventory.ExpandAnim());
            UpdateInventorySize();
        }

        public void LoadBank(GameSession session)
        {
            session.Send(StorageInventory.Update());
            session.Send(StorageInventory.Expand(ExtraSize));
            session.Send(StorageInventory.ExpandAnim());
            session.Send(StorageInventory.UpdateMesos(session.Player.Wallet.Bank.Amount));
            LoadItems(session);
        }

        private void UpdateInventorySize()
        {
            Item[] temp = Items;
            Items = new Item[DEFAULT_SIZE + ExtraSize];
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] == null)
                {
                    continue;
                }
                Items[i] = temp[i];
            }
        }
    }
}
