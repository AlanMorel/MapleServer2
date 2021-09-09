using Maple2Storage.Enums;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class BankInventory
    {
        public readonly long Id;
        private readonly int DEFAULT_SIZE = 36;
        public int ExtraSize;
        public Currency Mesos;

        public Item[] Items = new Item[36];

        public BankInventory()
        {
            Mesos = new Currency(CurrencyType.BankMesos, 0);
            Id = DatabaseManager.BankInventories.Insert(this);
        }

        public BankInventory(long id, int extraSize, List<Item> items, long bankMesos)
        {
            Id = id;
            ExtraSize = extraSize;
            Items = new Item[DEFAULT_SIZE + ExtraSize];
            Mesos = new Currency(CurrencyType.BankMesos, bankMesos);
            for (int i = 0; i < items.Count; i++)
            {
                Item item = items[i];
                item.SetMetadataValues();
                Items[i] = item;
            }
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
                    item.Slot = slot;
                    Items[slot] = item;
                    session.Send(StorageInventoryPacket.Add(item));
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
                    item.Slot = slot;
                    Items[slot] = item;
                    session.Send(StorageInventoryPacket.Add(item));
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
                session.Send(StorageInventoryPacket.Remove(uid));
                return true;
            }

            if (outItem.TrySplit(amount, out Item splitItem))
            {
                outItem.Amount -= amount;
                outItem.Slot = slot;
                session.Send(StorageInventoryPacket.Add(outItem));
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
            session.Send(StorageInventoryPacket.Move(srcUid, srcSlot, dstUid, dstSlot));
        }

        public void LoadItems(GameSession session)
        {
            session.Send(StorageInventoryPacket.LoadItems(Items));
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
            session.Send(StorageInventoryPacket.Sort(Items));
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
            session.Send(StorageInventoryPacket.Update());
            session.Send(StorageInventoryPacket.Expand(ExtraSize));
            session.Send(StorageInventoryPacket.ExpandAnim());
            session.Send(StorageInventoryPacket.UpdateMesos(Mesos.Amount));
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
