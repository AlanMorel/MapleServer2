using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class ItemSocketSystemHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.ITEM_SOCKET_SYSTEM;

        public ItemSocketSystemHandler(ILogger<ItemSocketSystemHandler> logger) : base(logger) { }

        private enum ItemSocketSystemMode : byte
        {
            UnlockSocket = 0x0,
            SelectUnlockSocketEquip = 0x2,
            UpgradeGem = 0x4,
            SelectGemUpgrade = 0x6,
            MountGem = 0x8,
            ExtractGem = 0xA,
        }

        private enum ItemSocketSystemNotice : int
        {
            TargetIsNotInYourInventory = 0x1,
            ItemIsNotInYourInventory = 0x2,
            CannotBeUsedAsMaterial = 0x3,
            ConfirmCatalystAmount = 0x4,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            ItemSocketSystemMode mode = (ItemSocketSystemMode) packet.ReadByte();

            switch (mode)
            {
                case ItemSocketSystemMode.UnlockSocket:
                    HandleUnlockSocket(session, packet);
                    break;
                case ItemSocketSystemMode.SelectUnlockSocketEquip:
                    HandleSelectUnlockSocketEquip(session, packet);
                    break;
                case ItemSocketSystemMode.SelectGemUpgrade:
                    HandleSelectGemUpgrade(session, packet);
                    break;
                case ItemSocketSystemMode.UpgradeGem:
                    HandleUpgradeGem(session, packet);
                    break;
                case ItemSocketSystemMode.MountGem:
                    HandleMountGem(session, packet);
                    break;
                case ItemSocketSystemMode.ExtractGem:
                    HandleExtractGem(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleUnlockSocket(GameSession session, PacketReader packet)
        {
            long itemUid = packet.ReadLong();
            byte fodderAmount = packet.ReadByte();
            List<long> fodderUids = new List<long>();
            for (int i = 0; i < fodderAmount; i++)
            {
                long fodderUid = packet.ReadLong();
                fodderUids.Add(fodderUid);
            }

            if (!session.Player.Inventory.Items.ContainsKey(itemUid))
            {
                session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.ItemIsNotInYourInventory));
                return;
            }
            Item equip = session.Player.Inventory.Items[itemUid];
            int equipUnlockedSlotCount = equip.Stats.GemSockets.Where(x => x.IsUnlocked == true).Count();

            foreach (long uid in fodderUids)
            {
                if (!session.Player.Inventory.Items.ContainsKey(uid))
                {
                    session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.ItemIsNotInYourInventory));
                    return;
                }

                Item fodder = session.Player.Inventory.Items[uid];
                int fodderUnlockedSlotCount = fodder.Stats.GemSockets.Where(x => x.IsUnlocked == true).Count();
                if (equipUnlockedSlotCount != fodderUnlockedSlotCount)
                {
                    session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.CannotBeUsedAsMaterial));
                    return;
                }
            }

            // get socket slot to unlock
            int slot = equip.Stats.GemSockets.FindIndex(0, equip.Stats.GemSockets.Count, x => x.IsUnlocked != true);
            if (slot < 0)
            {
                return;
            }

            // fragmment cost. hard coded into the client?
            int crystalFragmentCost = 0;
            if (slot == 0)
            {
                crystalFragmentCost = 400;
            }
            else if (slot == 1 || slot == 2)
            {
                crystalFragmentCost = 600;
            }

            int crystalFragmentsTotalAmount = 0;
            List<KeyValuePair<long, Item>> crystalFragments = session.Player.Inventory.Items.Where(x => x.Value.Tag == "CrystalPiece").ToList();
            crystalFragments.ForEach(x => crystalFragmentsTotalAmount += x.Value.Amount);

            if (crystalFragmentsTotalAmount < crystalFragmentCost)
            {
                return;
            }

            foreach (KeyValuePair<long, Item> item in crystalFragments)
            {
                if (item.Value.Amount >= crystalFragmentCost)
                {
                    InventoryController.Consume(session, item.Key, crystalFragmentCost);
                    break;
                }
                else
                {
                    crystalFragmentCost -= item.Value.Amount;
                    InventoryController.Consume(session, item.Key, item.Value.Amount);
                }
            }
            foreach (long uid in fodderUids)
            {
                InventoryController.Consume(session, uid, 1);
            }

            equip.Stats.GemSockets[slot].IsUnlocked = true;
            List<GemSocket> unlockedSockets = equip.Stats.GemSockets.Where(x => x.IsUnlocked == true).ToList();

            session.Send(ItemSocketSystemPacket.UnlockSocket(equip, (byte) slot, unlockedSockets));
        }

        private static void HandleSelectUnlockSocketEquip(GameSession session, PacketReader packet)
        {
            long unkUid = packet.ReadLong();
            byte slot = packet.ReadByte();
            long itemUid = packet.ReadLong();

            if (!session.Player.Inventory.Items.ContainsKey(itemUid))
            {
                session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.ItemIsNotInYourInventory));
                return;
            }

            session.Send(ItemSocketSystemPacket.SelectUnlockSocketEquip(unkUid, slot, itemUid));
        }

        private static void HandleUpgradeGem(GameSession session, PacketReader packet)
        {
            long equipUid = packet.ReadLong();
            byte slot = packet.ReadByte();
            long itemUid = packet.ReadLong();

            ItemGemstoneUpgradeMetadata metadata;

            if (equipUid == 0) // this is a gemstone in the player's inventory
            {
                if (!session.Player.Inventory.Items.ContainsKey(itemUid))
                {
                    session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.ItemIsNotInYourInventory));
                    return;
                }

                Item gem = session.Player.Inventory.Items[itemUid];
                if (gem == null)
                {
                    return;
                }

                metadata = ItemGemstoneUpgradeMetadataStorage.GetMetadata(gem.Id);
                if (metadata == null || metadata.NextItemId == 0)
                {
                    return;
                }

                if (!CheckGemUpgradeIngredients(session.Player.Inventory, metadata))
                {
                    return;
                }

                ConsumeIngredients(session, metadata);
                InventoryController.Consume(session, gem.Uid, 1);

                Item upgradeGem = new Item(metadata.NextItemId)
                {
                    Rarity = gem.Rarity
                };
                InventoryController.Add(session, upgradeGem, true);
                session.Send(ItemSocketSystemPacket.UpgradeGem(equipUid, slot, upgradeGem));
                return;
            }

            // upgrade gem mounted on a equipment
            if (!session.Player.Inventory.Items.ContainsKey(equipUid))
            {
                session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.ItemIsNotInYourInventory));
                return;
            }

            Gemstone gemstone = session.Player.Inventory.Items[equipUid].Stats.GemSockets[slot].Gemstone;
            if (gemstone == null)
            {
                return;
            }

            metadata = ItemGemstoneUpgradeMetadataStorage.GetMetadata(gemstone.Id);
            if (metadata == null || metadata.NextItemId == 0)
            {
                return;
            }

            if (!CheckGemUpgradeIngredients(session.Player.Inventory, metadata))
            {
                return;
            }

            ConsumeIngredients(session, metadata);

            Item newGem = new Item(metadata.NextItemId)
            {
                IsLocked = gemstone.IsLocked,
                UnlockTime = gemstone.UnlockTime,
            };

            Player owner = GameServer.Storage.GetPlayerById(gemstone.OwnerId);
            if (owner != null)
            {
                newGem.OwnerCharacterId = owner.CharacterId;
                newGem.OwnerCharacterName = owner.Name;
            }

            Gemstone upgradedGemstone = new Gemstone()
            {
                Id = metadata.NextItemId,
                IsLocked = gemstone.IsLocked,
                UnlockTime = gemstone.UnlockTime,
                OwnerId = gemstone.OwnerId,
                OwnerName = gemstone.OwnerName
            };

            session.Player.Inventory.Items[equipUid].Stats.GemSockets[slot].Gemstone = gemstone;
            session.Send(ItemSocketSystemPacket.UpgradeGem(equipUid, slot, newGem));
        }

        private static bool CheckGemUpgradeIngredients(Inventory inventory, ItemGemstoneUpgradeMetadata metadata)
        {
            for (int i = 0; i < metadata.IngredientItems.Count; i++)
            {
                int inventoryItemCount = 0;
                List<KeyValuePair<long, Item>> ingredients = new List<KeyValuePair<long, Item>>();
                ingredients = inventory.Items.Where(x => x.Value.Tag == metadata.IngredientItems[i]).ToList();
                ingredients.ForEach(x => inventoryItemCount += x.Value.Amount);

                if (inventoryItemCount < metadata.IngredientAmounts[i])
                {
                    return false;
                }
            }
            return true;
        }

        private static void ConsumeIngredients(GameSession session, ItemGemstoneUpgradeMetadata metadata)
        {
            for (int i = 0; i < metadata.IngredientItems.Count; i++)
            {
                List<KeyValuePair<long, Item>> ingredients = new List<KeyValuePair<long, Item>>();
                ingredients = session.Player.Inventory.Items.Where(x => x.Value.Tag == metadata.IngredientItems[i]).ToList();

                foreach (KeyValuePair<long, Item> item in ingredients)
                {
                    if (item.Value.Amount >= metadata.IngredientAmounts[i])
                    {
                        InventoryController.Consume(session, item.Key, metadata.IngredientAmounts[i]);
                        break;
                    }
                    else
                    {
                        metadata.IngredientAmounts[i] -= item.Value.Amount;
                        InventoryController.Consume(session, item.Key, item.Value.Amount);
                    }
                }
            }
        }

        private static void HandleSelectGemUpgrade(GameSession session, PacketReader packet)
        {
            long equipUid = packet.ReadLong();
            byte slot = packet.ReadByte();
            long itemUid = packet.ReadLong();

            if (equipUid == 0) // this is a gemstone in the player's inventory
            {
                if (!session.Player.Inventory.Items.ContainsKey(itemUid))
                {
                    session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.ItemIsNotInYourInventory));
                    return;
                }

                session.Send(ItemSocketSystemPacket.SelectGemUpgrade(equipUid, slot, itemUid));
                return;
            }

            // select gem mounted on a equipment
            if (!session.Player.Inventory.Items.ContainsKey(equipUid))
            {
                session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.ItemIsNotInYourInventory));
                return;
            }

            Gemstone gemstone = session.Player.Inventory.Items[equipUid].Stats.GemSockets[slot].Gemstone;
            if (gemstone == null)
            {
                return;
            }

            session.Send(ItemSocketSystemPacket.SelectGemUpgrade(equipUid, slot, itemUid));
        }

        private static void HandleMountGem(GameSession session, PacketReader packet)
        {
            long equipItemUid = packet.ReadLong();
            long gemItemUid = packet.ReadLong();
            byte slot = packet.ReadByte();

            if (!session.Player.Inventory.Items.ContainsKey(equipItemUid))
            {
                session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.TargetIsNotInYourInventory));
                return;
            }

            if (!session.Player.Inventory.Items.ContainsKey(gemItemUid))
            {
                session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.ItemIsNotInYourInventory));
                return;
            }

            Item equipItem = session.Player.Inventory.Items[equipItemUid];
            Item gemItem = session.Player.Inventory.Items[gemItemUid];

            if (!equipItem.Stats.GemSockets[slot].IsUnlocked)
            {
                return;
            }

            if (equipItem.Stats.GemSockets[slot].Gemstone != null)
            {
                return;
            }

            Gemstone gemstone = new Gemstone()
            {
                Id = gemItem.Id,
                IsLocked = gemItem.IsLocked,
                UnlockTime = gemItem.UnlockTime
            };
            if (gemItem.OwnerCharacterId != 0)
            {
                gemstone.OwnerId = gemItem.OwnerCharacterId;
                gemstone.OwnerName = gemItem.OwnerCharacterName;
            }

            equipItem.Stats.GemSockets[slot].Gemstone = gemstone;

            InventoryController.Consume(session, gemItem.Uid, 1);
            session.Send(ItemSocketSystemPacket.MountGem(equipItemUid, gemstone, slot));
        }

        private static void HandleExtractGem(GameSession session, PacketReader packet)
        {
            long equipItemUid = packet.ReadLong();
            byte slot = packet.ReadByte();

            if (!session.Player.Inventory.Items.ContainsKey(equipItemUid))
            {
                session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.ItemIsNotInYourInventory));
                return;
            }

            Item equipItem = session.Player.Inventory.Items[equipItemUid];

            if (equipItem.Stats.GemSockets[slot].Gemstone == null)
            {
                return;
            }

            Gemstone gemstone = equipItem.Stats.GemSockets[slot].Gemstone;

            // crystal fragment cost
            Item gemstoneItem = new Item(gemstone.Id)
            {
                IsLocked = gemstone.IsLocked,
                UnlockTime = gemstone.UnlockTime,
                Rarity = 4
            };

            if (gemstone.OwnerId != 0)
            {
                Player owner = GameServer.Storage.GetPlayerById(gemstone.OwnerId);
                if (owner != null)
                {
                    gemstoneItem.OwnerCharacterId = owner.CharacterId;
                    gemstoneItem.OwnerCharacterName = owner.Name;
                }
            }

            // remove gemstone from item
            equipItem.Stats.GemSockets[slot].Gemstone = null;

            InventoryController.Add(session, gemstoneItem, true);
            session.Send(ItemSocketSystemPacket.ExtractGem(equipItemUid, gemstoneItem.Uid, slot));
        }
    }
}
