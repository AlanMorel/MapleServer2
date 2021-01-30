using System;
using System.Collections.Generic;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Extensions;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using MapleServer2.Tools;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class ItemEquipHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.ITEM_EQUIP;

        public ItemEquipHandler(ILogger<ItemEquipHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte function = packet.ReadByte();

            switch (function)
            {
                case 0:
                    HandleEquipItem(session, packet);
                    break;
                case 1:
                    HandleUnequipItem(session, packet);
                    break;
            }
        }

        private void HandleEquipItem(GameSession session, PacketReader packet)
        {
            long itemUid = packet.ReadLong();
            string equipSlotStr = packet.ReadUnicodeString();
            if (!Enum.TryParse(equipSlotStr, out ItemSlot equipSlot))
            {
                Logger.Warning("Unknown equip slot: " + equipSlotStr);
                return;
            }

            // Remove the item from the users inventory
            InventoryController.Remove(session, itemUid, out Item item);

            // Get correct equipped inventory
            Dictionary<ItemSlot, Item> equippedInventory = session.Player.GetEquippedInventory(item.InventoryTab);
            if (equippedInventory == null)
            {
                Logger.Warning("equippedInventory was null: " + item.InventoryTab);
                return;
            }

            // Move previously equipped item back to inventory
            if (equippedInventory.Remove(equipSlot, out Item prevItem))
            {
                prevItem.Slot = item.Slot;
                InventoryController.Add(session, prevItem, false);
                session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.FieldPlayer, prevItem));
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
                    InventoryController.Add(session, prevItem2, false);
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
                            InventoryController.Add(session, prevItem2, false);
                            session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.FieldPlayer, prevItem2));
                        }
                    }
                }
            }

            // Equip new item
            equippedInventory[equipSlot] = item;
            session.FieldManager.BroadcastPacket(EquipmentPacket.EquipItem(session.FieldPlayer, item, equipSlot));

            if (item.InventoryTab == InventoryTab.Gear)
            {
                // TODO - Increase stats based on the item stats itself
                session.Player.Stats.CritRate.Max += 12;
                session.Player.Stats.CritRate.Total += 12;

                session.Player.Stats.MinAtk.Max += 15;
                session.Player.Stats.MinAtk.Total += 15;

                session.Player.Stats.MaxAtk.Max += 17;
                session.Player.Stats.MaxAtk.Total += 17;

                session.Player.Stats.MagAtk.Max += 15;
                session.Player.Stats.MagAtk.Total += 15;

                session.Send(StatPacket.SetStats(session.FieldPlayer));
            }
        }

        private static void HandleUnequipItem(GameSession session, PacketReader packet)
        {
            long itemUid = packet.ReadLong();

            bool unequipped = false;

            // Unequip gear
            foreach ((ItemSlot slot, Item item) in session.Player.Equips)
            {
                if (itemUid != item.Uid)
                    continue;
                if (session.Player.Equips.Remove(slot, out Item unequipItem))
                {
                    unequipped = true;

                    unequipItem.Slot = -1;
                    InventoryController.Add(session, unequipItem, false);
                    session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.FieldPlayer, unequipItem));
                    break;
                }
            }

            if (unequipped)
            {
                return;
            }

            // Unequip cosmetic
            foreach ((ItemSlot slot, Item item) in session.Player.Cosmetics)
            {
                if (itemUid != item.Uid)
                    continue;
                if (session.Player.Cosmetics.Remove(slot, out Item unequipItem))
                {
                    unequipItem.Slot = -1;
                    InventoryController.Add(session, unequipItem, false);
                    session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.FieldPlayer, unequipItem));
                    break;
                }
            }
        }
    }
}
