using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Enums;

namespace MapleServer2.Types;

public class Stats
{
    // TODO: Handle stat allocation in here?
    public Dictionary<StatAttribute, Stat> Data;

    public Stats() { }

    public Stats(NpcMetadata metadata)
    {
        Data = new()
        {
            {
                StatAttribute.Str,
                new(metadata.Stats.Str)
            },
            {
                StatAttribute.Dex,
                new(metadata.Stats.Dex)
            },
            {
                StatAttribute.Int,
                new(metadata.Stats.Int)
            },
            {
                StatAttribute.Luk,
                new(metadata.Stats.Luk)
            },
            {
                StatAttribute.Hp,
                new(metadata.Stats.Hp)
            }, // Max = 0 on login
            {
                StatAttribute.HpRegen,
                new(metadata.Stats.HpRegen)
            },
            {
                StatAttribute.HpRegenInterval,
                new(metadata.Stats.HpInterval)
            },
            {
                StatAttribute.Spirit,
                new(metadata.Stats.Sp)
            }, // Max = 0 on login
            {
                StatAttribute.SpRegen,
                new(metadata.Stats.SpRegen)
            },
            {
                StatAttribute.SpRegenInterval,
                new(metadata.Stats.SpInterval)
            },
            {
                StatAttribute.Stamina,
                new(metadata.Stats.Ep)
            }, // Max = 0 on login
            {
                StatAttribute.StaminaRegen,
                new(metadata.Stats.EpRegen)
            },
            {
                StatAttribute.StaminaRegenInterval,
                new(metadata.Stats.EpInterval)
            },
            {
                StatAttribute.AttackSpeed,
                new(metadata.Stats.AtkSpd)
            },
            {
                StatAttribute.MovementSpeed,
                new(metadata.Stats.MoveSpd)
            },
            {
                StatAttribute.Accuracy,
                new(metadata.Stats.Accuracy)
            },
            {
                StatAttribute.Evasion,
                new(metadata.Stats.Evasion)
            }, // changes with job
            {
                StatAttribute.CritRate,
                new(metadata.Stats.CritRate)
            }, // changes with job
            {
                StatAttribute.CritDamage,
                new(metadata.Stats.CritDamage)
            },
            {
                StatAttribute.CritEvasion,
                new(metadata.Stats.CritResist)
            },
            {
                StatAttribute.Defense,
                new(metadata.Stats.Defense)
            }, // base affected by something?
            {
                StatAttribute.PerfectGuard,
                new(metadata.Stats.Guard)
            },
            {
                StatAttribute.JumpHeight,
                new(metadata.Stats.JumpHeight)
            },
            {
                StatAttribute.PhysicalAtk,
                new(metadata.Stats.PhysAtk)
            }, // base for mage, 74 thief
            {
                StatAttribute.MagicAtk,
                new(metadata.Stats.MagAtk)
            }, // base for thief, 216 mage
            {
                StatAttribute.PhysicalRes,
                new(metadata.Stats.PhysRes)
            },
            {
                StatAttribute.MagicRes,
                new(metadata.Stats.MagRes)
            },
            {
                StatAttribute.MinWeaponAtk,
                new(metadata.Stats.MinAtk)
            },
            {
                StatAttribute.MaxWeaponAtk,
                new(metadata.Stats.MaxAtk)
            },
            {
                StatAttribute.MinDamage,
                new(metadata.Stats.Damage)
            },
            {
                StatAttribute.MaxDamage,
                new(metadata.Stats.Damage)
            },
            {
                StatAttribute.Pierce,
                new(metadata.Stats.Pierce)
            },
            {
                StatAttribute.MountMovementSpeed,
                new(metadata.Stats.MountSpeed)
            },
            {
                StatAttribute.BonusAtk,
                new(metadata.Stats.BonusAtk)
            },
            {
                StatAttribute.PetBonusAtk,
                new(metadata.Stats.BonusAtkPet)
            }
        };
    }

    public Stats(Job job)
    {
        (int strBase, int dexBase, int intBase, int lukBase, int hpBase, int critBase) = GetJobBaseStats(job);
        Data = new()
        {
            {
                StatAttribute.Str,
                new(strBase)
            },
            {
                StatAttribute.Dex,
                new(dexBase)
            },
            {
                StatAttribute.Int,
                new(intBase)
            },
            {
                StatAttribute.Luk,
                new(lukBase)
            },
            {
                StatAttribute.Hp,
                new(hpBase)
            }, // Max = 0 on login
            {
                StatAttribute.HpRegen,
                new(10)
            },
            {
                StatAttribute.HpRegenInterval,
                new(3000) // base (3000ms)
            },
            {
                StatAttribute.Spirit,
                new(100)
            }, // Max = 0 on login
            {
                StatAttribute.SpRegen,
                new(1)
            },
            {
                StatAttribute.SpRegenInterval,
                new(200) // base (200ms)
            },
            {
                StatAttribute.Stamina,
                new(120) // base 120 (20 = 1 block)
            }, // Max = 0 on login
            {
                StatAttribute.StaminaRegen,
                new(10) // base 10  (10 = 0.5 block)
            },
            {
                StatAttribute.StaminaRegenInterval,
                new(500) // base (500ms)
            },
            {
                StatAttribute.AttackSpeed,
                new(100)
            },
            {
                StatAttribute.MovementSpeed,
                new(100)
            },
            {
                StatAttribute.Accuracy,
                new(82)
            },
            {
                StatAttribute.Evasion,
                new(70)
            }, // changes with job
            {
                StatAttribute.CritRate,
                new(critBase)
            }, // changes with job
            {
                StatAttribute.CritDamage,
                new(250)
            },
            {
                StatAttribute.CritEvasion,
                new(50)
            },
            {
                StatAttribute.Defense,
                new(16)
            }, // base affected by something?
            {
                StatAttribute.PerfectGuard,
                new(0)
            },
            {
                StatAttribute.JumpHeight,
                new(100)
            },
            {
                StatAttribute.PhysicalAtk,
                new(10)
            }, // base for mage, 74 thief
            {
                StatAttribute.MagicAtk,
                new(2)
            }, // base for thief, 216 mage
            {
                StatAttribute.PhysicalRes,
                new(5)
            },
            {
                StatAttribute.MagicRes,
                new(4)
            },
            {
                StatAttribute.MinWeaponAtk,
                new(0)
            },
            {
                StatAttribute.MaxWeaponAtk,
                new(0)
            },
            {
                StatAttribute.MinDamage,
                new(0)
            },
            {
                StatAttribute.MaxDamage,
                new(0)
            },
            {
                StatAttribute.Pierce,
                new(0)
            },
            {
                StatAttribute.MountMovementSpeed,
                new(100)
            },
            {
                StatAttribute.BonusAtk,
                new(0)
            },
            {
                StatAttribute.PetBonusAtk,
                new(0) // base 0 (bonuses can be added)
            }
        };

        if (job is Job.Runeblade)
        {
            Data[StatAttribute.Int].IncreaseBonus(5);
        }
    }


    public Stat this[StatAttribute statIndex]
    {
        get
        {
            if (!Data.ContainsKey(statIndex))
            {
                Data[statIndex] = new(0);
            }
            return Data[statIndex];
        }

        private set => Data[statIndex] = value;
    }

    public void InitializePools(int hp, int spirit, int stamina)
    {
        Stat hpStat = this[StatAttribute.Hp];
        Stat spiritStat = this[StatAttribute.Spirit];
        Stat staStat = this[StatAttribute.Stamina];
        Data[StatAttribute.Hp] = new(hpStat.Bonus, hpStat.Base, hp);
        Data[StatAttribute.Spirit] = new(spiritStat.Bonus, spiritStat.Base, spirit);
        Data[StatAttribute.Stamina] = new(staStat.Bonus, staStat.Base, stamina);
    }

    public void Allocate(StatAttribute statAttribute)
    {
        int gainAmount = statAttribute switch
        {
            StatAttribute.Hp => 10,
            StatAttribute.CritRate => 3,
            _ => 1
        };

        Data[statAttribute].IncreaseBonus(gainAmount);
    }

    public void ResetAllocations(StatDistribution statDist)
    {
        foreach ((StatAttribute id, int value) in statDist.AllocatedStats)
        {
            int gainAmount = id switch
            {
                StatAttribute.Hp => 10,
                StatAttribute.CritRate => 3,
                _ => 1
            };

            Data[id].DecreaseBonus(value * gainAmount);
        }

        statDist.ResetPoints();
    }

    private static (int strBase, int dexBase, int intBase, int lukBase, int hpBase, int critBase) GetJobBaseStats(Job job)
    {
        int strBase = 0, dexBase = 0, intBase = 0, lukBase = 0, hpBase = 0, critBase = 0;
        switch (job)
        {
            case Job.Beginner:
                strBase = 7;
                dexBase = 6;
                intBase = 2;
                lukBase = 2;
                hpBase = 63;
                critBase = 35;
                break;
            case Job.Knight:
                strBase = 8;
                dexBase = 6;
                intBase = 2;
                lukBase = 1;
                hpBase = 65;
                critBase = 51;
                break;
            case Job.Berserker:
                strBase = 8;
                dexBase = 6;
                intBase = 1;
                lukBase = 2;
                hpBase = 70;
                critBase = 47;
                break;
            case Job.Wizard:
                strBase = 1;
                dexBase = 1;
                intBase = 14;
                lukBase = 1;
                hpBase = 61;
                critBase = 40;
                break;
            case Job.Priest:
                strBase = 1;
                dexBase = 1;
                intBase = 14;
                lukBase = 1;
                hpBase = 63;
                critBase = 45;
                break;
            case Job.Archer:
                strBase = 6;
                dexBase = 8;
                intBase = 1;
                lukBase = 2;
                hpBase = 61;
                critBase = 55;
                break;
            case Job.HeavyGunner:
                strBase = 2;
                dexBase = 8;
                intBase = 1;
                lukBase = 6;
                hpBase = 63;
                critBase = 52;
                break;
            case Job.Thief:
                strBase = 6;
                dexBase = 2;
                intBase = 1;
                lukBase = 8;
                hpBase = 62;
                critBase = 50;
                break;
            case Job.Assassin:
                strBase = 2;
                dexBase = 6;
                intBase = 1;
                lukBase = 8;
                hpBase = 61;
                critBase = 53;
                break;
            case Job.Runeblade:
                strBase = 8;
                dexBase = 6;
                intBase = 2;
                lukBase = 1;
                hpBase = 64;
                critBase = 46;
                break;
            case Job.Striker:
                strBase = 6;
                dexBase = 8;
                intBase = 1;
                lukBase = 2;
                hpBase = 64;
                critBase = 48;
                break;
            case Job.SoulBinder:
                strBase = 1;
                dexBase = 1;
                intBase = 14;
                lukBase = 1;
                hpBase = 62;
                critBase = 48;
                break;
            default:
            case Job.GameMaster:
            case Job.None:
                break;
        }

        return (strBase, dexBase, intBase, lukBase, hpBase, critBase);
    }

    private static (int strIncrease, int dexIncrease, int intIncrease, int lukIncrease, int hpIncrease) GetJobStatIncrease(Player player)
    {
        Job job = player.Job;
        int level = player.Levels.Level;
        Dictionary<StatAttribute, Stat> stats = player.Stats.Data;

        // TODO: Find HP formula
        int strIncrease = 0, dexIncrease = 0, intIncrease = 0, lukIncrease = 0, hpIncrease = 0;
        switch (job)
        {
            case Job.Beginner:
                strIncrease = 7;
                dexIncrease = 1;
                if (level % 2 == 0)
                {
                    lukIncrease = 1;
                }
                else
                {
                    intIncrease = 1;
                }

                hpIncrease = 0;
                break;
            case Job.Knight:
                strIncrease = 7;
                dexIncrease = 1;
                if (level % 2 == 0)
                {
                    intIncrease = 1;
                }
                else
                {
                    lukIncrease = 1;
                }

                hpIncrease = 0;
                break;
            case Job.Berserker:
                strIncrease = 7;
                dexIncrease = 1;
                if (level % 2 == 0)
                {
                    intIncrease = 1;
                }
                else
                {
                    lukIncrease = 1;
                }

                hpIncrease = 0;
                break;
            case Job.Wizard:
                intIncrease = 8;
                if (stats[StatAttribute.Str].Base > stats[StatAttribute.Dex].Base)
                {
                    dexIncrease = 1;
                }
                else if (stats[StatAttribute.Str].Base > stats[StatAttribute.Luk].Base)
                {
                    lukIncrease = 1;
                }
                else
                {
                    strIncrease = 1;
                }

                hpIncrease = 0;
                break;
            case Job.Priest:
                intIncrease = 8;
                if (stats[StatAttribute.Str].Base > stats[StatAttribute.Dex].Base)
                {
                    dexIncrease = 1;
                }
                else if (stats[StatAttribute.Str].Base > stats[StatAttribute.Luk].Base)
                {
                    lukIncrease = 1;
                }
                else
                {
                    strIncrease = 1;
                }

                hpIncrease = 0;
                break;
            case Job.Archer:
                dexIncrease = 7;
                strIncrease = 1;
                if (level % 2 == 0)
                {
                    lukIncrease = 1;
                }
                else
                {
                    intIncrease = 1;
                }

                hpIncrease = 0;
                break;
            case Job.HeavyGunner:
                dexIncrease = 7;
                lukIncrease = 1;
                if (level % 2 == 0)
                {
                    strIncrease = 1;
                }
                else
                {
                    intIncrease = 1;
                }

                hpIncrease = 0;
                break;
            case Job.Thief:
                lukIncrease = 7;
                strIncrease = 1;
                if (level % 2 == 0)
                {
                    dexIncrease = 1;
                }
                else
                {
                    intIncrease = 1;
                }

                hpIncrease = 0;
                break;
            case Job.Assassin:
                lukIncrease = 7;
                dexIncrease = 1;
                if (level % 2 == 0)
                {
                    strIncrease = 1;
                }
                else
                {
                    intIncrease = 1;
                }

                hpIncrease = 0;
                break;
            case Job.Runeblade:
                strIncrease = 7;
                dexIncrease = 1;
                if (level % 2 == 0)
                {
                    lukIncrease = 1;
                }
                else
                {
                    intIncrease = 1;
                }

                hpIncrease = 0;
                break;
            case Job.Striker:
                dexIncrease = 7;
                strIncrease = 1;
                if (level % 2 == 0)
                {
                    intIncrease = 1;
                }
                else
                {
                    lukIncrease = 1;
                }

                hpIncrease = 0;
                break;
            case Job.SoulBinder:
                intIncrease = 8;
                if (stats[StatAttribute.Str].Base > stats[StatAttribute.Dex].Base)
                {
                    dexIncrease = 1;
                }
                else if (stats[StatAttribute.Str].Base > stats[StatAttribute.Luk].Base)
                {
                    lukIncrease = 1;
                }
                else
                {
                    strIncrease = 1;
                }

                hpIncrease = 0;
                break;
            default:
            case Job.GameMaster:
            case Job.None:
                break;
        }

        return (strIncrease, dexIncrease, intIncrease, lukIncrease, hpIncrease);
    }

    public void AddBaseStats(Player player, int repeat = 1)
    {
        (int strIncrease, int dexIncrease, int intIncrease, int lukIncrease, int hpIncrease) = GetJobStatIncrease(player);
        Data[StatAttribute.Str].IncreaseBase(strIncrease * repeat);
        Data[StatAttribute.Dex].IncreaseBase(dexIncrease * repeat);
        Data[StatAttribute.Int].IncreaseBase(intIncrease * repeat);
        Data[StatAttribute.Luk].IncreaseBase(lukIncrease * repeat);
        Data[StatAttribute.Hp].IncreaseBase(hpIncrease * repeat);
    }
}
