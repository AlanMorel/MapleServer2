using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace MapleServer2.Types;

// Player Stats in Packet Order - Count: 35 (0x23)
public enum StatId : byte
{
    Str = 0x00,
    Dex = 0x01,
    Int = 0x02,
    Luk = 0x03,
    Hp = 0x04, // long
    HpRegen = 0x05,
    HpRegenTime = 0x06, // base (3000ms)
    Spirit = 0x07,
    SpRegen = 0x08,
    SpRegenTime = 0x09, // base (200ms)
    Stamina = 0x0A, // base 120 (20 = 1 block)
    StaRegen = 0x0B, // base 10  (10 = 0.5 block)
    StaRegenTime = 0x0C, // base (500ms)
    AtkSpd = 0x0D,
    MoveSpd = 0x0E,
    Acc = 0x0F,
    Eva = 0x10,
    CritRate = 0x11,
    CritDmg = 0x12,
    CritEva = 0x13,
    Def = 0x14,
    Guard = 0x15,
    JumpHeight = 0x16,
    PhysAtk = 0x17,
    MagAtk = 0x18,
    PhysRes = 0x19,
    MagRes = 0x1A,
    MinAtk = 0x1B,
    MaxAtk = 0x1C,
    MinDmg = 0x1D,
    MaxDmg = 0x1E,
    Pierce = 0x1F,
    MountSpeed = 0x20,
    BonusAtk = 0x21,
    PetBonusAtk = 0x22, // base 0 (bonuses can be added)
}

public class Stats
{
    // TODO: Handle stat allocation in here?
    public Dictionary<StatId, Stat> Data;

    public Stats() { }

    public Stats(NpcMetadata metadata)
    {
        Data = new()
        {
            {
                StatId.Str,
                new Stat(metadata.Stats.Str)
            },
            {
                StatId.Dex,
                new Stat(metadata.Stats.Dex)
            },
            {
                StatId.Int,
                new Stat(metadata.Stats.Int)
            },
            {
                StatId.Luk,
                new Stat(metadata.Stats.Luk)
            },
            {
                StatId.Hp,
                new Stat(metadata.Stats.Hp)
            }, // Max = 0 on login
            {
                StatId.HpRegen,
                new Stat(metadata.Stats.HpRegen)
            },
            {
                StatId.HpRegenTime,
                new Stat(metadata.Stats.HpInterval)
            },
            {
                StatId.Spirit,
                new Stat(metadata.Stats.Sp)
            }, // Max = 0 on login
            {
                StatId.SpRegen,
                new Stat(metadata.Stats.SpRegen)
            },
            {
                StatId.SpRegenTime,
                new Stat(metadata.Stats.SpInterval)
            },
            {
                StatId.Stamina,
                new Stat(metadata.Stats.Ep)
            }, // Max = 0 on login
            {
                StatId.StaRegen,
                new Stat(metadata.Stats.EpRegen)
            },
            {
                StatId.StaRegenTime,
                new Stat(metadata.Stats.EpInterval)
            },
            {
                StatId.AtkSpd,
                new Stat(metadata.Stats.AtkSpd)
            },
            {
                StatId.MoveSpd,
                new Stat(metadata.Stats.MoveSpd)
            },
            {
                StatId.Acc,
                new Stat(metadata.Stats.Accuracy)
            },
            {
                StatId.Eva,
                new Stat(metadata.Stats.Evasion)
            }, // changes with job
            {
                StatId.CritRate,
                new Stat(metadata.Stats.CritRate)
            }, // changes with job
            {
                StatId.CritDmg,
                new Stat(metadata.Stats.CritDamage)
            },
            {
                StatId.CritEva,
                new Stat(metadata.Stats.CritResist)
            },
            {
                StatId.Def,
                new Stat(metadata.Stats.Defense)
            }, // base affected by something?
            {
                StatId.Guard,
                new Stat(metadata.Stats.Guard)
            },
            {
                StatId.JumpHeight,
                new Stat(metadata.Stats.JumpHeight)
            },
            {
                StatId.PhysAtk,
                new Stat(metadata.Stats.PhysAtk)
            }, // base for mage, 74 thief
            {
                StatId.MagAtk,
                new Stat(metadata.Stats.MagAtk)
            }, // base for thief, 216 mage
            {
                StatId.PhysRes,
                new Stat(metadata.Stats.PhysRes)
            },
            {
                StatId.MagRes,
                new Stat(metadata.Stats.MagRes)
            },
            {
                StatId.MinAtk,
                new Stat(metadata.Stats.MinAtk)
            },
            {
                StatId.MaxAtk,
                new Stat(metadata.Stats.MaxAtk)
            },
            {
                StatId.MinDmg,
                new Stat(metadata.Stats.Damage)
            },
            {
                StatId.MaxDmg,
                new Stat(metadata.Stats.Damage)
            },
            {
                StatId.Pierce,
                new Stat(metadata.Stats.Pierce)
            },
            {
                StatId.MountSpeed,
                new Stat(metadata.Stats.MountSpeed)
            },
            {
                StatId.BonusAtk,
                new Stat(metadata.Stats.BonusAtk)
            },
            {
                StatId.PetBonusAtk,
                new Stat(metadata.Stats.BonusAtkPet)
            }
        };
    }

    public Stats(int strBase, int dexBase, int intBase, int lukBase, int hpBase, int critRateBase)
    {
        Data = new()
        {
            {
                StatId.Str,
                new Stat(strBase)
            },
            {
                StatId.Dex,
                new Stat(dexBase)
            },
            {
                StatId.Int,
                new Stat(intBase)
            },
            {
                StatId.Luk,
                new Stat(lukBase)
            },
            {
                StatId.Hp,
                new Stat(hpBase, hpBase, 0)
            }, // Max = 0 on login
            {
                StatId.HpRegen,
                new Stat(10)
            },
            {
                StatId.HpRegenTime,
                new Stat(3000)
            },
            {
                StatId.Spirit,
                new Stat(100, 100, 0)
            }, // Max = 0 on login
            {
                StatId.SpRegen,
                new Stat(1)
            },
            {
                StatId.SpRegenTime,
                new Stat(200)
            },
            {
                StatId.Stamina,
                new Stat(120)
            }, // Max = 0 on login
            {
                StatId.StaRegen,
                new Stat(10)
            },
            {
                StatId.StaRegenTime,
                new Stat(500)
            },
            {
                StatId.AtkSpd,
                new Stat(100)
            },
            {
                StatId.MoveSpd,
                new Stat(100)
            },
            {
                StatId.Acc,
                new Stat(82)
            },
            {
                StatId.Eva,
                new Stat(70)
            }, // changes with job
            {
                StatId.CritRate,
                new Stat(critRateBase)
            }, // changes with job
            {
                StatId.CritDmg,
                new Stat(250)
            },
            {
                StatId.CritEva,
                new Stat(50)
            },
            {
                StatId.Def,
                new Stat(16)
            }, // base affected by something?
            {
                StatId.Guard,
                new Stat(0)
            },
            {
                StatId.JumpHeight,
                new Stat(100)
            },
            {
                StatId.PhysAtk,
                new Stat(10)
            }, // base for mage, 74 thief
            {
                StatId.MagAtk,
                new Stat(2)
            }, // base for thief, 216 mage
            {
                StatId.PhysRes,
                new Stat(5)
            },
            {
                StatId.MagRes,
                new Stat(4)
            },
            {
                StatId.MinAtk,
                new Stat(0)
            },
            {
                StatId.MaxAtk,
                new Stat(0)
            },
            {
                StatId.MinDmg,
                new Stat(0)
            },
            {
                StatId.MaxDmg,
                new Stat(0)
            },
            {
                StatId.Pierce,
                new Stat(0)
            },
            {
                StatId.MountSpeed,
                new Stat(100)
            },
            {
                StatId.BonusAtk,
                new Stat(0)
            },
            {
                StatId.PetBonusAtk,
                new Stat(0)
            }
        };
    }

    public Stat this[StatId statIndex]
    {
        get => Data[statIndex];

        private set => Data[statIndex] = value;
    }

    public void InitializePools(int hp, int spirit, int stamina)
    {
        Stat hpStat = this[StatId.Hp];
        Stat spiritStat = this[StatId.Spirit];
        Stat staStat = this[StatId.Stamina];
        Data[StatId.Hp] = new(hpStat.Bonus, hpStat.Base, hp);
        Data[StatId.Spirit] = new(spiritStat.Bonus, spiritStat.Base, spirit);
        Data[StatId.Stamina] = new(staStat.Bonus, staStat.Base, stamina);
    }

    public void Allocate(StatId statId)
    {
        int gainAmount = 1;
        switch (statId)
        {
            case StatId.Hp:
                gainAmount = 10;
                break;
            case StatId.CritRate:
                gainAmount = 3;
                break;
            default:
                break;
        }
        Data[statId].IncreaseBonus(gainAmount);
    }

    public void ResetAllocations(StatDistribution statDist)
    {
        foreach (KeyValuePair<byte, int> entry in statDist.AllocatedStats)
        {
            StatId statId = (StatId) entry.Key;
            int gainAmount = 1;
            switch (statId)
            {
                case StatId.Hp:
                    gainAmount = 10;
                    break;
                case StatId.CritRate:
                    gainAmount = 3;
                    break;
                default:
                    break;
            }
            Data[statId].DecreaseBonus(entry.Value * gainAmount);
        }
        statDist.ResetPoints();
    }
}

/* Max = Base + stat allocations + bonuses.
 * Min = Base stat amount.
 * Current = Final value (e.g. capped Damage, current Hp, active CritRate, ...)
 *
 * Change PlayerStat.Current for temporary changes.
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

    public Stat(NpcStat<int> statInt)
    {
        Bonus = statInt.Bonus;
        Base = statInt.Base;
        Total = statInt.Total;
    }

    public Stat(NpcStat<long> statLong)
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
        _ => Total,
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
        Total -= amount;
    }

    public void Decrease(long amount)
    {
        TotalLong -= amount;
    }
}
