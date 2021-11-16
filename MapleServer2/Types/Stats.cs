using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace MapleServer2.Types;

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
                StatId.HpRegenInterval,
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
                StatId.SpRegenInterval,
                new Stat(metadata.Stats.SpInterval)
            },
            {
                StatId.Stamina,
                new Stat(metadata.Stats.Ep)
            }, // Max = 0 on login
            {
                StatId.StaminaRegen,
                new Stat(metadata.Stats.EpRegen)
            },
            {
                StatId.StaminaRegenInterval,
                new Stat(metadata.Stats.EpInterval)
            },
            {
                StatId.AttackSpeed,
                new Stat(metadata.Stats.AtkSpd)
            },
            {
                StatId.MovementSpeed,
                new Stat(metadata.Stats.MoveSpd)
            },
            {
                StatId.Accuracy,
                new Stat(metadata.Stats.Accuracy)
            },
            {
                StatId.Evasion,
                new Stat(metadata.Stats.Evasion)
            }, // changes with job
            {
                StatId.CritRate,
                new Stat(metadata.Stats.CritRate)
            }, // changes with job
            {
                StatId.CritDamage,
                new Stat(metadata.Stats.CritDamage)
            },
            {
                StatId.CritEvasion,
                new Stat(metadata.Stats.CritResist)
            },
            {
                StatId.Defense,
                new Stat(metadata.Stats.Defense)
            }, // base affected by something?
            {
                StatId.PerfectGuard,
                new Stat(metadata.Stats.Guard)
            },
            {
                StatId.JumpHeight,
                new Stat(metadata.Stats.JumpHeight)
            },
            {
                StatId.PhysicalAtk,
                new Stat(metadata.Stats.PhysAtk)
            }, // base for mage, 74 thief
            {
                StatId.MagicAtk,
                new Stat(metadata.Stats.MagAtk)
            }, // base for thief, 216 mage
            {
                StatId.PhysicalRes,
                new Stat(metadata.Stats.PhysRes)
            },
            {
                StatId.MagicRes,
                new Stat(metadata.Stats.MagRes)
            },
            {
                StatId.MinWeaponAtk,
                new Stat(metadata.Stats.MinAtk)
            },
            {
                StatId.MaxWeaponAtk,
                new Stat(metadata.Stats.MaxAtk)
            },
            {
                StatId.MinDamage,
                new Stat(metadata.Stats.Damage)
            },
            {
                StatId.MaxDamage,
                new Stat(metadata.Stats.Damage)
            },
            {
                StatId.Pierce,
                new Stat(metadata.Stats.Pierce)
            },
            {
                StatId.MountMovementSpeed,
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
                new Stat(hpBase)
            }, // Max = 0 on login
            {
                StatId.HpRegen,
                new Stat(10)
            },
            {
                StatId.HpRegenInterval, // base (3000ms)
                new Stat(3000)
            },
            {
                StatId.Spirit,
                new Stat(100)
            }, // Max = 0 on login
            {
                StatId.SpRegen,
                new Stat(1)
            },
            {
                StatId.SpRegenInterval, // base (200ms)
                new Stat(200)
            },
            {
                StatId.Stamina,         // base 120 (20 = 1 block)
                new Stat(120)
            }, // Max = 0 on login
            {
                StatId.StaminaRegen,    // base 10  (10 = 0.5 block)
                new Stat(10)
            },
            {
                StatId.StaminaRegenInterval,    // base (500ms)
                new Stat(500)
            },
            {
                StatId.AttackSpeed,
                new Stat(100)
            },
            {
                StatId.MovementSpeed,
                new Stat(100)
            },
            {
                StatId.Accuracy,
                new Stat(82)
            },
            {
                StatId.Evasion,
                new Stat(70)
            }, // changes with job
            {
                StatId.CritRate,
                new Stat(critRateBase)
            }, // changes with job
            {
                StatId.CritDamage,
                new Stat(250)
            },
            {
                StatId.CritEvasion,
                new Stat(50)
            },
            {
                StatId.Defense,
                new Stat(16)
            }, // base affected by something?
            {
                StatId.PerfectGuard,
                new Stat(0)
            },
            {
                StatId.JumpHeight,
                new Stat(100)
            },
            {
                StatId.PhysicalAtk,
                new Stat(10)
            }, // base for mage, 74 thief
            {
                StatId.MagicAtk,
                new Stat(2)
            }, // base for thief, 216 mage
            {
                StatId.PhysicalRes,
                new Stat(5)
            },
            {
                StatId.MagicRes,
                new Stat(4)
            },
            {
                StatId.MinWeaponAtk,
                new Stat(0)
            },
            {
                StatId.MaxWeaponAtk,
                new Stat(0)
            },
            {
                StatId.MinDamage,
                new Stat(0)
            },
            {
                StatId.MaxDamage,
                new Stat(0)
            },
            {
                StatId.Pierce,
                new Stat(0)
            },
            {
                StatId.MountMovementSpeed,
                new Stat(100)
            },
            {
                StatId.BonusAtk,
                new Stat(0)
            },
            {
                StatId.PetBonusAtk, // base 0 (bonuses can be added)
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
