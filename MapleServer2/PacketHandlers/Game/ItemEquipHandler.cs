using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class ItemEquipHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.ITEM_EQUIP;

    public ItemEquipHandler() : base() { }

    private enum ItemEquipMode : byte
    {
        Equip = 0,
        Unequip = 1
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        ItemEquipMode function = (ItemEquipMode) packet.ReadByte();

        switch (function)
        {
            case ItemEquipMode.Equip:
                HandleEquipItem(session, packet);
                break;
            case ItemEquipMode.Unequip:
                HandleUnequipItem(session, packet);
                break;
        }
    }

    private static void HandleEquipItem(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();
        string equipSlotStr = packet.ReadUnicodeString();
        if (!Enum.TryParse(equipSlotStr, out ItemSlot equipSlot))
        {
            Logger.Warn("Unknown equip slot: {equipSlotStr}", equipSlotStr);
            return;
        }

        // Remove the item from the users inventory
        Inventory inventory = session.Player.Inventory;
        inventory.RemoveItem(session, itemUid, out Item item);
        if (item == null)
        {
            return;
        }

        // Get correct equipped inventory
        Dictionary<ItemSlot, Item> equippedInventory = session.Player.GetEquippedInventory(item.InventoryTab);
        if (equippedInventory == null)
        {
            Logger.Warn("equippedInventory was null: {item.InventoryTab}", item.InventoryTab);
            return;
        }

        // Move previously equipped item back to inventory
        if (equippedInventory.Remove(equipSlot, out Item prevItem))
        {
            prevItem.Slot = item.Slot;
            prevItem.IsEquipped = false;
            inventory.AddItem(session, prevItem, false);
            session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.FieldPlayer, prevItem));

            if (prevItem.InventoryTab == InventoryTab.Gear)
            {
                DecreaseStats(session, prevItem);
            }
        }

        // Handle unequipping pants when equipping dresses
        // Handle unequipping off-hand when equipping two-handed weapons
        if (item.IsDress || item.IsTwoHand)
        {
            if (equippedInventory.Remove(item.IsDress ? ItemSlot.PA : ItemSlot.LH, out Item prevItem2))
            {
                prevItem2.Slot = -1;
                if (prevItem == null)
                {
                    prevItem2.Slot = item.Slot;
                }
                prevItem2.IsEquipped = false;
                inventory.AddItem(session, prevItem2, false);
                session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.FieldPlayer, prevItem2));
            }
        }

        // Handle unequipping dresses when equipping pants
        // Handle unequipping two-handed main-hands when equipping off-hand weapons
        if (item.ItemSlot == ItemSlot.PA || item.ItemSlot == ItemSlot.LH)
        {
            ItemSlot prevItemSlot = item.ItemSlot == ItemSlot.PA ? ItemSlot.CL : ItemSlot.RH;
            if (equippedInventory.ContainsKey(prevItemSlot))
            {
                if (equippedInventory[prevItemSlot] != null && equippedInventory[prevItemSlot].IsDress)
                {
                    if (equippedInventory.Remove(prevItemSlot, out Item prevItem2))
                    {
                        prevItem2.Slot = item.Slot;
                        prevItem2.IsEquipped = false;
                        inventory.AddItem(session, prevItem2, false);
                        session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.FieldPlayer, prevItem2));
                    }
                }
            }
        }

        // Equip new item
        item.IsEquipped = true;
        item.ItemSlot = equipSlot;
        equippedInventory[equipSlot] = item;
        session.FieldManager.BroadcastPacket(EquipmentPacket.EquipItem(session.FieldPlayer, item, equipSlot));

        // Add stats if gear
        if (item.InventoryTab == InventoryTab.Gear)
        {
            IncreaseStats(session, item);
        }
    }

    private static void HandleUnequipItem(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();
        Inventory inventory = session.Player.Inventory;

        // Unequip gear
        KeyValuePair<ItemSlot, Item> kvpEquips = inventory.Equips.FirstOrDefault(x => x.Value.Uid == itemUid);
        if (kvpEquips.Value != null)
        {
            if (inventory.Equips.Remove(kvpEquips.Key, out Item unequipItem))
            {
                unequipItem.Slot = -1;
                unequipItem.IsEquipped = false;
                inventory.AddItem(session, unequipItem, false);
                session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.FieldPlayer, unequipItem));

                DecreaseStats(session, unequipItem);
            }

            return;
        }

        // Unequip cosmetics
        KeyValuePair<ItemSlot, Item> kvpCosmetics = inventory.Cosmetics.FirstOrDefault(x => x.Value.Uid == itemUid);
        if (kvpCosmetics.Value != null)
        {
            if (inventory.Cosmetics.Remove(kvpCosmetics.Key, out Item unequipItem))
            {
                unequipItem.Slot = -1;
                unequipItem.IsEquipped = false;
                inventory.AddItem(session, unequipItem, false);
                session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.FieldPlayer, unequipItem));
            }
        }
    }

    private static void DecreaseStats(GameSession session, Item item)
    {
        if (item.Stats.BasicStats.Count != 0)
        {
            foreach (NormalStat stat in item.Stats.BasicStats.OfType<NormalStat>())
            {
                session.Player.Stats[(StatId) stat.ItemAttribute].DecreaseBonus(stat.Flat);
            }
        }

        if (item.Stats.BonusStats.Count != 0)
        {
            foreach (NormalStat stat in item.Stats.BonusStats.OfType<NormalStat>())
            {
                session.Player.Stats[(StatId) stat.ItemAttribute].DecreaseBonus(stat.Flat);
            }
        }

        session.Send(StatPacket.SetStats(session.FieldPlayer));
    }

    private static void IncreaseStats(GameSession session, Item item)
    {
        if (item.Stats.BasicStats.Count != 0)
        {
            foreach (NormalStat stat in item.Stats.BasicStats.OfType<NormalStat>())
            {
                session.Player.Stats[(StatId) stat.ItemAttribute].IncreaseBonus(stat.Flat);
            }
        }

        if (item.Stats.BonusStats.Count != 0)
        {
            foreach (NormalStat stat in item.Stats.BonusStats.OfType<NormalStat>())
            {
                session.Player.Stats[(StatId) stat.ItemAttribute].IncreaseBonus(stat.Flat);
            }
        }

        session.Send(StatPacket.SetStats(session.FieldPlayer));
    }
}
