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
            double critDamage = source.Stats[StatAttribute.CritDamage].Total;
            finalCritDamage = CritResist * ((critDamage / 100) - 1) + 1;
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
        const bool IsMelee = true;

        damageBonus += IsMelee ? source.Stats[StatAttribute.MeleeDamage].Total : source.Stats[StatAttribute.RangedDamage].Total;

        bool isBoss = false;

        if (target is INpc npc)
        {
            isBoss = npc.Value.IsBoss();
        }

        damageBonus += isBoss ? source.Stats[StatAttribute.BossDamage].Total : 0;

        // TODO: properly fetch enemy attack speed weakness from enemy buff. new stat recommended
        const double AttackSpeedWeakness = 0;

        damageBonus += AttackSpeedWeakness * source.Stats[StatAttribute.AttackSpeed].Total;

        double damageMultiplier = (damageBonus / 100) * skill.GetDamageRate();

        // TODO: properly fetch enemy pierce resistance from enemy buff. new stat recommended
        const double EnemyPierceResistance = 1;

        double defensePierce = 1 - Math.Min(30, EnemyPierceResistance * source.Stats[StatAttribute.Pierce].Total) / 100;
        damageMultiplier *= 100 / (target.Stats[StatAttribute.Defense].Total * defensePierce);

        bool isPhysical = skill.GetSkillDamageType() == DamageType.Physical;
        StatAttribute resistanceStat = isPhysical ? StatAttribute.PhysicalRes : StatAttribute.MagicRes;
        StatAttribute attackStat = isPhysical ? StatAttribute.PhysicalAtk : StatAttribute.MagicAtk;
        StatAttribute piercingStat = isPhysical ? StatAttribute.PhysicalPiercing : StatAttribute.MagicPiercing;

        double targetRes = target.Stats[resistanceStat].Total;
        double attackType = source.Stats[attackStat].Total;
        double resPierce = source.Stats[piercingStat].Total;
        double resistance = (1500.0 - Math.Max(0, targetRes - 15 * resPierce)) / 1500;

        // does this need to be divided by anything at all to account for raw physical attack?
        damageMultiplier *= attackType * resistance;

        // TODO: apply special standalone multipliers like Spicy Maple Noodles buff? it seems to have had it's own multiplier. new stat recommended
        const double FinalDamageMultiplier = 100;
        damageMultiplier *= FinalDamageMultiplier / 100;

        double attackDamage = 300;

        if (source is IFieldActor<Player> player)
        {
            double bonusAttack = player.Stats[StatAttribute.BonusAtk].Total + 0.4 * player.Stats[StatAttribute.PetBonusAtk].Total;

            // TODO: properly fetch enemy bonus attack weakness from enemy buff. new stat recommended
            const double BonusAttackWeakness = 1;

            double minDamage = player.Stats[StatAttribute.MinWeaponAtk].Total + BonusAttackWeakness * bonusAttack;
            double maxDamage = player.Stats[StatAttribute.MaxWeaponAtk].Total + BonusAttackWeakness * bonusAttack;

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
}
