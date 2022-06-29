﻿using Maple2Storage.Types;

namespace MapleServer2.Types;

/* Bonus = Base + stat allocations + bonuses.
 * Base = Base stat amount.
 * Total = Final value (e.g. capped Damage, current Hp, active CritRate, ...)
 *
 * Change Stat.Bonus for temporary changes.
 */
public class Stat
{
    public long BonusLong;
    public long BaseLong;
    public long TotalLong;
    public int Bonus { get => (int) BonusLong; protected set { BonusLong = value; } }
    public int Base { get => (int) BaseLong; protected set { BaseLong = value; } }
    public int Total { get => (int) TotalLong; protected set { TotalLong = value; } }

    public Stat() { }

    public Stat(XmlStat<int> statInt)
    {
        Bonus = statInt.Bonus;
        Base = statInt.Base;
        Total = statInt.Total;
    }

    public Stat(XmlStat<long> statLong)
    {
        BonusLong = statLong.Bonus;
        BaseLong = statLong.Base;
        TotalLong = statLong.Total;
    }

    public Stat(int totalStat) : this(totalStat, totalStat, totalStat) { }

    public Stat(int bonusStat, int baseStat, int totalStat)
    {
        Bonus = bonusStat;
        Base = baseStat;
        Total = totalStat;
    }

    public Stat(long totalStat) : this(totalStat, totalStat, totalStat) { }

    public Stat(long bonusStat, long baseStat, long totalStat)
    {
        BonusLong = bonusStat;
        BaseLong = baseStat;
        TotalLong = totalStat;
    }

    public int this[int i] => i switch
    {
        0 => Bonus,
        1 => Base,
        _ => Total
    };

    public void IncreaseBonus(int amount)
    {
        Bonus += amount;
        Total += amount;
    }

    public void IncreaseBonus(long amount)
    {
        BonusLong += amount;
        TotalLong += amount;
    }

    public void IncreaseBase(int amount)
    {
        Bonus += amount;
        Base += amount;
        Total += amount;
    }

    public void IncreaseBase(long amount)
    {
        BonusLong += amount;
        BaseLong += amount;
        TotalLong += amount;
    }

    public void Increase(int amount)
    {
        Total = Math.Min(Bonus, Total + amount);
    }

    public void Increase(long amount)
    {
        TotalLong = Math.Min(BonusLong, TotalLong + amount);
    }

    public void DecreaseBonus(int amount)
    {
        int newMax = Math.Clamp(Bonus - amount, Base, Bonus);
        int newCurrent = Total - amount;
        //if (statIndex == PlayerStatId.Hp || statIndex == PlayerStatId.Spirit)
        //{
        //    newCurrent = Math.Clamp(newCurrent, 50, newMax); // TODO: Find Hp/Sp reset formula
        //}
        Bonus = newMax;
        Total = newCurrent;
    }

    public void DecreaseBonus(long amount)
    {
        long newMax = Math.Clamp(BonusLong - amount, BaseLong, BonusLong);
        long newCurrent = Total - amount;
        //if (statIndex == PlayerStatId.Hp || statIndex == PlayerStatId.Spirit)
        //{
        //    newCurrent = Math.Clamp(newCurrent, 50, newMax); // TODO: Find Hp/Sp reset formula
        //}
        BonusLong = newMax;
        TotalLong = newCurrent;
    }

    public void DecreaseBase(int amount)
    {
        // Clamp Min to 0?
        Bonus -= amount;
        Base -= amount;
        Total -= amount;
    }

    public void DecreaseBase(long amount)
    {
        // Clamp Min to 0?
        BonusLong -= amount;
        BaseLong -= amount;
        TotalLong -= amount;
    }

    public void Decrease(int amount)
    {
        Total -= Math.Clamp(amount, 0, Total);
    }

    public void Decrease(long amount)
    {
        TotalLong -= amount;
    }

    public void Reset()
    {
        Bonus = 0;
        Base = 0;
        Total = 0;
    }
}
