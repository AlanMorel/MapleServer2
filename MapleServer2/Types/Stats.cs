using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Enums;

namespace MapleServer2.Types;

public class Stats
{
    // TODO: Handle stat allocation in here?
    // ReSharper disable once FieldCanBeMadeReadOnly.Global - JsonConvert.DeserializeObject can't set values if it's readonly
    public Dictionary<StatAttribute, Stat> Data;
    public Dictionary<int, StatGroup> EffectGroups = new();
    public Dictionary<int, StatGroup> SkillGroups = new();

    public Stats() { }

    public Stats(NpcMetadata metadata)
    {
        Data = new()
        {
            {
                StatAttribute.Str,
                new(metadata.NpcStats.Str)
            },
            {
                StatAttribute.Dex,
                new(metadata.NpcStats.Dex)
            },
            {
                StatAttribute.Int,
                new(metadata.NpcStats.Int)
            },
            {
                StatAttribute.Luk,
                new(metadata.NpcStats.Luk)
            },
            {
                StatAttribute.Hp,
                new(metadata.NpcStats.Hp)
            },
            {
                StatAttribute.HpRegen,
                new(metadata.NpcStats.HpRegen)
            },
            {
                StatAttribute.HpRegenInterval,
                new(metadata.NpcStats.HpInterval)
            },
            {
                StatAttribute.Spirit,
                new(metadata.NpcStats.Sp)
            },
            {
                StatAttribute.SpRegen,
                new(metadata.NpcStats.SpRegen)
            },
            {
                StatAttribute.SpRegenInterval,
                new(metadata.NpcStats.SpInterval)
            },
            {
                StatAttribute.Stamina,
                new(metadata.NpcStats.Ep)
            },
            {
                StatAttribute.StaminaRegen,
                new(metadata.NpcStats.EpRegen)
            },
            {
                StatAttribute.StaminaRegenInterval,
                new(metadata.NpcStats.EpInterval)
            },
            {
                StatAttribute.AttackSpeed,
                new(metadata.NpcStats.AtkSpd)
            },
            {
                StatAttribute.MovementSpeed,
                new(metadata.NpcStats.MoveSpd)
            },
            {
                StatAttribute.Accuracy,
                new(metadata.NpcStats.Accuracy)
            },
            {
                StatAttribute.Evasion,
                new(metadata.NpcStats.Evasion)
            },
            {
                StatAttribute.CritRate,
                new(metadata.NpcStats.CritRate)
            },
            {
                StatAttribute.CritDamage,
                new(metadata.NpcStats.CritDamage)
            },
            {
                StatAttribute.CritEvasion,
                new(metadata.NpcStats.CritResist)
            },
            {
                StatAttribute.Defense,
                new(metadata.NpcStats.Defense)
            },
            {
                StatAttribute.PerfectGuard,
                new(metadata.NpcStats.Guard)
            },
            {
                StatAttribute.JumpHeight,
                new(metadata.NpcStats.JumpHeight)
            },
            {
                StatAttribute.PhysicalAtk,
                new(metadata.NpcStats.PhysAtk)
            },
            {
                StatAttribute.MagicAtk,
                new(metadata.NpcStats.MagAtk)
            },
            {
                StatAttribute.PhysicalRes,
                new(metadata.NpcStats.PhysRes)
            },
            {
                StatAttribute.MagicRes,
                new(metadata.NpcStats.MagRes)
            },
            {
                StatAttribute.MinWeaponAtk,
                new(metadata.NpcStats.MinAtk)
            },
            {
                StatAttribute.MaxWeaponAtk,
                new(metadata.NpcStats.MaxAtk)
            },
            {
                StatAttribute.MinDamage,
                new(metadata.NpcStats.Damage)
            },
            {
                StatAttribute.MaxDamage,
                new(metadata.NpcStats.Damage)
            },
            {
                StatAttribute.Pierce,
                new(metadata.NpcStats.Pierce, StatAttributeType.Rate)
            },
            {
                StatAttribute.MountMovementSpeed,
                new(metadata.NpcStats.MountSpeed)
            },
            {
                StatAttribute.BonusAtk,
                new(metadata.NpcStats.BonusAtk)
            },
            {
                StatAttribute.PetBonusAtk,
                new(metadata.NpcStats.BonusAtkPet)
            }
        };
    }

    public Stats(JobCode jobCode)
    {
        Data = new()
        {
            {
                StatAttribute.Str,
                new(BaseStats.Strength(jobCode, 1))
            },
            {
                StatAttribute.Dex,
                new(BaseStats.Dexterity(jobCode, 1))
            },
            {
                StatAttribute.Int,
                new(BaseStats.Intelligence(jobCode, 1))
            },
            {
                StatAttribute.Luk,
                new(BaseStats.Luck(jobCode, 1))
            },
            {
                StatAttribute.Hp,
                new(BaseStats.Health(jobCode, 1)) // Max = 0 on login
            },
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
                new(100) // Max = 0 on login
            },
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
                new(120) // base 120 (20 = 1 block) / Max = 0 on login
            },
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
                new(BaseStats.Accuracy(jobCode, 1))
            },
            {
                StatAttribute.Evasion,
                new(BaseStats.Evasion(jobCode, 1)) // TODO: changes with job
            },
            {
                StatAttribute.CritRate,
                new(BaseStats.CriticalRate(jobCode, 1)) // TODO: changes with job
            },
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
                new(16) // // base affected by something?
            },
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
                new(10) // TODO: changes with job (base for mage, 74 thief)
            },
            {
                StatAttribute.MagicAtk,
                new(2) // TODO: changes with job (base for thief, 216 mage)
            },
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
                new(0, StatAttributeType.Rate)
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
    }

    public void Allocate(StatAttribute statAttribute)
    {
        int gainAmount = statAttribute switch
        {
            StatAttribute.Hp => 10,
            StatAttribute.CritRate => 3,
            _ => 1
        };

        Data[statAttribute].Add(gainAmount);
    }

    public void RecomputeAllocations(StatDistribution statDist)
    {
        foreach ((StatAttribute id, int value) in statDist.AllocatedStats)
        {
            for (int i = 0; i < value; i++)
            {
                Allocate(id);
            }
        }
    }

    public void ResetStats()
    {
        foreach ((StatAttribute id, Stat stat) in Data)
        {
            if (stat != null)
            {
                stat.Reset();
            }
        }

        EffectGroups.Clear();
        SkillGroups.Clear();
    }

    public void ComputeStatBonuses()
    {
        foreach ((StatAttribute id, Stat stat) in Data)
        {
            if (stat != null)
            {
                stat.ComputeBonus();
            }
        }
    }

    // TODO: level 50+ has a different formula
    public void AddBaseStats(Player player, short level = 1)
    {
        long str = BaseStats.Strength(player.JobCode, level);
        long dex = BaseStats.Dexterity(player.JobCode, level);
        long luk = BaseStats.Luck(player.JobCode, level);
        long inte = BaseStats.Intelligence(player.JobCode, level);
        long hp = BaseStats.Health(player.JobCode, level);

        Data[StatAttribute.Str].Value.BaseLong = str;
        Data[StatAttribute.Dex].Value.BaseLong = dex;
        Data[StatAttribute.Luk].Value.BaseLong = luk;
        Data[StatAttribute.Int].Value.BaseLong = inte;
        Data[StatAttribute.Hp].Value.BaseLong = hp;

        Data[StatAttribute.PhysicalAtk].Value.BaseLong = BaseStats.PhysicalAttack(player.JobCode, str, dex, luk);
        Data[StatAttribute.MagicAtk].Value.BaseLong = BaseStats.MagicAttack(player.JobCode, inte);
    }

    public void AddAttackBonus(Player player)
    {
        long str = Data[StatAttribute.Str].Value.BonusAmountLong;
        long dex = Data[StatAttribute.Dex].Value.BonusAmountLong;
        long luk = Data[StatAttribute.Luk].Value.BonusAmountLong;
        long inte = Data[StatAttribute.Int].Value.BonusAmountLong;
        long hp = Data[StatAttribute.Hp].Value.BonusAmountLong;

        Data[StatAttribute.PhysicalAtk].AddBonus(BaseStats.PhysicalAttack(player.JobCode, str, dex, luk));
        Data[StatAttribute.MagicAtk].AddBonus(BaseStats.MagicAttack(player.JobCode, inte));
    }

    public void AddStat(StatAttribute attribute, StatAttributeType type, long flat, float rate)
    {
        if (!Data.TryGetValue(attribute, out Stat stat))
        {
            stat = new();
            stat.Modifier.Type = type;
            stat.Modifier.Rate = type == StatAttributeType.Flat ? 1 : 0;

            Data[attribute] = stat;
        }

        stat.Add(flat, rate);
    }

    public void AddEffectGroup(int id, float value, float rate)
    {
        if (!EffectGroups.TryGetValue(id, out StatGroup stat))
        {
            stat = new StatGroup();
        }

        stat.Value += value;
        stat.Rate += rate;

        EffectGroups[id] = stat;
    }

    public void AddSkillGroup(int id, float value, float rate)
    {
        if (!SkillGroups.TryGetValue(id, out StatGroup stat))
        {
            stat = new StatGroup();
        }

        stat.Value += value;
        stat.Rate += rate;

        SkillGroups[id] = stat;
    }

    public StatGroup GetEffectStats(int[] ids)
    {
        StatGroup stat = new()
        {
            Rate = 1
        };

        if (ids == null)
        {
            return stat;
        }

        foreach (int id in ids)
        {
            if (EffectGroups.TryGetValue(id, out StatGroup statGroup))
            {
                stat.Value += statGroup.Value;
                stat.Rate += statGroup.Rate;
            }
        }

        return stat;
    }

    public StatGroup GetSkillStats(int[] ids)
    {
        StatGroup stat = new()
        {
            Rate = 1
        };

        if (ids == null)
        {
            return stat;
        }

        foreach (int id in ids)
        {
            if (SkillGroups.TryGetValue(id, out StatGroup statGroup))
            {
                stat.Value += statGroup.Value;
                stat.Rate += statGroup.Rate;
            }
        }

        return stat;
    }
}
