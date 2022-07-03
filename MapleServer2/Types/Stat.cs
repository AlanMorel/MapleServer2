using Maple2Storage.Enums;
using Maple2Storage.Types;

namespace MapleServer2.Types;

/* Bonus = Base + stat allocations + bonuses.
 * Base = Base stat amount.
 * Total = Final value (e.g. capped Damage, current Hp, active CritRate, ...)
 *
 * Change Stat.Bonus for temporary changes.
 */
public struct StatValue
{
    public long BonusLong;
    public long BaseLong;
    public long TotalLong;
    public int Bonus { get => (int) BonusLong; set { BonusLong = value; } }
    public int Base { get => (int) BaseLong; set { BaseLong = value; } }
    public int Total { get => (int) TotalLong; set { TotalLong = value; } }
    public long BonusAmountLong { get => BonusLong - BaseLong; }
    public int BonusAmount { get => (int) BonusAmountLong; }
}

/* Flat is only used on StatAttributeType.Flat
 * Rate can be used on StatAttributeType.Flat or StatAttributeType.Rate, but the meaning of the two changes based on context.
 * On StatAttributeType.Flat the rates for a single stat type will be accumulated, and the bonus and total will be multiplied by the accumulated rate at the end
 * On StatAttributeType.Rate the rates will just be accumulated as 1000 * rate to support up to a tenth of a percent of precision
 */
public struct StatModifier
{
    public StatAttributeType Type;
    public long Flat;
    public float Rate;
}

public class Stat
{
    public StatValue Value;
    public StatModifier Modifier;
    public int Bonus { get => (int) Value.BonusLong; }
    public int Base { get => (int) Value.BaseLong; }
    public int Total { get => (int) Value.TotalLong; }
    public int BonusAmount { get => (int) Value.BonusAmountLong; }
    public long BonusLong { get => Value.BonusLong; }
    public long BaseLong { get => Value.BaseLong; }
    public long TotalLong { get => Value.TotalLong; }
    public long BonusAmountLong { get => Value.BonusAmountLong; }

    public Stat() { }

    public Stat(XmlStat<int> statInt, StatAttributeType type = StatAttributeType.Flat)
    {
        Value.Bonus = statInt.Bonus;
        Value.Base = statInt.Base;
        Value.Total = statInt.Total;
        Modifier.Type = type;
        Modifier.Rate = 1;
    }

    public Stat(XmlStat<long> statLong, StatAttributeType type = StatAttributeType.Flat)
    {
        Value.BonusLong = statLong.Bonus;
        Value.BaseLong = statLong.Base;
        Value.TotalLong = statLong.Total;
        Modifier.Type = type;
        Modifier.Rate = 1;
    }

    public Stat(int totalStat, StatAttributeType type = StatAttributeType.Flat) : this(totalStat, totalStat, totalStat, type) { }

    public Stat(int bonusStat, int baseStat, int totalStat, StatAttributeType type = StatAttributeType.Flat)
    {
        Value.Bonus = bonusStat;
        Value.Base = baseStat;
        Value.Total = totalStat;
        Modifier.Type = type;
        Modifier.Rate = 1;
    }

    public Stat(long totalStat, StatAttributeType type = StatAttributeType.Flat) : this(totalStat, totalStat, totalStat, type) { }

    public Stat(long bonusStat, long baseStat, long totalStat, StatAttributeType type = StatAttributeType.Flat)
    {
        Value.BonusLong = bonusStat;
        Value.BaseLong = baseStat;
        Value.TotalLong = totalStat;
        Modifier.Type = type;
        Modifier.Rate = 1;
    }

    public int this[int i] => i switch
    {
        0 => Bonus,
        1 => Base,
        _ => Total
    };

    public void Reset()
    {
        Value.Bonus = 0;
        Value.Base = 0;
        Value.Total = 0;
        Modifier.Flat = 0;
        Modifier.Rate = 1;
    }

    public void Add(StatModifier modifier)
    {
        if (modifier.Type != Modifier.Type)
        {
            throw new ArgumentException($"Attempt to add mismatching stat type {modifier.Type} to type {Modifier.Type}");
        }

        Modifier.Flat += modifier.Flat;
        Modifier.Rate += modifier.Rate;
    }

    public void Add(int flat, float rate = 0)
    {
        Modifier.Flat += flat;
        Modifier.Rate += rate;
    }

    public void Add(long flat, float rate = 0)
    {
        Modifier.Flat += flat;
        Modifier.Rate += rate;
    }

    public void Add(float rate)
    {
        Modifier.Rate += rate;
    }

    public long ComputeValue()
    {
        if (Modifier.Type == StatAttributeType.Flat)
        {
            return (long) (Modifier.Rate * (Value.BaseLong + Modifier.Flat));
        }

        return (long) (Value.BaseLong + Modifier.Flat + 1000 * Modifier.Rate);
    }

    public void ComputeBonus()
    {
        Value.BonusLong = ComputeValue();
        Value.TotalLong = Value.BonusLong;
    }

    public void ComputeBase()
    {
        Value.BaseLong = ComputeValue();
    }

    public void AddBase(int value)
    {
        Value.Base += value;
    }

    public void AddBase(long value)
    {
        Value.BaseLong += value;
    }

    public void AddBonus(int value)
    {
        Value.Bonus += value;
        Value.Total += value;
    }

    public void AddBonus(long value)
    {
        Value.BonusLong += value;
        Value.TotalLong += value;
    }

    public void AddValue(int value)
    {
        SetValue(Total + value);
    }

    public void AddValue(long value)
    {
        SetValue(TotalLong + value);
    }

    public void SetValue(int value)
    {
        Value.Total = Math.Clamp(value, 0, Bonus);
    }

    public void SetValue(long value)
    {
        Value.TotalLong = Math.Clamp(value, 0, BonusLong);
    }
}
