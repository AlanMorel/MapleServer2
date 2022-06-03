using Maple2Storage.Enums;

namespace MapleServer2.Types;

public class DamageHandler
{
    public IFieldActor Source { get; init; }
    public IFieldActor Target { get; init; }
    public double Damage { get; private set; }
    public bool IsCrit { get; private set; }

    private DamageHandler(double damage, bool isCrit)
    {
        Damage = damage;
        IsCrit = isCrit;
    }

    private DamageHandler(IFieldActor source, IFieldActor target, double damage, bool isCrit)
    {
        Source = source;
        Target = target;
        Damage = damage;
        IsCrit = isCrit;
    }

    public static DamageHandler CalculateSkillDamage(SkillCast skillCast)
    {
        return new(skillCast.GetDamageRate(), false);
    }

    public static List<DamageHandler> CalculateDamage(SkillCast skill, IFieldActor<Player> source, IEnumerable<IFieldActor> targets)
    {
        if (source.Value.GmFlags.Contains("oneshot"))
        {
            return targets.Select(t => new DamageHandler(t.Stats[StatAttribute.Hp].Total, true)).ToList();
        }

        // get luck coefficient from class. new stat recommended, can be refactored away like isCrit was
        return CalculateDamage(skill, source, targets, 1);
    }

    public static List<DamageHandler> CalculateDamage(SkillCast skill, IFieldActor source, IEnumerable<IFieldActor> targets, double luckCoefficient)
    {
        return targets.Select(t => CalculateDamage(skill, source, t, 1)).ToList();
    }

    public static DamageHandler CalculateDamage(SkillCast skill, IFieldActor<Player> source, IFieldActor target)
    {
        if (source.Value.GmFlags.Contains("oneshot"))
        {
            return new(source, target, target.Stats[StatAttribute.Hp].Total, true);
        }

        // get luck coefficient from class. new stat recommended, can be refactored away like isCrit was
        return CalculateDamage(skill, (IFieldActor) source, target, 1);
    }

    public static DamageHandler CalculateDamage(SkillCast skill, IFieldActor source, IFieldActor target, double luckCoefficient)
    {
        // TODO: get accuracyWeakness from enemy stats from enemy buff. new stat recommended
        double accuracyWeakness = 0;
        double hitRate = (source.Stats[StatAttribute.Accuracy].Total + accuracyWeakness) / Math.Max(target.Stats[StatAttribute.Evasion].Total, 0.1);

        if (Random.Shared.Next(1000) > hitRate)
        {
            return new(source, target, 0, false); // we missed
        }

        bool isCrit = skill.IsGuaranteedCrit() || RollCrit(source, target, luckCoefficient);

        double finalCritDamage = 1;

        if (isCrit)
        {
            // TODO: get critResist from enemy stats from enemy buff. new stat recommended
            double critResist = 1;
            double critDamage = source.Stats[StatAttribute.CritDamage].Total;
            finalCritDamage = critResist * ((critDamage / 100) - 1) + 1;
        }

        double damageBonus = 100 + source.Stats[StatAttribute.TotalDamage].Total;

        damageBonus *= finalCritDamage;

        switch (skill.GetElement())
        {
            case Element.Fire:
                damageBonus += source.Stats[StatAttribute.FireDamage].Total;
                break;
            case Element.Ice:
                damageBonus += source.Stats[StatAttribute.IceDamage].Total;
                break;
            case Element.Electric:
                damageBonus += source.Stats[StatAttribute.ElectricDamage].Total;
                break;
            case Element.Holy:
                damageBonus += source.Stats[StatAttribute.HolyDamage].Total;
                break;
            case Element.Dark:
                damageBonus += source.Stats[StatAttribute.DarkDamage].Total;
                break;
            case Element.Poison:
                damageBonus += source.Stats[StatAttribute.PoisonDamage].Total;
                break;
        }

        // TODO: properly check for melee vs ranged. new skill attribute recommended

        const bool isMelee = true;

        damageBonus += isMelee ? source.Stats[StatAttribute.MeleeDamage].Total : source.Stats[StatAttribute.RangedDamage].Total;

        bool isBoss = false;

        if (target is INpc npc)
        {
            isBoss = npc.Value.IsBoss();
        }

        damageBonus += isBoss ? source.Stats[StatAttribute.BossDamage].Total : 0;

        // TODO: properly fetch enemy attack speed weakness from enemy buff. new stat recommended

        const double attackSpeedWeakness = 0;

        damageBonus += attackSpeedWeakness * source.Stats[StatAttribute.AttackSpeed].Total;

        double damageMultiplier = (damageBonus / 100) * skill.GetDamageRate();

        // TODO: properly fetch enemy pierce resistance from enemy buff. new stat recommended

        const double enemyPierceResistance = 1;
        double defensePierce = 1 - Math.Min(30, enemyPierceResistance * source.Stats[StatAttribute.Pierce].Total) / 100;
        damageMultiplier *= 100 / (target.Stats[StatAttribute.Defense].Total * defensePierce);

        double targetRes = (skill.GetSkillDamageType() == DamageType.Physical) ? target.Stats[StatAttribute.PhysicalRes].Total : target.Stats[StatAttribute.MagicRes].Total;
        double attackType = (skill.GetSkillDamageType() == DamageType.Physical) ? source.Stats[StatAttribute.PhysicalAtk].Total : source.Stats[StatAttribute.MagicAtk].Total;
        double resPierce = (skill.GetSkillDamageType() == DamageType.Physical) ? source.Stats[StatAttribute.PhysicalPiercing].Total : source.Stats[StatAttribute.MagicPiercing].Total;
        double resistance = (1500.0 - Math.Max(0, targetRes - 15 * resPierce)) / 1500;

        // does this need to be divided by anything at all to account for raw physical attack?
        damageMultiplier *= attackType * resistance;

        // TODO: apply special standalone multipliers like Spicy Maple Noodles buff? it seems to have had it's own multiplier. new stat recommended

        double finalDamageMultiplier = 100;

        damageMultiplier *= finalDamageMultiplier / 100;

        double attackDamage = 300;

        if (source is IFieldActor<Player> player)
        {
            double bonusAttack = source.Stats[StatAttribute.BonusAtk].Total + 0.4 * source.Stats[StatAttribute.PetBonusAtk].Total;
            double bonusAttackWeakness = 1;

            // TODO: properly fetch enemy bonus attack weakness from enemy buff. new stat recommended

            double minDamage = source.Stats[StatAttribute.MinWeaponAtk].Total + bonusAttackWeakness * bonusAttack;
            double maxDamage = source.Stats[StatAttribute.MaxWeaponAtk].Total + bonusAttackWeakness * bonusAttack;

            attackDamage = minDamage + (maxDamage - minDamage) * Random.Shared.NextDouble();
        }

        attackDamage *= damageMultiplier;

        return new(source, target, attackDamage, isCrit);
    }

    public static bool RollCrit(IFieldActor source, IFieldActor target, double luckCoefficient)
    {
        // used to weigh crit rate in the formula, like how class luck coefficients weigh luck
        const double critConstant = 5.3;

        // used to convert a percent value to a decimal value
        const double percentageConversion = 0.015;

        const double maxCritRate = 0.4;

        double luck = source.Stats[StatAttribute.Luk].Total * luckCoefficient;
        double critRate = source.Stats[StatAttribute.CritRate].Total * critConstant;
        double critEvasion = Math.Max(target.Stats[StatAttribute.CritEvasion].Total, 1) * 2;
        double critChance = Math.Min(critRate / critEvasion * percentageConversion, maxCritRate);

        return Random.Shared.Next(1000) < 1000 * critChance;
    }
}
