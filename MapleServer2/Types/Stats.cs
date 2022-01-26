using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Enums;

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
                new(metadata.Stats.Str)
            },
            {
                StatId.Dex,
                new(metadata.Stats.Dex)
            },
            {
                StatId.Int,
                new(metadata.Stats.Int)
            },
            {
                StatId.Luk,
                new(metadata.Stats.Luk)
            },
            {
                StatId.Hp,
                new(metadata.Stats.Hp)
            }, // Max = 0 on login
            {
                StatId.HpRegen,
                new(metadata.Stats.HpRegen)
            },
            {
                StatId.HpRegenInterval,
                new(metadata.Stats.HpInterval)
            },
            {
                StatId.Spirit,
                new(metadata.Stats.Sp)
            }, // Max = 0 on login
            {
                StatId.SpRegen,
                new(metadata.Stats.SpRegen)
            },
            {
                StatId.SpRegenInterval,
                new(metadata.Stats.SpInterval)
            },
            {
                StatId.Stamina,
                new(metadata.Stats.Ep)
            }, // Max = 0 on login
            {
                StatId.StaminaRegen,
                new(metadata.Stats.EpRegen)
            },
            {
                StatId.StaminaRegenInterval,
                new(metadata.Stats.EpInterval)
            },
            {
                StatId.AttackSpeed,
                new(metadata.Stats.AtkSpd)
            },
            {
                StatId.MovementSpeed,
                new(metadata.Stats.MoveSpd)
            },
            {
                StatId.Accuracy,
                new(metadata.Stats.Accuracy)
            },
            {
                StatId.Evasion,
                new(metadata.Stats.Evasion)
            }, // changes with job
            {
                StatId.CritRate,
                new(metadata.Stats.CritRate)
            }, // changes with job
            {
                StatId.CritDamage,
                new(metadata.Stats.CritDamage)
            },
            {
                StatId.CritEvasion,
                new(metadata.Stats.CritResist)
            },
            {
                StatId.Defense,
                new(metadata.Stats.Defense)
            }, // base affected by something?
            {
                StatId.PerfectGuard,
                new(metadata.Stats.Guard)
            },
            {
                StatId.JumpHeight,
                new(metadata.Stats.JumpHeight)
            },
            {
                StatId.PhysicalAtk,
                new(metadata.Stats.PhysAtk)
            }, // base for mage, 74 thief
            {
                StatId.MagicAtk,
                new(metadata.Stats.MagAtk)
            }, // base for thief, 216 mage
            {
                StatId.PhysicalRes,
                new(metadata.Stats.PhysRes)
            },
            {
                StatId.MagicRes,
                new(metadata.Stats.MagRes)
            },
            {
                StatId.MinWeaponAtk,
                new(metadata.Stats.MinAtk)
            },
            {
                StatId.MaxWeaponAtk,
                new(metadata.Stats.MaxAtk)
            },
            {
                StatId.MinDamage,
                new(metadata.Stats.Damage)
            },
            {
                StatId.MaxDamage,
                new(metadata.Stats.Damage)
            },
            {
                StatId.Pierce,
                new(metadata.Stats.Pierce)
            },
            {
                StatId.MountMovementSpeed,
                new(metadata.Stats.MountSpeed)
            },
            {
                StatId.BonusAtk,
                new(metadata.Stats.BonusAtk)
            },
            {
                StatId.PetBonusAtk,
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
                StatId.Str,
                new(strBase)
            },
            {
                StatId.Dex,
                new(dexBase)
            },
            {
                StatId.Int,
                new(intBase)
            },
            {
                StatId.Luk,
                new(lukBase)
            },
            {
                StatId.Hp,
                new(hpBase)
            }, // Max = 0 on login
            {
                StatId.HpRegen,
                new(10)
            },
            {
                StatId.HpRegenInterval,
                new(3000) // base (3000ms)
            },
            {
                StatId.Spirit,
                new(100)
            }, // Max = 0 on login
            {
                StatId.SpRegen,
                new(1)
            },
            {
                StatId.SpRegenInterval,
                new(200) // base (200ms)
            },
            {
                StatId.Stamina,
                new(120) // base 120 (20 = 1 block)
            }, // Max = 0 on login
            {
                StatId.StaminaRegen,
                new(10) // base 10  (10 = 0.5 block)
            },
            {
                StatId.StaminaRegenInterval,
                new(500) // base (500ms)
            },
            {
                StatId.AttackSpeed,
                new(100)
            },
            {
                StatId.MovementSpeed,
                new(100)
            },
            {
                StatId.Accuracy,
                new(82)
            },
            {
                StatId.Evasion,
                new(70)
            }, // changes with job
            {
                StatId.CritRate,
                new(critBase)
            }, // changes with job
            {
                StatId.CritDamage,
                new(250)
            },
            {
                StatId.CritEvasion,
                new(50)
            },
            {
                StatId.Defense,
                new(16)
            }, // base affected by something?
            {
                StatId.PerfectGuard,
                new(0)
            },
            {
                StatId.JumpHeight,
                new(100)
            },
            {
                StatId.PhysicalAtk,
                new(10)
            }, // base for mage, 74 thief
            {
                StatId.MagicAtk,
                new(2)
            }, // base for thief, 216 mage
            {
                StatId.PhysicalRes,
                new(5)
            },
            {
                StatId.MagicRes,
                new(4)
            },
            {
                StatId.MinWeaponAtk,
                new(0)
            },
            {
                StatId.MaxWeaponAtk,
                new(0)
            },
            {
                StatId.MinDamage,
                new(0)
            },
            {
                StatId.MaxDamage,
                new(0)
            },
            {
                StatId.Pierce,
                new(0)
            },
            {
                StatId.MountMovementSpeed,
                new(100)
            },
            {
                StatId.BonusAtk,
                new(0)
            },
            {
                StatId.PetBonusAtk,
                new(0) // base 0 (bonuses can be added)
            }
        };

        if (job is Job.Runeblade)
        {
            Data[StatId.Int].IncreaseBonus(5);
        }
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
        int gainAmount = statId switch
        {
            StatId.Hp => 10,
            StatId.CritRate => 3,
            _ => 1
        };

        Data[statId].IncreaseBonus(gainAmount);
    }

    public void ResetAllocations(StatDistribution statDist)
    {
        foreach ((StatId id, int value) in statDist.AllocatedStats)
        {
            int gainAmount = id switch
            {
                StatId.Hp => 10,
                StatId.CritRate => 3,
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
        Dictionary<StatId, Stat> stats = player.Stats.Data;

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
                if (stats[StatId.Str].Base > stats[StatId.Dex].Base)
                {
                    dexIncrease = 1;
                }
                else if (stats[StatId.Str].Base > stats[StatId.Luk].Base)
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
                if (stats[StatId.Str].Base > stats[StatId.Dex].Base)
                {
                    dexIncrease = 1;
                }
                else if (stats[StatId.Str].Base > stats[StatId.Luk].Base)
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
                if (stats[StatId.Str].Base > stats[StatId.Dex].Base)
                {
                    dexIncrease = 1;
                }
                else if (stats[StatId.Str].Base > stats[StatId.Luk].Base)
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

    public void AddBaseStats(Player player)
    {
        (int strIncrease, int dexIncrease, int intIncrease, int lukIncrease, int hpIncrease) = GetJobStatIncrease(player);
        Data[StatId.Str].IncreaseBase(strIncrease);
        Data[StatId.Dex].IncreaseBase(dexIncrease);
        Data[StatId.Int].IncreaseBase(intIncrease);
        Data[StatId.Luk].IncreaseBase(lukIncrease);
        Data[StatId.Hp].IncreaseBase(hpIncrease);

        if (player.Job is Job.Runeblade)
        {
            Data[StatId.Int].IncreaseBonus(5);
        }
    }
}
