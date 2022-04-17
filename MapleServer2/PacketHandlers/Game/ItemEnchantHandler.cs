using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using MoonSharp.Interpreter;

namespace MapleServer2.PacketHandlers.Game;

public class ItemEnchantHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.RequestItemEnchant;

    private enum ItemEnchantMode : byte
    {
        None = 0,
        BeginEnchant = 0x01,
        UpdateCatalysts = 0x02,
        UpdateCharges = 0x03,
        Ophelia = 0x04,
        Peachy = 0x06
    }

    private enum ItemEnchantError : short
    {
        ItemCannotBeEnchanted = 0x01,
        UnstableItem = 0x02,
        NotEnoughMaterials = 0x03
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        ItemEnchantMode mode = (ItemEnchantMode) packet.ReadByte();

        switch (mode)
        {
            case ItemEnchantMode.None: // Sent when opening up enchant ui
                break;
            case ItemEnchantMode.BeginEnchant:
                HandleBeginEnchant(session, packet);
                break;
            case ItemEnchantMode.UpdateCatalysts:
                HandleUpdateCatalysts(session, packet);
                break;
            case ItemEnchantMode.UpdateCharges:
                HandleUpdateCharges(session, packet);
                break;
            case ItemEnchantMode.Ophelia:
                HandleOpheliaEnchant(session, packet);
                break;
            case ItemEnchantMode.Peachy:
                HandlePeachyEnchant(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleBeginEnchant(GameSession session, PacketReader packet)
    {
        EnchantType type = (EnchantType) packet.ReadByte();
        long itemUid = packet.ReadLong();

        IInventory inventory = session.Player.Inventory;

        Item item = inventory.GetFromInventoryOrEquipped(itemUid);
        if (item is null)
        {
            return;
        }

        if (item.DisableEnchant || EnchantLimitMetadataStorage.IsEnchantable(item.GetItemType(), item.Level, item.EnchantLevel))
        {
            session.Send(ItemEnchantPacket.Notice((short) ItemEnchantError.ItemCannotBeEnchanted));
            return;
        }

        if (item.Type is ItemType.Necklace or ItemType.Belt or ItemType.Earring or ItemType.Ring or ItemType.Shield or ItemType.Spellbook)
        {
            session.Send(ItemEnchantPacket.Notice((short) ItemEnchantError.ItemCannotBeEnchanted));
            return;
        }

        session.Player.ItemEnchant = GetEnchantInfo(item);

        // Get stat difference between current and next enchant level
        Dictionary<StatAttribute, ItemStat> statDiff = GetEnchantStatDiff(item.Stats.Enchants, session.Player.ItemEnchant.Stats);

        session.Send(ItemEnchantPacket.BeginEnchant(type, item, session.Player.ItemEnchant, statDiff));
    }

    private static void HandleUpdateCatalysts(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();
        bool addCatalyst = packet.ReadBool();

        if (!session.Player.Inventory.HasItem(itemUid) && !session.Player.Inventory.ItemIsEquipped(itemUid))
        {
            return;
        }

        ItemEnchant itemEnchant = session.Player.ItemEnchant;
        if (itemEnchant is null)
        {
            return;
        }

        float totalCatalystRate = itemEnchant.Rates.AdditionalCatalysts * itemEnchant.Rates.CatalystRate;
        if (addCatalyst && totalCatalystRate >= 30) // 30 being max amount catalysts can boost 
        {
            session.Send(ItemEnchantPacket.Notice((short) ItemEnchantError.UnstableItem));
            return;
        }

        itemEnchant.UpdateAdditionalCatalysts(itemUid, 1, addCatalyst);

        session.Send(ItemEnchantPacket.UpdateCharges(itemEnchant));
    }

    private static ItemEnchant GetEnchantInfo(Item item)
    {
        ItemEnchant itemEnchantStats = new(item.Uid, item.EnchantLevel);
        Script script = ScriptLoader.GetScript("Functions/calcEnchantValues");
        DynValue successRateScriptResult = script.RunFunction("calcEnchantRates", item.EnchantLevel + 1);
        DynValue ingredientsResult = script.RunFunction("calcEnchantIngredients", item.EnchantLevel + 1, item.Rarity, (int) item.Type, item.Level);

        itemEnchantStats.Rates.BaseSuccessRate = (float) successRateScriptResult.Tuple[0].Number;
        itemEnchantStats.Rates.CatalystRate = (float) successRateScriptResult.Tuple[1].Number;
        itemEnchantStats.PityCharges = (int) successRateScriptResult.Tuple[2].Number;
        itemEnchantStats.Rates.ChargesRate = (float) successRateScriptResult.Tuple[3].Number;

        itemEnchantStats.CatalystAmountRequired = (int) ingredientsResult.Tuple[0].Number;
        for (int i = 1; i < ingredientsResult.Tuple.Length; i += 2)
        {
            EnchantIngredient ingredient = new((ItemStringTag) Enum.Parse(typeof(ItemStringTag), ingredientsResult.Tuple[i].String), (int) ingredientsResult.Tuple[i + 1].Number);
            itemEnchantStats.Ingredients.Add(ingredient);
        }

        itemEnchantStats.Stats = EnchantHelper.GetEnchantStats(item.EnchantLevel + 1, item.Type, item.Level);
        return itemEnchantStats;
    }

    private static void HandleUpdateCharges(GameSession session, PacketReader packet)
    {
        int chargeCount = packet.ReadInt();
        ItemEnchant itemEnchant = session.Player.ItemEnchant;

        Player player = session.Player;
        Item item = player.Inventory.GetFromInventoryOrEquipped(itemEnchant.ItemUid);
        if (item == null || item.Charges < chargeCount)
        {
            return;
        }

        player.ItemEnchant.UpdateCharges(chargeCount);

        session.Send(ItemEnchantPacket.UpdateCharges(session.Player.ItemEnchant));
    }

    private static void HandleOpheliaEnchant(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();

        IInventory inventory = session.Player.Inventory;
        Item item = inventory.GetFromInventoryOrEquipped(itemUid);
        if (item is null)
        {
            return;
        }

        ItemEnchant itemEnchantStats = session.Player.ItemEnchant;
        if (itemEnchantStats.Level != item.EnchantLevel && itemEnchantStats.ItemUid != item.Uid)
        {
            itemEnchantStats = GetEnchantInfo(item);
        }

        // Check if player has enough ingredients
        if (!PlayerHasIngredients(itemEnchantStats, inventory))
        {
            session.Send(ItemEnchantPacket.Notice((short) ItemEnchantError.NotEnoughMaterials));
            return;
        }

        foreach (EnchantIngredient ingredient in itemEnchantStats.Ingredients)
        {
            inventory.ConsumeByTag(session, ingredient.Tag.ToString(), ingredient.Amount);
        }

        foreach (long catalystUids in itemEnchantStats.CatalystItemUids)
        {
            inventory.ConsumeItem(session, catalystUids, 1);
        }

        Random random = Random.Shared;
        double randomResult = random.NextDouble();

        float successRate = itemEnchantStats.Rates.BaseSuccessRate + (itemEnchantStats.Rates.ChargesAdded * itemEnchantStats.Rates.ChargesRate) + (Math.Min(itemEnchantStats.Rates.AdditionalCatalysts * itemEnchantStats.Rates.CatalystRate, 30));
        if (successRate < (float) (randomResult * 100))
        {
            FailEnchant(session, itemEnchantStats, item);
            return;
        }

        SetEnchantStats(session, itemEnchantStats, item);
    }

    private static bool PlayerHasIngredients(ItemEnchant itemEnchantStats, IInventory inventory)
    {
        foreach (EnchantIngredient ingredient in itemEnchantStats.Ingredients)
        {
            IReadOnlyCollection<Item> ingredientTotal = inventory.GetAllByTag(ingredient.Tag.ToString());
            if (ingredientTotal.Sum(x => x.Amount) < ingredient.Amount)
            {
                return false;
            }
        }
        return itemEnchantStats.CatalystAmountRequired <= itemEnchantStats.CatalystItemUids.Count;
    }

    private static void SetEnchantStats(GameSession session, ItemEnchant itemEnchantStats, Item item)
    {
        Dictionary<StatAttribute, ItemStat> statDiff = GetEnchantStatDiff(item.Stats.Enchants, itemEnchantStats.Stats);
        foreach (ItemStat stat in itemEnchantStats.Stats.Values)
        {
            if (!item.Stats.Enchants.ContainsKey(stat.ItemAttribute))
            {
                item.Stats.Enchants[stat.ItemAttribute] = new BasicStat(stat.ItemAttribute, stat.Rate, StatAttributeType.Rate);
                continue;
            }
            item.Stats.Enchants[stat.ItemAttribute].Flat = stat.Flat;
            item.Stats.Enchants[stat.ItemAttribute].Rate = stat.Rate;
        }
        item.EnchantLevel++;
        item.EnchantExp = 0;
        item.Charges -= itemEnchantStats.Rates.ChargesAdded;

        session.Send(ItemEnchantPacket.EnchantSuccess(item, statDiff.Values.ToList()));
        //TODO: If item is equipped, update stats
    }

    private static Dictionary<StatAttribute, ItemStat> GetEnchantStatDiff(Dictionary<StatAttribute, ItemStat> itemStats, Dictionary<StatAttribute, ItemStat> nextLevelStats)
    {
        Dictionary<StatAttribute, ItemStat> statDiffs = new();
        foreach (ItemStat stat in nextLevelStats.Values)
        {
            float currentRate = 0;
            if (itemStats.ContainsKey(stat.ItemAttribute))
            {
                currentRate = itemStats[stat.ItemAttribute].Rate;
            }
            statDiffs[stat.ItemAttribute] = new BasicStat(stat.ItemAttribute, stat.Rate - currentRate, StatAttributeType.Rate);
        }

        return statDiffs;
    }

    private static void FailEnchant(GameSession session, ItemEnchant itemEnchantStats, Item item)
    {
        item.Charges -= itemEnchantStats.Rates.ChargesAdded;
        item.Charges += itemEnchantStats.PityCharges;
        session.Send(ItemEnchantPacket.EnchantFail(item, itemEnchantStats));
    }

    private static void HandlePeachyEnchant(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();

        Item item = session.Player.Inventory.GetByUid(itemUid);
        if (item == null)
        {
            return;
        }

        ItemEnchant itemEnchantStats = session.Player.ItemEnchant;
        if (itemEnchantStats.Level != item.EnchantLevel && itemEnchantStats.ItemUid != item.Uid)
        {
            itemEnchantStats = GetEnchantInfo(item);
        }

        IInventory inventory = session.Player.Inventory;

        // Check if player has enough ingredients
        if (!PlayerHasIngredients(itemEnchantStats, inventory))
        {
            session.Send(ItemEnchantPacket.Notice((short) ItemEnchantError.NotEnoughMaterials));
            return;
        }

        foreach (EnchantIngredient ingredient in itemEnchantStats.Ingredients)
        {
            inventory.ConsumeByTag(session, ingredient.Tag.ToString(), ingredient.Amount);
        }

        foreach (long catalystUids in itemEnchantStats.CatalystItemUids)
        {
            inventory.ConsumeItem(session, catalystUids, 1);
        }

        int neededEnchants = GetNeededEnchantExp(item.EnchantLevel);
        int expGained = (int) Math.Ceiling((double) (10000 / neededEnchants));

        item.EnchantExp += expGained;
        if (item.EnchantExp >= 10000)
        {
            item.EnchantExp = 0;
            SetEnchantStats(session, itemEnchantStats, item);
        }

        session.Send(ItemEnchantPacket.UpdateExp(item));
    }

    private static int GetNeededEnchantExp(int currentEnchantLevel)
    {
        // hard coded values in client (?)
        return currentEnchantLevel switch
        {
            0 or 1 or 2 => 1,
            3 or 4 or 5 => 2,
            6 or 7 or 8 => 4,
            9 => 5,
            10 => 3,
            11 or 12 => 5,
            13 or 14 => 8,
            _ => 0
        };
    }
}
