using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using MoonSharp.Interpreter;

namespace MapleServer2.PacketHandlers.Game;

public class ItemSocketSystemHandler : GamePacketHandler<ItemSocketSystemHandler>
{
    public override RecvOp OpCode => RecvOp.ItemSocketSystem;

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
                LogUnknownMode(mode);
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

        IInventory inventory = session.Player.Inventory;
        if (!inventory.HasItem(itemUid))
        {
            session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.ItemIsNotInYourInventory));
            return;
        }
        Item equip = inventory.GetByUid(itemUid);
        int equipUnlockedSlotCount = equip.GemSockets.Count(x => x.IsUnlocked);

        foreach (long uid in fodderUids)
        {
            if (!inventory.HasItem(uid))
            {
                session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.ItemIsNotInYourInventory));
                return;
            }

            Item fodder = inventory.GetByUid(uid);
            int fodderUnlockedSlotCount = fodder.GemSockets.Count(x => x.IsUnlocked);
            if (equipUnlockedSlotCount == fodderUnlockedSlotCount)
            {
                continue;
            }

            session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.CannotBeUsedAsMaterial));
            return;
        }

        // get socket slot to unlock
        int slot = equip.GemSockets.FindIndex(0, equip.GemSockets.Count, x => !x.IsUnlocked);
        if (slot < 0)
        {
            return;
        }

        Script script = ScriptLoader.GetScript("Functions/calcItemSocketUnlockIngredient");
        DynValue scriptResult = script.RunFunction("calcItemSocketUnlockIngredient", equip.Rarity, slot, (int) equip.InventoryTab);

        string ingredientTag = scriptResult.Tuple[0].String;
        int ingredientCost = (int) scriptResult.Tuple[1].Number;

        if (!ConsumeIngredients(session, inventory, ingredientCost, ingredientTag))
        {
            return;
        }

        foreach (long uid in fodderUids)
        {
            inventory.ConsumeItem(session, uid, 1);
        }

        equip.GemSockets[slot].IsUnlocked = true;
        List<GemSocket> unlockedSockets = equip.GemSockets.Where(x => x.IsUnlocked).ToList();

        session.Send(ItemSocketSystemPacket.UnlockSocket(equip, (byte) slot, unlockedSockets));
    }

    private static void HandleSelectUnlockSocketEquip(GameSession session, PacketReader packet)
    {
        long unkUid = packet.ReadLong();
        byte slot = packet.ReadByte();
        long itemUid = packet.ReadLong();

        if (!session.Player.Inventory.HasItem(itemUid))
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
        Item upgradeGem = null;

        IInventory inventory = session.Player.Inventory;
        if (equipUid == 0) // this is a gemstone in the player's inventory
        {
            if (!inventory.HasItem(itemUid))
            {
                session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.ItemIsNotInYourInventory));
                return;
            }

            Item gem = inventory.GetByUid(itemUid);
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

            for (int i = 0; i < metadata.IngredientItems.Count; i++)
            {
                if (!ConsumeIngredients(session, inventory, metadata.IngredientAmounts[i], metadata.IngredientItems[i]))
                {
                    return;
                }
            }

            inventory.ConsumeItem(session, gem.Uid, 1);

            upgradeGem = new(metadata.NextItemId, rarity: gem.Rarity);
            inventory.AddItem(session, upgradeGem, true);
            session.Send(ItemSocketSystemPacket.UpgradeGem(equipUid, slot, upgradeGem));
            return;
        }

        // upgrade gem mounted on a equipment
        if (!inventory.HasItem(equipUid) && !session.Player.Inventory.ItemIsEquipped(equipUid))
        {
            session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.ItemIsNotInYourInventory));
            return;
        }

        Item item = inventory.GetByUid(equipUid) ?? inventory.GetEquippedItem(equipUid);
        Gemstone gemstone = item.GemSockets[slot].Gemstone;
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

        for (int i = 0; i < metadata.IngredientItems.Count; i++)
        {
            if (!ConsumeIngredients(session, inventory, metadata.IngredientAmounts[i], metadata.IngredientItems[i]))
            {
                return;
            }
        }

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

        upgradeGem = new(metadata.NextItemId, rarity: 4);

        Gemstone upgradedGemstone = new()
        {
            Id = metadata.NextItemId,
            IsLocked = gemstone.IsLocked,
            UnlockTime = gemstone.UnlockTime,
            OwnerId = gemstone.OwnerId,
            OwnerName = gemstone.OwnerName,
            Stats = new(upgradeGem.Stats),
            AdditionalEffects = ItemMetadataStorage.GetAdditionalEffects(metadata.NextItemId)
        };

        item.GemSockets[slot].Gemstone = upgradedGemstone;
        session.Send(ItemSocketSystemPacket.UpgradeGem(equipUid, slot, newGem));

        if (session.Player.Inventory.ItemIsEquipped(equipUid))
        {
            session.Player.RemoveEffects(gemstone.AdditionalEffects);
            session.Player.AddEffects(upgradedGemstone.AdditionalEffects);

            session.Player.FieldPlayer.ComputeStats();
        }
    }

    private static bool CheckGemUpgradeIngredients(IInventory inventory, ItemGemstoneUpgradeMetadata metadata)
    {
        for (int i = 0; i < metadata.IngredientItems.Count; i++)
        {
            IReadOnlyCollection<Item> ingredients = inventory.GetAllByTag(metadata.IngredientItems[i]);
            int inventoryItemCount = ingredients.Sum(x => x.Amount);

            if (inventoryItemCount < metadata.IngredientAmounts[i])
            {
                return false;
            }
        }
        return true;
    }

    private static bool ConsumeIngredients(GameSession session, IInventory inventory, int ingredientCost, string ingredientTag)
    {
        IReadOnlyCollection<Item> ingredients = inventory.GetAllByTag(ingredientTag);
        int totalIngredientAmount = ingredients.Sum(x => x.Amount);

        if (totalIngredientAmount < ingredientCost)
        {
            return false;
        }

        foreach (Item item in ingredients)
        {
            if (item.Amount >= ingredientCost)
            {
                inventory.ConsumeItem(session, item.Uid, ingredientCost);
                break;
            }

            ingredientCost -= item.Amount;
            inventory.ConsumeItem(session, item.Uid, item.Amount);
        }
        return true;
    }

    private static void HandleSelectGemUpgrade(GameSession session, PacketReader packet)
    {
        long equipUid = packet.ReadLong();
        byte slot = packet.ReadByte();
        long itemUid = packet.ReadLong();

        if (equipUid == 0) // this is a gemstone in the player's inventory
        {
            if (!session.Player.Inventory.HasItem(itemUid))
            {
                session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.ItemIsNotInYourInventory));
                return;
            }

            session.Send(ItemSocketSystemPacket.SelectGemUpgrade(equipUid, slot, itemUid));
            return;
        }

        // select gem mounted on a equipment
        if (!session.Player.Inventory.HasItem(equipUid) && !session.Player.Inventory.ItemIsEquipped(equipUid))
        {
            session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.ItemIsNotInYourInventory));
            return;
        }

        Item item = session.Player.Inventory.GetByUid(equipUid) ?? session.Player.Inventory.GetEquippedItem(equipUid);
        Gemstone gemstone = item.GemSockets[slot].Gemstone;
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

        if (!session.Player.Inventory.HasItem(equipItemUid) && !session.Player.Inventory.ItemIsEquipped(equipItemUid))
        {
            session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.TargetIsNotInYourInventory));
            return;
        }

        if (!session.Player.Inventory.HasItem(gemItemUid))
        {
            session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.ItemIsNotInYourInventory));
            return;
        }

        Item equipItem = session.Player.Inventory.GetByUid(equipItemUid) ?? session.Player.Inventory.GetEquippedItem(equipItemUid);
        Item gemItem = session.Player.Inventory.GetByUid(gemItemUid);

        if (!equipItem.GemSockets[slot].IsUnlocked)
        {
            return;
        }

        if (equipItem.GemSockets[slot].Gemstone != null)
        {
            return;
        }

        Gemstone gemstone = new()
        {
            Id = gemItem.Id,
            IsLocked = gemItem.IsLocked,
            UnlockTime = gemItem.UnlockTime,
            Stats = new(gemItem.Stats),
            AdditionalEffects = gemItem.AdditionalEffects
        };
        if (gemItem.OwnerCharacterId != 0)
        {
            gemstone.OwnerId = gemItem.OwnerCharacterId;
            gemstone.OwnerName = gemItem.OwnerCharacterName;
        }

        equipItem.GemSockets[slot].Gemstone = gemstone;

        session.Player.Inventory.ConsumeItem(session, gemItem.Uid, 1);
        session.Send(ItemSocketSystemPacket.MountGem(equipItemUid, gemstone, slot));

        if (session.Player.Inventory.ItemIsEquipped(equipItemUid))
        {
            session.Player.AddEffects(gemstone.AdditionalEffects);

            session.Player.FieldPlayer.ComputeStats();
        }
    }

    private static void HandleExtractGem(GameSession session, PacketReader packet)
    {
        long equipItemUid = packet.ReadLong();
        byte slot = packet.ReadByte();

        if (!session.Player.Inventory.HasItem(equipItemUid) && !session.Player.Inventory.ItemIsEquipped(equipItemUid))
        {
            session.Send(ItemSocketSystemPacket.Notice((int) ItemSocketSystemNotice.ItemIsNotInYourInventory));
            return;
        }

        Item equipItem = session.Player.Inventory.GetByUid(equipItemUid) ?? session.Player.Inventory.GetEquippedItem(equipItemUid);

        if (equipItem.GemSockets[slot].Gemstone == null)
        {
            return;
        }

        Gemstone gemstone = equipItem.GemSockets[slot].Gemstone;

        int gemLevel = ItemGemstoneUpgradeMetadataStorage.GetGemLevel(gemstone.Id);

        Script script = ScriptLoader.GetScript("Functions/calcGetGemStonePutOffPrice");
        DynValue scriptResult = script.RunFunction("calcGetGemStonePutOffPrice", gemLevel, (int) equipItem.InventoryTab);

        string itemTag = scriptResult.Tuple[0].String;
        int ingredientCost = (int) scriptResult.Tuple[1].Number;

        if (!ConsumeIngredients(session, session.Player.Inventory, ingredientCost, itemTag))
        {
            return;
        }

        Item gemstoneItem = new(gemstone.Id, rarity: 4)
        {
            IsLocked = gemstone.IsLocked,
            UnlockTime = gemstone.UnlockTime,
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
        equipItem.GemSockets[slot].Gemstone = null;

        session.Player.Inventory.AddItem(session, gemstoneItem, true);
        session.Send(ItemSocketSystemPacket.ExtractGem(equipItemUid, gemstoneItem.Uid, slot));

        if (session.Player.Inventory.ItemIsEquipped(equipItemUid))
        {
            session.Player.AddEffects(gemstone.AdditionalEffects);

            session.Player.FieldPlayer.ComputeStats();
        }
    }
}
