using System;
using System.Collections.Generic;
using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class ChangeAttributesHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.CHANGE_ATTRIBUTES;

        public ChangeAttributesHandler(ILogger<GamePacketHandler> logger) : base(logger) { }

        private enum ChangeAttributesMode : byte
        {
            ChangeAttributes = 0,
            SelectNewAttributes = 2,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            ChangeAttributesMode function = (ChangeAttributesMode) packet.ReadByte();

            switch (function)
            {
                case ChangeAttributesMode.ChangeAttributes:
                    HandleChangeAttributes(session, packet);
                    break;
                case ChangeAttributesMode.SelectNewAttributes:
                    HandleSelectNewAttributes(session, packet);
                    break;
            }
        }

        private static void HandleChangeAttributes(GameSession session, PacketReader packet)
        {
            short lockStatId = -1;
            bool isSpecialStat = false;
            long itemUid = packet.ReadLong();
            packet.Skip(8);
            bool useLock = packet.ReadBool();
            if (useLock)
            {
                isSpecialStat = packet.ReadBool();
                lockStatId = packet.ReadShort();
            }

            Inventory inventory = session.Player.Inventory;

            int greenCrystalTotalAmount = 0;
            int metacellTotalAmount = 0;
            int crystalFragmentTotalAmount = 0;

            // There are multiple ids for each type of material
            List<KeyValuePair<long, Item>> greenCrystals = inventory.Items.Where(x => x.Value.Tag == "GreenCrystal").ToList();
            greenCrystals.ForEach(x => greenCrystalTotalAmount += x.Value.Amount);

            List<KeyValuePair<long, Item>> metacells = inventory.Items.Where(x => x.Value.Tag == "MetaCell").ToList();
            metacells.ForEach(x => metacellTotalAmount += x.Value.Amount);

            List<KeyValuePair<long, Item>> crystalFragments = inventory.Items.Where(x => x.Value.Tag == "CrystalPiece").ToList();
            crystalFragments.ForEach(x => crystalFragmentTotalAmount += x.Value.Amount);

            Item gear = inventory.Items.FirstOrDefault(x => x.Key == itemUid).Value;
            Item scrollLock = null;

            // Check if gear exist in inventory
            if (gear == null)
            {
                return;
            }

            string tag = "";
            if (Item.IsAccessory(gear.ItemSlot))
            {
                tag = "LockItemOptionAccessory";
            }
            else if (Item.IsArmor(gear.ItemSlot))
            {
                tag = "LockItemOptionArmor";
            }
            else if (Item.IsWeapon(gear.ItemSlot))
            {
                tag = "LockItemOptionWeapon";
            }
            else if (Item.IsPet(gear.Id))
            {
                tag = "LockItemOptionPet";
            }

            if (useLock)
            {
                scrollLock = inventory.Items.FirstOrDefault(x => x.Value.Tag == tag && x.Value.Rarity == gear.Rarity).Value;
                // Check if scroll lock exist in inventory
                if (scrollLock == null)
                {
                    return;
                }
            }

            int greenCrystalCost = 5;

            int metacellCosts = 25;
            if (gear.TimesAttributesChanged < 14)
            {
                metacellCosts = 11 + gear.TimesAttributesChanged * 1;
            }

            // Relation between TimesAttributesChanged to amount of crystalFragments for epic gear
            int[] crystalFragmentsEpicGear = new int[] { 200, 250, 312, 390, 488, 610, 762, 953, 1192, 1490, 1718, 2131, 2642, 3277, 4063 };

            int crystalFragmentsCosts = Math.Min(crystalFragmentsEpicGear[gear.TimesAttributesChanged], crystalFragmentsEpicGear[14]);

            if (gear.Rarity > (short) RarityType.Epic)
            {
                greenCrystalCost = 25;
                metacellCosts = 375;
                if (gear.TimesAttributesChanged < 14)
                {
                    metacellCosts = 165 + gear.TimesAttributesChanged * 15;
                }
                crystalFragmentsCosts = 6000;
                if (gear.TimesAttributesChanged < 14)
                {
                    crystalFragmentsCosts = 400 + gear.TimesAttributesChanged * 400;
                }
            }

            // Check if player has enough materials
            if (greenCrystalTotalAmount < greenCrystalCost || metacellTotalAmount < metacellCosts || crystalFragmentTotalAmount < crystalFragmentsCosts)
            {
                return;
            }

            gear.TimesAttributesChanged++;

            Random random = new Random();
            Item newItem = new Item(gear);

            // Get random stats except stat that is locked
            List<ItemStat> randomList = ItemStats.RollBonusStatsWithStatLocked(newItem.Id, newItem.Rarity, newItem.Stats.BonusStats.Count, lockStatId, isSpecialStat);

            for (int i = 0; i < newItem.Stats.BonusStats.Count; i++)
            {
                // Check if BonusStats[i] is NormalStat and isSpecialStat is false
                // Check if BonusStats[i] is SpecialStat and isSpecialStat is true
                if ((newItem.Stats.BonusStats[i].GetType() == typeof(NormalStat) && !isSpecialStat) || (newItem.Stats.BonusStats[i].GetType() == typeof(SpecialStat) && isSpecialStat))
                {
                    // If this is the attribute being locked, continue
                    if (newItem.Stats.BonusStats[i].GetId() == lockStatId)
                    {
                        continue;
                    }
                }

                newItem.Stats.BonusStats[i] = randomList[i];
            }

            // Consume materials from inventory
            ConsumeMaterials(session, greenCrystalCost, metacellCosts, crystalFragmentsCosts, greenCrystals, metacells, crystalFragments);

            if (useLock)
            {
                InventoryController.Consume(session, scrollLock.Uid, 1);
            }
            inventory.TemporaryStorage[newItem.Uid] = newItem;

            session.Send(ChangeAttributesPacket.PreviewNewItem(newItem));
        }

        private static void HandleSelectNewAttributes(GameSession session, PacketReader packet)
        {
            long itemUid = packet.ReadLong();

            Inventory inventory = session.Player.Inventory;
            Item gear = inventory.TemporaryStorage.FirstOrDefault(x => x.Key == itemUid).Value;
            if (gear == null)
            {
                return;
            }

            inventory.TemporaryStorage.Remove(itemUid);
            inventory.Replace(gear);
            session.Send(ChangeAttributesPacket.AddNewItem(gear));
        }

        private static void ConsumeMaterials(GameSession session, int greenCrystalCost, int metacellCosts, int crystalFragmentsCosts, List<KeyValuePair<long, Item>> greenCrystals, List<KeyValuePair<long, Item>> metacells, List<KeyValuePair<long, Item>> crystalFragments)
        {
            foreach (KeyValuePair<long, Item> item in greenCrystals)
            {
                if (item.Value.Amount >= greenCrystalCost)
                {
                    InventoryController.Consume(session, item.Key, greenCrystalCost);
                    break;
                }
                else
                {
                    greenCrystalCost -= item.Value.Amount;
                    InventoryController.Consume(session, item.Key, item.Value.Amount);
                }
            }

            foreach (KeyValuePair<long, Item> item in metacells)
            {
                if (item.Value.Amount >= metacellCosts)
                {
                    InventoryController.Consume(session, item.Key, metacellCosts);
                    break;
                }
                else
                {
                    metacellCosts -= item.Value.Amount;
                    InventoryController.Consume(session, item.Key, item.Value.Amount);
                }
            }

            foreach (KeyValuePair<long, Item> item in crystalFragments)
            {
                if (item.Value.Amount >= crystalFragmentsCosts)
                {
                    InventoryController.Consume(session, item.Key, crystalFragmentsCosts);
                    break;
                }
                else
                {
                    crystalFragmentsCosts -= item.Value.Amount;
                    InventoryController.Consume(session, item.Key, item.Value.Amount);
                }
            }
        }
    }
}
