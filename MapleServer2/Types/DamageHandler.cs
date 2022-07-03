using Maple2Storage.Enums;
using MapleServer2.Enums;

namespace MapleServer2.Types;

public class DamageHandler
{
    public IFieldActor Source { get; }
    public IFieldActor Target { get; }
    public double Damage { get; }
    public HitType HitType { get; }

    private DamageHandler(IFieldActor source, IFieldActor target, double damage, HitType hitType)
    {
        Source = source;
        Target = target;
        Damage = damage;
        HitType = hitType;
    }

    public static DamageHandler CalculateDamage(SkillCast skill, IFieldActor<Player> source, IFieldActor target)
    {
        if (source.Value.GmFlags.Contains("oneshot"))
        {
            return new(source, target, target.Stats[StatAttribute.Hp].Total, HitType.Critical);
        }

        // get luck coefficient from class. new stat recommended, can be refactored away like isCrit was
        return CalculateDamage(skill, source, target, 1);
    }

    public static double FetchMultiplier(Stats stats, StatAttribute attribute)
    {
        return (double) stats[attribute].Total / 1000;
    }

    public static DamageHandler CalculateDamage(SkillCast skill, IFieldActor source, IFieldActor target, double luckCoefficient)
    {
        // TODO: get accuracyWeakness from enemy stats from enemy buff. new stat recommended
        const double AccuracyWeakness = 0;
        double hitRate = (source.Stats[StatAttribute.Accuracy].Total + AccuracyWeakness) / Math.Max(target.Stats[StatAttribute.Evasion].Total, 0.1);

        if (Random.Shared.NextDouble() > hitRate)
        {
            return new(source, target, 0, HitType.Miss); // we missed
        }

        bool isCrit = skill.IsGuaranteedCrit() || RollCrit(source, target, luckCoefficient);

        double finalCritDamage = 1;

        if (isCrit)
        {
            // TODO: get critResist from enemy stats from enemy buff. new stat recommended
            const double CritResist = 1;
            double critDamage = 1000 + source.Stats[StatAttribute.CritDamage].Total + source.Stats[StatAttribute.CriticalDamage].Total;
            finalCritDamage = CritResist * ((critDamage / 1000) - 1) + 1;
        }

        double damageBonus = 1 + FetchMultiplier(source.Stats, StatAttribute.TotalDamage) + FetchMultiplier(source.Stats, StatAttribute.Damage);

        damageBonus *= finalCritDamage;

        switch (skill.GetElement())
        {
            case Element.Fire:
                damageBonus += FetchMultiplier(source.Stats, StatAttribute.FireDamage);
                break;
            case Element.Ice:
                damageBonus += FetchMultiplier(source.Stats, StatAttribute.IceDamage);
                break;
            case Element.Electric:
                damageBonus += FetchMultiplier(source.Stats, StatAttribute.ElectricDamage);
                break;
            case Element.Holy:
                damageBonus += FetchMultiplier(source.Stats, StatAttribute.HolyDamage);
                break;
            case Element.Dark:
                damageBonus += FetchMultiplier(source.Stats, StatAttribute.DarkDamage);
                break;
            case Element.Poison:
                damageBonus += FetchMultiplier(source.Stats, StatAttribute.PoisonDamage);
                break;
        }

        SkillRangeType rangeType = skill.GetRangeType();

        if (rangeType != SkillRangeType.Special)
        {
            damageBonus += FetchMultiplier(source.Stats, rangeType == SkillRangeType.Melee ? StatAttribute.MeleeDamage : StatAttribute.RangedDamage);
        }

        bool isBoss = false;

        if (target is INpc npc)
        {
            isBoss = npc.Value.IsBoss();
        }

        damageBonus += isBoss ? FetchMultiplier(source.Stats, StatAttribute.BossDamage) : 0;

        // TODO: properly fetch enemy attack speed weakness from enemy buff. new stat recommended
        const double AttackSpeedWeakness = 0;

        damageBonus += AttackSpeedWeakness * FetchMultiplier(source.Stats, StatAttribute.AttackSpeed);

        double damageMultiplier = damageBonus * skill.GetDamageRate();

        // TODO: properly fetch enemy pierce resistance from enemy buff. new stat recommended
        const double EnemyPierceResistance = 1;

        double defensePierce = 1 - Math.Min(0.3, EnemyPierceResistance * FetchMultiplier(source.Stats, StatAttribute.Pierce));
        damageMultiplier *= 1 / (Math.Max(target.Stats[StatAttribute.Defense].Total, 1) * defensePierce);

        bool isPhysical = skill.GetSkillDamageType() == DamageType.Physical;
        StatAttribute resistanceStat = isPhysical ? StatAttribute.PhysicalRes : StatAttribute.MagicRes;
        StatAttribute attackStat = isPhysical ? StatAttribute.PhysicalAtk : StatAttribute.MagicAtk;
        StatAttribute piercingStat = isPhysical ? StatAttribute.PhysicalPiercing : StatAttribute.MagicPiercing;

        double targetRes = target.Stats[resistanceStat].Total;
        double attackType = source.Stats[attackStat].Total;
        double resPierce = FetchMultiplier(source.Stats, piercingStat);
        double resistance = (1500.0 - Math.Max(0, targetRes - 1500 * resPierce)) / 1500;

        // does this need to be divided by anything at all to account for raw physical attack?
        damageMultiplier *= attackType * resistance;

        // TODO: apply special standalone multipliers like Spicy Maple Noodles buff? it seems to have had it's own multiplier. new stat recommended
        const double FinalDamageMultiplier = 1;
        damageMultiplier *= FinalDamageMultiplier;

        double attackDamage = 300;

        if (source is IFieldActor<Player> player)
        {
            double bonusAttack = player.Stats[StatAttribute.BonusAtk].Total + 0.396 * player.Stats[StatAttribute.PetBonusAtk].Total;

            // TODO: properly fetch enemy bonus attack weakness from enemy buff. new stat recommended
            const double BonusAttackWeakness = 1;

            double bonusAttackCoeff = BonusAttackWeakness * GetBonusAttackCoefficient(player.Value);
            double minDamage = player.Stats[StatAttribute.MinWeaponAtk].Total + bonusAttackCoeff * bonusAttack;
            double maxDamage = player.Stats[StatAttribute.MaxWeaponAtk].Total + bonusAttackCoeff * bonusAttack;

            attackDamage = minDamage + (maxDamage - minDamage) * Random.Shared.NextDouble();
        }

        attackDamage *= damageMultiplier;

        return new(source, target, Math.Max(1, attackDamage), isCrit ? HitType.Critical : HitType.Normal);
    }

    private static bool RollCrit(IFieldActor source, IFieldActor target, double luckCoefficient)
    {
        // used to weigh crit rate in the formula, like how class luck coefficients weigh luck
        const double CritConstant = 5.3;

        // used to convert a percent value to a decimal value
        const double PercentageConversion = 0.015;

        const double MaxCritRate = 0.4;

        double luck = source.Stats[StatAttribute.Luk].Total * luckCoefficient;
        double critRate = source.Stats[StatAttribute.CritRate].Total * CritConstant;
        double critEvasion = Math.Max(target.Stats[StatAttribute.CritEvasion].Total, 1) * 2;
        double critChance = Math.Min(critRate / critEvasion * PercentageConversion, MaxCritRate);

        return Random.Shared.Next(1000) < 1000 * critChance;
    }

    private static double GetRarityBonusAttackMultiplier(Item item)
    {
        return (item?.Rarity ?? 0) switch
        {
            1 => 0.26,
            2 => 0.27,
            3 => 0.2883,
            4 => 0.5,
            5 => 1,
            6 => 1,
            _ => 0
        };
    }

    private static bool IsTwoHanded(Item item)
    {
        return item != null && item.IsTwoHand;
    }

    private static double GetWeaponBonusAttackMultiplier(Player player)
    {
        player.Inventory.Equips.TryGetValue(ItemSlot.RH, out Item rightHand);

        double weaponBonusAttackCoeff = GetRarityBonusAttackMultiplier(rightHand);

        if (!IsTwoHanded(rightHand))
        {
            player.Inventory.Equips.TryGetValue(ItemSlot.RH, out Item leftHand);

            weaponBonusAttackCoeff = 0.5 * (weaponBonusAttackCoeff + GetRarityBonusAttackMultiplier(rightHand));
        }

        return weaponBonusAttackCoeff;
    }

    private static double GetClassBonusAttackMultiplier(Job job)
    {
        return job switch
        {
            Job.Beginner => 1.039,
            Job.Knight => 1.105,
            Job.Berserker => 1.354,
            Job.Wizard => 1.398,
            Job.Priest => 0.975,
            Job.Archer => 1.143,
            Job.HeavyGunner => 1.364,
            Job.Thief => 1.151,
            Job.Assassin => 1.114,
            Job.Runeblade => 1.259,
            Job.Striker => 1.264,
            Job.SoulBinder => 1.177,
            _ => 1,
        };
    }

    private static double GetBonusAttackCoefficient(Player player)
    {
        return 4.96 * GetWeaponBonusAttackMultiplier(player) * GetClassBonusAttackMultiplier(player.Job);
    }
}
