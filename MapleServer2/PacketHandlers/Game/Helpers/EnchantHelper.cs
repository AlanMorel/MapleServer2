using Maple2Storage.Enums;
using MapleServer2.Tools;
using MapleServer2.Types;
using MoonSharp.Interpreter;

namespace MapleServer2.PacketHandlers.Game.Helpers;

public static class EnchantHelper
{
    public static Dictionary<StatAttribute, ItemStat> GetEnchantStats(int enchantLevel, ItemType itemType, int itemLevel)
    {
        Dictionary<StatAttribute, ItemStat> enchantStats = new();
        ScriptLoader scriptLoader = new("Functions/calcEnchantValues");
        DynValue statValueScriptResult = scriptLoader.Call("calcEnchantBoostValues", enchantLevel, (int) itemType, itemLevel);

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
}
