using Maple2Storage.Enums;
using MapleServer2.Enums;

namespace MapleServer2.Types;

public class ItemEnchant
{
    public readonly long ItemUid;
    public readonly int Level;
    public readonly EnchantRates Rates;
    public readonly List<EnchantIngredient> Ingredients = new();
    public Dictionary<StatAttribute, ItemStat> Stats = new();
    public int CatalystAmountRequired;
    public int PityCharges;
    public readonly List<long> CatalystItemUids = new();

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

    public EnchantRates(float baseSuccessRate)
    {
        BaseSuccessRate = baseSuccessRate;
    }
}
