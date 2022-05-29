using Maple2Storage.Enums;
using MapleServer2.Enums;

namespace MapleServer2.Types;

public class ItemEnchant
{
    public readonly EnchantType Type;
    public readonly long ItemUid;
    public int Level;
    public readonly EnchantRates Rates;
    public readonly List<EnchantIngredient> Ingredients = new();
    public Dictionary<StatAttribute, ItemStat> Stats = new();
    public int CatalystAmountRequired;
    public int PityCharges;
    public readonly List<long> CatalystItemUids = new();
    public bool Success;

    public ItemEnchant(EnchantType type, long itemUid, int level)
    {
        Type = type;
        ItemUid = itemUid;
        Level = level;
        Rates = new();
    }

    public void UpdateCharges(int charges)
    {
        Rates.ChargesAdded = charges;
    }

    public bool UpdateAdditionalCatalysts(long itemUid, bool add)
    {
        if (add)
        {
            if (CatalystItemUids.Count >= CatalystAmountRequired)
            {
                if (Rates.BaseSuccessRate + Rates.CatalystTotalRate() >= 30)
                {
                    return false;
                }
                Rates.AdditionalCatalysts++;
            }
            CatalystItemUids.Add(itemUid);
            return true;
        }

        if (CatalystItemUids.Count > CatalystAmountRequired)
        {
            Rates.AdditionalCatalysts--;
        }
        CatalystItemUids.Remove(itemUid);
        return true;
    }
}

public class EnchantIngredient
{
    public readonly ItemStringTag Tag;
    public int Amount;

    public EnchantIngredient(ItemStringTag tag, int amount)
    {
        Tag = tag;
        Amount = amount;
    }
}

public class EnchantRates
{
    public float BaseSuccessRate;
    public int AdditionalCatalysts;
    public float CatalystRate;
    public int ChargesAdded;
    public float ChargesRate;

    public EnchantRates() { }

    public float ChargeTotalRate()
    {
        return ChargesAdded * ChargesRate;
    }

    public float CatalystTotalRate()
    {
        if (BaseSuccessRate >= 30)
        {
            return 0;
        }
        return Math.Min(30 - BaseSuccessRate, CatalystRate * AdditionalCatalysts);
    }

    public float TotalSuccessRate()
    {
        return BaseSuccessRate + CatalystTotalRate() + ChargeTotalRate();
    }
}
