using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class ItemSocketSystemHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.ITEM_SOCKET_SYSTEM;

    private enum ItemSocketSystemMode : byte
    {
        UnlockSocket = 0x0,
        SelectUnlockSocketEquip = 0x2,
        UpgradeGem = 0x4,
        SelectGemUpgrade = 0x6,
        MountGem = 0x8,
        ExtractGem = 0xA
    }

    private enum ItemSocketSystemNotice
    {
        TargetIsNotInYourInventory = 0x1,
        ItemIsNotInYourInventory = 0x2,
        CannotBeUsedAsMaterial = 0x3,
        ConfirmCatalystAmount = 0x4
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
        List<long> fodderUids = new();
        for (int i = 0; i < fodderAmount; i++)
        {
            long fodderUid = packet.ReadLong();
            fodderUids.Add(fodderUid);
        }

        Inventory inventory = session.Player.Inventory;
        if (!inventory.Items.ContainsKey(itemUid))
        {
            session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.ItemIsNotInYourInventory));
            return;
        }
        Item equip = inventory.Items[itemUid];
        int equipUnlockedSlotCount = equip.Stats.GemSockets.Count(x => x.IsUnlocked);

        foreach (long uid in fodderUids)
        {
            if (!inventory.Items.ContainsKey(uid))
            {
                session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.ItemIsNotInYourInventory));
                return;
            }

            Item fodder = inventory.Items[uid];
            int fodderUnlockedSlotCount = fodder.Stats.GemSockets.Count(x => x.IsUnlocked);
            if (equipUnlockedSlotCount == fodderUnlockedSlotCount)
            {
                continue;
            }

            session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.CannotBeUsedAsMaterial));
            return;
        }

        // get socket slot to unlock
        int slot = equip.Stats.GemSockets.FindIndex(0, equip.Stats.GemSockets.Count, x => !x.IsUnlocked);
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
        else if (slot is 1 or 2)
        {
            crystalFragmentCost = 600;
        }

        int crystalFragmentsTotalAmount = 0;
        List<KeyValuePair<long, Item>> crystalFragments = inventory.Items.Where(x => x.Value.Tag == "CrystalPiece").ToList();
        crystalFragments.ForEach(x => crystalFragmentsTotalAmount += x.Value.Amount);

        if (crystalFragmentsTotalAmount < crystalFragmentCost)
        {
            return;
        }

        foreach ((long uid, Item item) in crystalFragments)
        {
            if (item.Amount >= crystalFragmentCost)
            {
                inventory.ConsumeItem(session, uid, crystalFragmentCost);
                break;
            }

            crystalFragmentCost -= item.Amount;
            inventory.ConsumeItem(session, uid, item.Amount);
        }
        foreach (long uid in fodderUids)
        {
            inventory.ConsumeItem(session, uid, 1);
        }

        equip.Stats.GemSockets[slot].IsUnlocked = true;
        List<GemSocket> unlockedSockets = equip.Stats.GemSockets.Where(x => x.IsUnlocked).ToList();

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

        Inventory inventory = session.Player.Inventory;
        if (equipUid == 0) // this is a gemstone in the player's inventory
        {
            if (!inventory.Items.ContainsKey(itemUid))
            {
                session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.ItemIsNotInYourInventory));
                return;
            }

            Item gem = inventory.Items[itemUid];
            if (gem == null)
            {
                return;
            }

            metadata = ItemGemstoneUpgradeMetadataStorage.GetMetadata(gem.Id);
            if (metadata == null || metadata.NextItemId == 0)
            {
                return;
            }

            if (!CheckGemUpgradeIngredients(inventory, metadata))
            {
                return;
            }

            ConsumeIngredients(session, metadata);
            inventory.ConsumeItem(session, gem.Uid, 1);

            Item upgradeGem = new(metadata.NextItemId)
            {
                Rarity = gem.Rarity
            };
            inventory.AddItem(session, upgradeGem, true);
            session.Send(ItemSocketSystemPacket.UpgradeGem(equipUid, slot, upgradeGem));
            return;
        }

        // upgrade gem mounted on a equipment
        if (!inventory.Items.ContainsKey(equipUid))
        {
            session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.ItemIsNotInYourInventory));
            return;
        }

        Gemstone gemstone = inventory.Items[equipUid].Stats.GemSockets[slot].Gemstone;
        if (gemstone == null)
        {
            return;
        }

        metadata = ItemGemstoneUpgradeMetadataStorage.GetMetadata(gemstone.Id);
        if (metadata == null || metadata.NextItemId == 0)
        {
            return;
        }

        if (!CheckGemUpgradeIngredients(inventory, metadata))
        {
            return;
        }

        ConsumeIngredients(session, metadata);

        Item newGem = new(metadata.NextItemId)
        {
            IsLocked = gemstone.IsLocked,
            UnlockTime = gemstone.UnlockTime
        };

        Player owner = GameServer.PlayerManager.GetPlayerById(gemstone.OwnerId);
        if (owner != null)
        {
            newGem.OwnerCharacterId = owner.CharacterId;
            newGem.OwnerCharacterName = owner.Name;
        }

        Gemstone upgradedGemstone = new()
        {
            Id = metadata.NextItemId,
            IsLocked = gemstone.IsLocked,
            UnlockTime = gemstone.UnlockTime,
            OwnerId = gemstone.OwnerId,
            OwnerName = gemstone.OwnerName
        };

        inventory.Items[equipUid].Stats.GemSockets[slot].Gemstone = gemstone;
        session.Send(ItemSocketSystemPacket.UpgradeGem(equipUid, slot, newGem));
    }

    private static bool CheckGemUpgradeIngredients(Inventory inventory, ItemGemstoneUpgradeMetadata metadata)
    {
        for (int i = 0; i < metadata.IngredientItems.Count; i++)
        {
            int inventoryItemCount = 0;
            List<KeyValuePair<long, Item>> ingredients = inventory.Items.Where(x => x.Value.Tag == metadata.IngredientItems[i]).ToList();
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
            List<KeyValuePair<long, Item>> ingredients = session.Player.Inventory.Items.Where(x => x.Value.Tag == metadata.IngredientItems[i]).ToList();

            foreach ((long uid, Item item) in ingredients)
            {
                if (item.Amount >= metadata.IngredientAmounts[i])
                {
                    session.Player.Inventory.ConsumeItem(session, uid, metadata.IngredientAmounts[i]);
                    break;
                }

                metadata.IngredientAmounts[i] -= item.Amount;
                session.Player.Inventory.ConsumeItem(session, uid, item.Amount);
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

        Gemstone gemstone = new()
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

        session.Player.Inventory.ConsumeItem(session, gemItem.Uid, 1);
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
        Item gemstoneItem = new(gemstone.Id)
        {
            IsLocked = gemstone.IsLocked,
            UnlockTime = gemstone.UnlockTime,
            Rarity = 4
        };

        if (gemstone.OwnerId != 0)
        {
            Player owner = GameServer.PlayerManager.GetPlayerById(gemstone.OwnerId);
            if (owner != null)
            {
                gemstoneItem.OwnerCharacterId = owner.CharacterId;
                gemstoneItem.OwnerCharacterName = owner.Name;
            }
        }

        // remove gemstone from item
        equipItem.Stats.GemSockets[slot].Gemstone = null;

        session.Player.Inventory.AddItem(session, gemstoneItem, true);
        session.Send(ItemSocketSystemPacket.ExtractGem(equipItemUid, gemstoneItem.Uid, slot));
    }
}
