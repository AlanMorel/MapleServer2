using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using MoonSharp.Interpreter;

namespace MapleServer2.PacketHandlers.Game.Helpers;

public static class EnchantHelper
{
    public static Dictionary<StatAttribute, ItemStat> GetEnchantStats(int enchantLevel, ItemType itemType, int itemLevel)
    {
        Dictionary<StatAttribute, ItemStat> enchantStats = new();
        Script script = ScriptLoader.GetScript("Functions/calcEnchantValues");
        DynValue statValueScriptResult = script.RunFunction("calcEnchantBoostValues", enchantLevel, (int) itemType, itemLevel);

        for (int i = 0; i < statValueScriptResult.Tuple.Length; i += 2)
        {
            if (statValueScriptResult.Tuple[i].Number == 0)
            {
                continue;
            }

            StatAttribute attribute = (StatAttribute) statValueScriptResult.Tuple[i].Number;
            float boostRate = (float) statValueScriptResult.Tuple[i + 1].Number;
            enchantStats[attribute] = new BasicStat(attribute, boostRate, StatAttributeType.Rate);
        }
        return enchantStats;
    }

    public static bool PlayerHasIngredients(ItemEnchant itemEnchantStats, IInventory inventory)
    {
        foreach (EnchantIngredient ingredient in itemEnchantStats.Ingredients)
        {
            IReadOnlyCollection<Item> ingredientTotal = inventory.GetAllByTag(ingredient.Tag.ToString());
            if (ingredientTotal.Sum(x => x.Amount) < ingredient.Amount)
            {
                return false;
            }
        }
        return true;
    }

    public static bool PlayerHasRequiredCatalysts(ItemEnchant itemEnchantStats)
    {
        return itemEnchantStats.CatalystAmountRequired <= itemEnchantStats.CatalystItemUids.Count;
    }

    public static void TalkError(GameSession session, ItemEnchant itemEnchant)
    {
        int npcId = itemEnchant.Type switch
        {
            EnchantType.Ophelia => 11000508,
            _ => 11000510
        };
        ScriptMetadata scriptMetadata = ScriptMetadataStorage.GetNpcScriptMetadata(npcId);
        NpcScript npcScript = scriptMetadata?.NpcScripts.FirstOrDefault(x => x.Id == 31);
        if (npcScript is null)
        {
            return;
        }

        Script script = ScriptLoader.GetScript($"Npcs/{npcId}", session);
        int eventId = (int) script.RunFunction("getProcessEventId", PlayerHasIngredients(itemEnchant, session.Player.Inventory),
            PlayerHasRequiredCatalysts(itemEnchant)).Number;

        if (eventId == 0)
        {
            return;
        }

        HandleNpcTalkEventType(session, npcScript, eventId);
    }

    public static void ExcessCatalystError(GameSession session, ItemEnchant itemEnchant)
    {
        int npcId = itemEnchant.Type switch
        {
            EnchantType.Ophelia => 11000508,
            _ => 11000510
        };
        ScriptMetadata scriptMetadata = ScriptMetadataStorage.GetNpcScriptMetadata(npcId);
        NpcScript npcScript = scriptMetadata?.NpcScripts.FirstOrDefault(x => x.Id == 31);
        if (npcScript is null)
        {
            return;
        }

        Script script = ScriptLoader.GetScript($"Npcs/{npcId}", session);
        int eventId = (int) script.RunFunction("getExcessCatalystEventId").Number;

        if (eventId == 0)
        {
            return;
        }

        HandleNpcTalkEventType(session, npcScript, eventId);
    }

    public static void HandleNpcTalkEventType(GameSession session, NpcScript npcScript, int eventId)
    {
        ScriptEvent scriptEvent = npcScript.Contents.First().Events.FirstOrDefault(x => x.Id == eventId);
        int indexContent = Random.Shared.Next(scriptEvent.Contents.Count);
        EventContent eventContents = scriptEvent.Contents[indexContent];
        session.Send(NpcTalkPacket.CustomText(eventContents.Text, eventContents.VoiceId, eventContents.Illustration));
    }
}
