using Maple2Storage.Enums;
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
            }
            Data[statId].DecreaseBonus(entry.Value * gainAmount);
        }
        statDist.ResetPoints();
    }
}
