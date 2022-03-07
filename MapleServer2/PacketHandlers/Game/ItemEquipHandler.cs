using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class ItemEquipHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.ITEM_EQUIP;

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
                IPacketHandler<GameSession>.LogUnknownMode(function);
                break;
        }
    }

    private static void HandleEquipItem(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();
        string equipSlotStr = packet.ReadUnicodeString();
        if (!Enum.TryParse(equipSlotStr, out ItemSlot equipSlot))
        {
            Logger.Warn($"Unknown equip slot: {equipSlotStr}");
            return;
        }

        Item item = session.Player.Inventory.GetByUid(itemUid);
        if (item is null)
        {
            return;
        }

        if (item.TransferFlag.HasFlag(ItemTransferFlag.Binds) && item.TransferType == TransferType.BindOnEquip && !item.BindItem(session.Player))
        {
            return;
        }

        // Remove the item from the users inventory
        IInventory inventory = session.Player.Inventory;
        inventory.RemoveItem(session, itemUid, out item);

        // Get correct equipped inventory
        Dictionary<ItemSlot, Item> equippedInventory = session.Player.GetEquippedInventory(item.InventoryTab);
        if (equippedInventory == null)
        {
            Logger.Warn($"equippedInventory was null: {item.InventoryTab}");
            return;
        }

        // Move previously equipped item back to inventory
        if (equippedInventory.Remove(equipSlot, out Item prevItem))
        {
            prevItem.Slot = item.Slot;
            prevItem.IsEquipped = false;
            inventory.AddItem(session, prevItem, false);
            session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.Player.FieldPlayer, prevItem));

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
                session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.Player.FieldPlayer, prevItem2));
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
                        session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.Player.FieldPlayer, prevItem2));
                    }
                }
            }
        }

        // Equip new item
        item.IsEquipped = true;
        item.ItemSlot = equipSlot;
        equippedInventory[equipSlot] = item;
        session.FieldManager.BroadcastPacket(EquipmentPacket.EquipItem(session.Player.FieldPlayer, item, equipSlot));

        // Add stats if gear
        if (item.InventoryTab == InventoryTab.Gear)
        {
            IncreaseStats(session, item);
        }
    }

    private static void HandleUnequipItem(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();
        IInventory inventory = session.Player.Inventory;

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
            session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.Player.FieldPlayer, unequipItem));

            DecreaseStats(session, unequipItem);
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
        session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.Player.FieldPlayer, unequipItem2));
    }

    private static void DecreaseStats(GameSession session, Item item)
    {
        if (item.Stats.BasicStats.Count != 0)
        {
            foreach (NormalStat stat in item.Stats.BasicStats.OfType<NormalStat>())
            {
                session.Player.Stats[stat.ItemAttribute].DecreaseBonus(stat.Flat);
            }
        }

        if (item.Stats.BonusStats.Count != 0)
        {
            foreach (NormalStat stat in item.Stats.BonusStats.OfType<NormalStat>())
            {
                session.Player.Stats[stat.ItemAttribute].DecreaseBonus(stat.Flat);
            }
        }

        session.Send(StatPacket.SetStats(session.Player.FieldPlayer));
    }

    private static void IncreaseStats(GameSession session, Item item)
    {
        if (item.Stats.BasicStats.Count != 0)
        {
            foreach (NormalStat stat in item.Stats.BasicStats.OfType<NormalStat>())
            {
                session.Player.Stats[stat.ItemAttribute].IncreaseBonus(stat.Flat);
            }
        }

        if (item.Stats.BonusStats.Count != 0)
        {
            foreach (NormalStat stat in item.Stats.BonusStats.OfType<NormalStat>())
            {
                session.Player.Stats[stat.ItemAttribute].IncreaseBonus(stat.Flat);
            }
        }

        session.Send(StatPacket.SetStats(session.Player.FieldPlayer));
    }
}
