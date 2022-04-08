using Maple2Storage.Enums;
using MapleServer2.Enums;

namespace MapleServer2.Types;

public class ItemEnchant
{
    public long ItemUid;
    public int Level;
    public EnchantRates Rates;
    public List<EnchantIngredient> Ingredients = new();
    public Dictionary<StatAttribute, EnchantStats> Stats = new();
    public int CatalystAmountRequired;
    public int PityCharges;
    public List<long> CatalystItemUids = new();

    public ItemEnchant(long itemUid, int level)
    {
        ItemUid = itemUid;
        Level = level;
        Rates = new();
    }

    public void UpdateCharges(int charges)
    {
        Rates.ChargesAdded = charges;
    }

    public void UpdateAdditionalCatalysts(long itemUid, int addCatalyst, bool add)
    {
        if (add)
        {
            CatalystItemUids.Add(itemUid);
            if (CatalystItemUids.Count > CatalystAmountRequired)
            {
                Rates.AdditionalCatalysts++;
            }
            return;
        }

        if (CatalystItemUids.Count >= CatalystAmountRequired)
        {
            Rates.AdditionalCatalysts--;
        }
        CatalystItemUids.Remove(itemUid);
    }
}

public class EnchantIngredient
{
    public ItemStringTag Tag;
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

    public EnchantRates(float baseSuccessRate)
    {
        BaseSuccessRate = baseSuccessRate;
    }
}

public class EnchantStats
{
    public StatAttribute Attribute;
    public int AddValue;
    public float AddRate;

    public EnchantStats(StatAttribute attribute, int addValue, float addRate)
    {
        Attribute = attribute;
        AddValue = addValue;
        AddRate = addRate;
    }
}
