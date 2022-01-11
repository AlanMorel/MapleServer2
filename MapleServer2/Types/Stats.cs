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

    public Stats(int strBase, int dexBase, int intBase, int lukBase, int hpBase, int critRateBase)
    {
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
                StatId.HpRegenInterval, // base (3000ms)
                new(3000)
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
                StatId.SpRegenInterval, // base (200ms)
                new(200)
            },
            {
                StatId.Stamina,         // base 120 (20 = 1 block)
                new(120)
            }, // Max = 0 on login
            {
                StatId.StaminaRegen,    // base 10  (10 = 0.5 block)
                new(10)
            },
            {
                StatId.StaminaRegenInterval,    // base (500ms)
                new(500)
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
                new(critRateBase)
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
                StatId.PetBonusAtk, // base 0 (bonuses can be added)
                new(0)
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
        foreach ((byte id, int value) in statDist.AllocatedStats)
        {
            StatId statId = (StatId) id;
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
            Data[statId].DecreaseBonus(value * gainAmount);
        }
        statDist.ResetPoints();
    }
}
