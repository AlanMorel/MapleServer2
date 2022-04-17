using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Tools;
using MoonSharp.Interpreter;

namespace MapleServer2.Types;

public abstract class ItemStat
{
    public StatAttribute ItemAttribute;
    public StatAttributeType AttributeType;
    public int Flat;
    public float Rate;

    public ItemStat() { }

    protected ItemStat(StatAttribute attribute, StatAttributeType type, float value)
    {
        ItemAttribute = attribute;
        AttributeType = type;
        SetValue(value);
    }

    public void SetValue(float value)
    {
        if (AttributeType == StatAttributeType.Flat)
        {
            Flat = (int) value;
            return;
        }

        Rate = value;
    }

    public void SetEnchantValues(int flat, float addRate)
    {
        Flat = flat;
        Rate = addRate;
    }

    public float GetValue()
    {
        if (AttributeType == StatAttributeType.Flat)
        {
            return Flat;
        }
        return Rate;
    }

    public short WriteAttribute()
    {
        if (ItemAttribute > (StatAttribute) 11000)
        {
            return (short) (ItemAttribute - 11000);
        }
        return (short) ItemAttribute;
    }
}

public class BasicStat : ItemStat
{
    public BasicStat() { }
    public BasicStat(StatAttribute attribute, float value, StatAttributeType type) : base(attribute, type, value) { }

    public BasicStat(ParserStat parsedStat) : base(parsedStat.Attribute, parsedStat.AttributeType, parsedStat.Value) { }
}

public class SpecialStat : ItemStat
{
    public SpecialStat() { }
    public SpecialStat(StatAttribute attribute, float value, StatAttributeType type) : base(attribute, type, value) { }

    public SpecialStat(ParserSpecialStat parsedStat) : base(parsedStat.Attribute, parsedStat.AttributeType, parsedStat.Value) { }
}

public class Gemstone
{
    public int Id;
    public long OwnerId = 0;
    public string OwnerName = "";
    public bool IsLocked;
    public long UnlockTime;
}

public class GemSocket
{
    public bool IsUnlocked;
    public Gemstone Gemstone;
}

public class ItemStats
{
    public Dictionary<StatAttribute, ItemStat> Constants;
    public Dictionary<StatAttribute, ItemStat> Statics;
    public Dictionary<StatAttribute, ItemStat> Randoms;
    public Dictionary<StatAttribute, ItemStat> Enchants;
    public Dictionary<StatAttribute, ItemStat> LimitBreakEnchants;
    public List<GemSocket> GemSockets;

    public ItemStats() { }

    public ItemStats(Item item)
    {
        CreateNewStats(item);
    }

    public ItemStats(ItemStats other)
    {
        Constants = CopyStats(other.Constants);
        Statics = CopyStats(other.Statics);
        Randoms = CopyStats(other.Randoms);
        Enchants = CopyStats(other.Enchants);
        LimitBreakEnchants = CopyStats(other.LimitBreakEnchants);
        GemSockets = new();
    }

    private void CreateNewStats(Item item)
    {
        Constants = new();
        Statics = new();
        Randoms = new();
        Enchants = new();
        LimitBreakEnchants = new();
        GemSockets = new();
        if (item.Rarity is 0 or > 6)
        {
            return;
        }

        int optionId = ItemMetadataStorage.GetOptionId(item.Id);
        float optionLevelFactor = ItemMetadataStorage.GetOptionLevelFactor(item.Id);

        ConstantStats.GetStats(item, optionId, optionLevelFactor, out Dictionary<StatAttribute, ItemStat> constantStats);
        Constants = constantStats;
        StaticStats.GetStats(item, optionId, optionLevelFactor, out Dictionary<StatAttribute, ItemStat> staticStats);
        Statics = staticStats;
        RandomStats.GetStats(item, out Dictionary<StatAttribute, ItemStat> randomStats);
        Randoms = randomStats;
        if (item.EnchantLevel > 0)
        {
            Enchants = EnchantHelper.GetEnchantStats(item.EnchantLevel, item.Type, item.Level);
        }
        GetGemSockets(item, optionLevelFactor);
    }

    private static Dictionary<StatAttribute, ItemStat> CopyStats(Dictionary<StatAttribute, ItemStat> otherStats)
    {
        Dictionary<StatAttribute, ItemStat> stats = new();
        foreach ((StatAttribute key, ItemStat value) in otherStats)
        {
            if (value is BasicStat)
            {
                stats[key] = new BasicStat(value.ItemAttribute, value.Flat + value.Rate, value.AttributeType);
            }
            else
            {
                stats[key] = new SpecialStat(value.ItemAttribute, value.Flat + value.Rate, value.AttributeType);
            }
        }
        return stats;
    }

    public float GetTotalStatValue(StatAttribute attribute)
    {
        float statValue = 0;

        if (Constants.ContainsKey(attribute))
        {
            statValue += Constants[attribute].GetValue();
        }
        if (Statics.ContainsKey(attribute))
        {
            statValue += Statics[attribute].GetValue();
        }
        if (Randoms.ContainsKey(attribute))
        {
            statValue += Randoms[attribute].GetValue();
        }
        if (Enchants.ContainsKey(attribute))
        {
            statValue += Enchants[attribute].Flat;
            statValue += statValue * Enchants[attribute].Rate;
        }
        if (LimitBreakEnchants.ContainsKey(attribute))
        {
            statValue += LimitBreakEnchants[attribute].Flat;
            statValue += statValue * LimitBreakEnchants[attribute].Rate;
        }

        return statValue;
    }

    public static void AddHiddenNormalStat(List<ItemStat> stats, StatAttribute attribute, int value, float calibrationFactor)
    {
        ItemStat normalStat = stats.FirstOrDefault(x => x.ItemAttribute == attribute);
        if (normalStat == null)
        {
            return;
        }
        int calibratedValue = (int) (value * calibrationFactor);

        int index = stats.FindIndex(x => x.ItemAttribute == attribute);
        int biggerValue = Math.Max(value, calibratedValue);
        int smallerValue = Math.Min(value, calibratedValue);
        int summedFlat = (int) (normalStat.Flat + Random.Shared.Next(smallerValue, biggerValue));
        stats[index] = new SpecialStat(normalStat.ItemAttribute, summedFlat, StatAttributeType.Flat);
    }

    private void GetGemSockets(Item item, float optionLevelFactor)
    {
        // Check for predefined sockets
        int socketId = ItemMetadataStorage.GetSocketDataId(item.Id);
        if (socketId != 0)
        {
            ItemSocketRarityData socketData = ItemSocketMetadataStorage.GetMetadata(socketId, item.Rarity);
            if (socketData is not null)
            {
                for (int i = 0; i < socketData.MaxCount; i++)
                {
                    GemSockets.Add(new());
                }

                for (int j = 0; j < socketData.FixedOpenCount; j++)
                {
                    GemSockets[j].IsUnlocked = true;
                }
                return;
            }
        }

        Script script = ScriptLoader.GetScript("Functions/calcItemSocketMaxCount");
        DynValue dynValue = script.RunFunction("calcItemSocketMaxCount", (int) item.Type, item.Rarity, optionLevelFactor, (int) item.InventoryTab);
        int slotAmount = (int) dynValue.Number;
        if (slotAmount <= 0)
        {
            return;
        }

        // add sockets
        for (int i = 0; i < slotAmount; i++)
        {
            GemSocket socket = new();
            GemSockets.Add(socket);
        }

        // roll to unlock sockets
        for (int i = 0; i < GemSockets.Count; i++)
        {
            int successNumber = Random.Shared.Next(0, 100);

            // 5% success rate to unlock a gemsocket
            if (successNumber < 95)
            {
                break;
            }
            GemSockets[i].IsUnlocked = true;
        }
    }
}
