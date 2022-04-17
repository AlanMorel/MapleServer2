using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Tools;
using MoonSharp.Interpreter;

namespace MapleServer2.Types;

public static class ConstantStats
{
    public static void GetStats(Item item, int optionId, float optionLevelFactor, out Dictionary<StatAttribute, ItemStat> constantStats)
    {
        constantStats = new();
        int constantId = ItemMetadataStorage.GetOptionConstant(item.Id);
        ItemOptionsConstant basicOptions = ItemOptionConstantMetadataStorage.GetMetadata(constantId, item.Rarity);
        if (basicOptions == null)
        {
            GetDefault(item, constantStats, optionId, optionLevelFactor);
            return;
        }

        foreach (ParserStat stat in basicOptions.Stats)
        {
            constantStats[stat.Attribute] = new BasicStat(stat);
        }
        foreach (ParserSpecialStat stat in basicOptions.SpecialStats)
        {
            constantStats[stat.Attribute] = new SpecialStat(stat);
        }

        // TODO: Implement Hidden ndd (defense) and wapmax (Max Weapon Attack)

        if (optionLevelFactor > 50)
        {
            GetDefault(item, constantStats, optionId, optionLevelFactor);
        }
    }

    private static void GetDefault(Item item, Dictionary<StatAttribute, ItemStat> constantStats, int optionId, float optionLevelFactor)
    {
        ItemOptionPick baseOptions = ItemOptionPickMetadataStorage.GetMetadata(optionId, item.Rarity);
        if (baseOptions is null)
        {
            return;
        }

        Script script = ScriptLoader.GetScript("Functions/calcItemValues");

        foreach (ConstantPick constantPick in baseOptions.Constants)
        {
            string calcScript;
            switch (constantPick.Stat)
            {
                case StatAttribute.Hp:
                    calcScript = "constant_value_hp";
                    break;
                case StatAttribute.Defense:
                    calcScript = "constant_value_ndd";
                    break;
                case StatAttribute.MagicRes:
                    calcScript = "constant_value_mar";
                    break;
                case StatAttribute.PhysicalRes:
                    calcScript = "constant_value_par";
                    break;
                case StatAttribute.CritRate:
                    calcScript = "constant_value_cap";
                    break;
                case StatAttribute.Str:
                    calcScript = "constant_value_str";
                    break;
                case StatAttribute.Dex:
                    calcScript = "constant_value_dex";
                    break;
                case StatAttribute.Int:
                    calcScript = "constant_value_int";
                    break;
                case StatAttribute.Luk:
                    calcScript = "constant_value_luk";
                    break;
                case StatAttribute.MagicAtk:
                    calcScript = "constant_value_map";
                    break;
                case StatAttribute.MinWeaponAtk:
                    calcScript = "constant_value_wapmin";
                    break;
                case StatAttribute.MaxWeaponAtk:
                    calcScript = "constant_value_wapmax";
                    break;
                default:
                    continue;
            }

            if (!constantStats.ContainsKey(constantPick.Stat))
            {
                constantStats[constantPick.Stat] = new BasicStat(constantPick.Stat, 0, StatAttributeType.Flat);
            }

            float statValue = constantStats[constantPick.Stat].GetValue();
            DynValue result = script.RunFunction(calcScript, statValue, constantPick.DeviationValue, (int) item.Type,
                (int) item.RecommendJobs.First(), optionLevelFactor, item.Rarity, item.Level);

            constantStats[constantPick.Stat].SetValue((float) result.Number);
            if (constantStats[constantPick.Stat].GetValue() <= 0.0000f)
            {
                constantStats.Remove(constantPick.Stat);
            }
        }
    }
}
