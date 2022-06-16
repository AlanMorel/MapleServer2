using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using MoonSharp.Interpreter;

namespace MapleServer2.PacketHandlers.Game;

public class LimitBreakHandler : GamePacketHandler<LimitBreakHandler>
{
    public override RecvOp OpCode => RecvOp.LimitBreak;

    private enum LimitBreakMode : byte
    {
        SelectItem = 0x00,
        LimitBreakItem = 0x01
    }

    private enum LimitBreakError : short
    {
        ItemCannotLimitBreak = 0x01,
        InsufficientIngredients = 0x02,
        InsufficientMesos = 0x03
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        LimitBreakMode mode = (LimitBreakMode) packet.ReadByte();

        switch (mode)
        {
            case LimitBreakMode.SelectItem:
                HandleSelectItem(session, packet);
                break;
            case LimitBreakMode.LimitBreakItem:
                HandleLimitBreakItem(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleSelectItem(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();

        Item item = session.Player.Inventory.GetByUid(itemUid);
        if (item is null)
        {
            return;
        }

        if (item.EnchantLevel < 15 && item.LimitBreakLevel <= 0)
        {
            session.Send(LimitBreakPacket.Notice((short) LimitBreakError.ItemCannotLimitBreak));
            return;
        }

        Script script = ScriptLoader.GetScript("Functions/calcLimitBreakValues");
        DynValue scriptResultCosts = script.RunFunction("calcLimitBreakCost", item.LimitBreakLevel);
        long mesoCost = (long) scriptResultCosts.Tuple[0].Number;

        List<EnchantIngredient> ingredients = GetIngredients(scriptResultCosts);

        Item nextLevelItem = GetNextLevelItem(item);
        session.Send(LimitBreakPacket.SelectedItem(item.Uid, nextLevelItem, mesoCost, ingredients));
    }

    private static void HandleLimitBreakItem(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();
        Item item = session.Player.Inventory.GetByUid(itemUid);
        if (item is null)
        {
            return;
        }

        if (item.EnchantLevel < 15 && item.LimitBreakLevel <= 0)
        {
            session.Send(LimitBreakPacket.Notice((short) LimitBreakError.ItemCannotLimitBreak));
            return;
        }
        Script script = ScriptLoader.GetScript("Functions/calcLimitBreakValues");
        DynValue scriptResultCosts = script.RunFunction("calcLimitBreakCost", item.LimitBreakLevel);
        if (!session.Player.Wallet.Meso.Modify((long) -scriptResultCosts.Tuple[0].Number))
        {
            session.Send(LimitBreakPacket.Notice((short) LimitBreakError.InsufficientMesos));
            return;
        }

        List<EnchantIngredient> ingredients = GetIngredients(scriptResultCosts);

        if (!PlayerHasIngredients(ingredients, session.Player.Inventory))
        {
            session.Send(LimitBreakPacket.Notice((short) LimitBreakError.InsufficientIngredients));
            return;
        }

        foreach (EnchantIngredient ingredient in ingredients)
        {
            session.Player.Inventory.ConsumeByTag(session, ingredient.Tag.ToString(), ingredient.Amount);
        }

        Item nextLevelItem = GetNextLevelItem(item);

        item.LimitBreakLevel = nextLevelItem.LimitBreakLevel;
        item.EnchantLevel = nextLevelItem.EnchantLevel;
        item.Stats = nextLevelItem.Stats;
        session.Send(LimitBreakPacket.LimitBreakItem(item));
    }

    private static List<EnchantIngredient> GetIngredients(DynValue scriptResultCosts)
    {
        List<EnchantIngredient> ingredients = new();
        for (int i = 1; i < scriptResultCosts.Tuple.Length; i += 2)
        {
            EnchantIngredient ingredient = new((ItemStringTag) Enum.Parse(typeof(ItemStringTag), scriptResultCosts.Tuple[i].String), (int) scriptResultCosts.Tuple[i + 1].Number);
            ingredients.Add(ingredient);
        }
        return ingredients;
    }

    private static bool PlayerHasIngredients(List<EnchantIngredient> ingredients, IInventory inventory)
    {
        foreach (EnchantIngredient ingredient in ingredients)
        {
            IReadOnlyCollection<Item> ingredientTotal = inventory.GetAllByTag(ingredient.Tag.ToString());
            if (ingredientTotal.Sum(x => x.Amount) < ingredient.Amount)
            {
                return false;
            }
        }
        return true;
    }

    private static Item GetNextLevelItem(Item item)
    {
        Script script = ScriptLoader.GetScript("Functions/calcLimitBreakValues");
        DynValue scriptResultRates = script.RunFunction("calcLimitBreakStatRateValues", item.LimitBreakLevel);
        DynValue scriptResultFlats = script.RunFunction("calcLimitBreakStatFlatValues", item.LimitBreakLevel);

        Item nextLevelItem = new(item)
        {
            Uid = 0
        };

        if (nextLevelItem.LimitBreakLevel == 0)
        {
            nextLevelItem.EnchantLevel = 0;
            nextLevelItem.Stats.LimitBreakEnchants = nextLevelItem.Stats.Enchants;
            nextLevelItem.Stats.Enchants = new();
        }

        for (int i = 0; i < scriptResultRates.Tuple.Length; i += 2)
        {
            if (scriptResultRates.Tuple[i].Number == 0)
            {
                continue;
            }
            StatAttribute attribute = (StatAttribute) scriptResultRates.Tuple[i].Number;
            float boostRate = (float) scriptResultRates.Tuple[i + 1].Number;
            if (!nextLevelItem.Stats.LimitBreakEnchants.ContainsKey(attribute))
            {
                if (boostRate == 0)
                {
                    continue;
                }
                ItemStat stat = new BasicStat(attribute, boostRate, StatAttributeType.Rate);
                if (attribute > (StatAttribute) 11000)
                {
                    stat = new SpecialStat(attribute, boostRate, StatAttributeType.Rate);
                }
                stat.ItemAttribute = attribute;

                nextLevelItem.Stats.LimitBreakEnchants[attribute] = stat;
                continue;
            }
            nextLevelItem.Stats.LimitBreakEnchants[attribute].Rate += boostRate;
        }

        for (int i = 0; i < scriptResultFlats.Tuple.Length; i += 2)
        {
            if (scriptResultFlats.Tuple[i].Number == 0)
            {
                continue;
            }
            StatAttribute attribute = (StatAttribute) scriptResultFlats.Tuple[i].Number;
            int boostValue = (int) scriptResultFlats.Tuple[i + 1].Number;
            if (!nextLevelItem.Stats.LimitBreakEnchants.ContainsKey(attribute))
            {
                ItemStat stat = new BasicStat(attribute, boostValue, StatAttributeType.Flat);
                if (attribute > (StatAttribute) 11000)
                {
                    stat = new SpecialStat(attribute, boostValue, StatAttributeType.Flat);
                }
                stat.ItemAttribute = attribute;

                nextLevelItem.Stats.LimitBreakEnchants[attribute] = stat;
                continue;
            }
            nextLevelItem.Stats.LimitBreakEnchants[attribute].Flat += boostValue;
        }
        nextLevelItem.LimitBreakLevel++;
        return nextLevelItem;
    }
}
