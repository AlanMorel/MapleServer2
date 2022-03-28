using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Tools;
using MoonSharp.Interpreter;

namespace MapleServer2.Types;

public static class StaticStats
{
    public static void GetStats(Item item, int optionId, float optionLevelFactor, float globalOptionLevelFactor, out List<ItemStat> staticStats)
    {
        staticStats = new();
        if (optionLevelFactor < 50)
        {
            return;
        }
        int staticId = ItemMetadataStorage.GetOptionStatic(item.Id);

        ItemOptionsStatic staticOptions = ItemOptionStaticMetadataStorage.GetMetadata(staticId, item.Rarity);
        if (staticOptions == null)
        {
            GetDefault(item, staticStats, optionId, optionLevelFactor, globalOptionLevelFactor);
            return;
        }

        foreach (ParserStat stat in staticOptions.Stats)
        {
            staticStats.Add(new BasicStat(stat));
        }
        foreach (ParserSpecialStat stat in staticOptions.SpecialStats)
        {
            staticStats.Add(new SpecialStat(stat));
        }

        // TODO: Implement Hidden ndd (defense) and wapmax (Max Weapon Attack)

        GetDefault(item, staticStats, optionId, optionLevelFactor, globalOptionLevelFactor);
    }

    private static void GetDefault(Item item, List<ItemStat> stats, int optionId, float optionLevelFactor, float globalOptionLevelFactor)
    {
        ItemOptionPick baseOptions = ItemOptionPickMetadataStorage.GetMetadata(optionId, item.Rarity);
        if (baseOptions is null)
        {
            return;
        }

        ScriptLoader scriptLoader = new("Functions/calcItemValues");
        float currentStatValue = 0;
        ItemStat stat = null;
        int statIndex = -1;
        foreach (StaticPick staticPickFlat in baseOptions.StaticValues)
        {
            stat = stats.FirstOrDefault(x => x.ItemAttribute == staticPickFlat.Stat);
            if (stat?.ItemAttribute == StatAttribute.MaxWeaponAtk && stat?.Flat != 0)
            {
                continue;
            }
            if (stat is null)
            {
                stat = new BasicStat(staticPickFlat.Stat, 0, StatAttributeType.Flat);
                stats.Add(stat);
            }
            statIndex = stats.FindIndex(x => x.ItemAttribute == staticPickFlat.Stat);
            currentStatValue = stat.GetValue();

            double statValue = CalculateStat(item, optionLevelFactor, globalOptionLevelFactor, staticPickFlat, scriptLoader, currentStatValue);
            if (statValue == 0)
            {
                continue;
            }

            stat.SetValue((int) statValue);

            stats[statIndex] = stat;
        }

        foreach (StaticPick staticPickRate in baseOptions.StaticRates)
        {
            stat = stats.FirstOrDefault(x => x.ItemAttribute == staticPickRate.Stat);
            if (stat is null)
            {
                stat = new BasicStat(staticPickRate.Stat, 0, StatAttributeType.Rate);
                stats.Add(stat);
            }
            statIndex = stats.FindIndex(x => x.ItemAttribute == staticPickRate.Stat);
            currentStatValue = stat.GetValue();

            double statValue = CalculateStat(item, optionLevelFactor, globalOptionLevelFactor, staticPickRate, scriptLoader, currentStatValue);
            if (statValue == 0)
            {
                continue;
            }

            stat.SetValue((float) statValue);

            stats[statIndex] = stat;
        }
    }
    private static double CalculateStat(Item item, float optionLevelFactor, float globalOptionLevelFactor, StaticPick staticPick, ScriptLoader scriptLoader, float currentStatValue)
    {

        Random random = Random.Shared;
        ;
        string calcScript = "";
        switch (staticPick.Stat)
        {
            case StatAttribute.Hp:
                calcScript = "static_value_hp";
                break;
            case StatAttribute.Defense: // TODO: this is not calculating correctly
                calcScript = "static_value_ndd";
                break;
            case StatAttribute.MagicRes:
                calcScript = "static_value_mar";
                break;
            case StatAttribute.PhysicalRes:
                calcScript = "static_value_par";
                break;
            case StatAttribute.PhysicalAtk:
                calcScript = "static_value_pap";
                break;
            case StatAttribute.MagicAtk:
                calcScript = "static_value_map";
                break;
            case StatAttribute.MaxWeaponAtk:
                calcScript = "static_value_wapmax";
                break;
            case StatAttribute.PerfectGuard:
                calcScript = "static_rate_abp";
                break;
            default:
                return 0;
        }


        DynValue result = scriptLoader.Call(calcScript, currentStatValue, staticPick.DeviationValue, (int) item.Type,
            (int) item.RecommendJobs.First(), optionLevelFactor, item.Rarity, globalOptionLevelFactor);

        if (result.Tuple.Length < 2)
        {
            return 0;
        }

        if (result.Tuple[0].Number == 0 && result.Tuple[1].Number == 0)
        {
            return 0;
        }

        // Get random between min and max values
        double statValue = random.NextDouble() * (result.Tuple[1].Number - result.Tuple[0].Number) + result.Tuple[0].Number;
        return statValue;
    }
}
