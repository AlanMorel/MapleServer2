using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class ItemEquipHandler : GamePacketHandler<ItemEquipHandler>
{
    public override RecvOp OpCode => RecvOp.ItemEquip;

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
            default:
                LogUnknownMode(function);
                break;
        }
    }

    private static void HandleEquipItem(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();
        string equipSlotStr = packet.ReadUnicodeString();
        if (!Enum.TryParse(equipSlotStr, out ItemSlot equipSlot))
        {
            Logger.Warning("Unknown equip slot: {equipSlotStr}", equipSlotStr);
            return;
        }

        Player player = session.Player;
        Item item = player.Inventory.GetByUid(itemUid);
        if (item is null || item.IsExpired())
        {
            return;
        }

        if (item.TransferFlag.HasFlag(ItemTransferFlag.Binds) && item.TransferType == TransferType.BindOnEquip && !item.BindItem(player))
        {
            return;
        }

        // Remove the item from the users inventory
        IInventory inventory = player.Inventory;
        inventory.RemoveItem(session, itemUid, out item);

        // Get correct equipped inventory
        Dictionary<ItemSlot, Item> equippedInventory = player.GetEquippedInventory(item.InventoryTab);
        if (equippedInventory == null)
        {
            Logger.Warning("equippedInventory was null: {inventoryTab}", item.InventoryTab);
            return;
        }

        // Move previously equipped item back to inventory
        if (equippedInventory.Remove(equipSlot, out Item prevItem))
        {
            prevItem.Slot = item.Slot;
            prevItem.IsEquipped = false;
            inventory.AddItem(session, prevItem, false);
            session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(player.FieldPlayer, prevItem));

            if (prevItem.InventoryTab == InventoryTab.Gear)
            {
                player.DecreaseStats(prevItem);
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
                session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(player.FieldPlayer, prevItem2));
            }
        }

        // Handle unequipping dresses when equipping pants
        // Handle unequipping two-handed main-hands when equipping off-hand weapons
        if (item.ItemSlot is ItemSlot.PA or ItemSlot.LH)
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
                        session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(player.FieldPlayer, prevItem2));
                    }
                }
            }
        }

        // Equip new item
        item.IsEquipped = true;
        item.ItemSlot = equipSlot;
        equippedInventory[equipSlot] = item;
        session.FieldManager.BroadcastPacket(EquipmentPacket.EquipItem(player.FieldPlayer, item, equipSlot));

        // Add stats if gear
        if (item.InventoryTab == InventoryTab.Gear)
        {
            player.IncreaseStats(item);
        }
    }

    private static void HandleUnequipItem(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();
        Player player = session.Player;
        IInventory inventory = player.Inventory;

        // Unequip gear
        (ItemSlot itemSlot, Item item) = inventory.Equips.FirstOrDefault(x => x.Value.Uid == itemUid);
        if (item is not null)
        {
            if (!inventory.Equips.Remove(itemSlot, out Item unequipItem))
            {
                return;
            }

            unequipItem.Slot = -1;
            unequipItem.IsEquipped = false;
            inventory.AddItem(session, unequipItem, false);
            session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(player.FieldPlayer, unequipItem));

            player.DecreaseStats(unequipItem);
            return;
        }

        // Unequip cosmetics
        (ItemSlot itemSlot2, Item item2) = inventory.Cosmetics.FirstOrDefault(x => x.Value.Uid == itemUid);
        if (item2 is null)
        {
            return;
        }

        if (!inventory.Cosmetics.Remove(itemSlot2, out Item unequipItem2))
        {
            return;
        }

        unequipItem2.Slot = -1;
        unequipItem2.IsEquipped = false;
        inventory.AddItem(session, unequipItem2, false);
        session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(player.FieldPlayer, unequipItem2));
    }
}
