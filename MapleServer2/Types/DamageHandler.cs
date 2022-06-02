using Maple2Storage.Enums;
using MapleServer2.Enums;

namespace MapleServer2.Types;

public class DamageHandler
{
    public IFieldActor Source { get; }
    public IFieldActor Target { get; }
    public double Damage { get; }
    public bool IsCrit { get; }
    public HitType HitType { get; }

    private DamageHandler(double damage, bool isCrit)
    {
        Damage = damage;
        IsCrit = isCrit;
    }

    private DamageHandler(IFieldActor source, IFieldActor target, double damage, bool isCrit, HitType hitType)
    {
        Source = source;
        Target = target;
        Damage = damage;
        IsCrit = isCrit;
        HitType = hitType;
    }

    public static DamageHandler CalculateDamage(SkillCast skill, IFieldActor<Player> source, IFieldActor target, bool isCrit = false)
    {
        if (source.Value.GmFlags.Contains("oneshot"))
        {
            return new(source, target, target.Stats[StatAttribute.Hp].Total, true, HitType.Critical);
        }

        return CalculateDamage(skill, (IFieldActor) source, target, isCrit);
    }

    public static DamageHandler CalculateDamage(SkillCast skill, IFieldActor source, IFieldActor target, bool isCrit = false)
    {
        // TODO: Calculate attack damage w/ stats
        double attackDamage = 300;
        double skillDamageRate = isCrit ? skill.GetCriticalDamage() : skill.GetDamageRate();
        double skillDamage = skillDamageRate * attackDamage;
        double targetRes = (skill.GetSkillDamageType() == DamageType.Physical) ? target.Stats[StatAttribute.PhysicalRes].Total : target.Stats[StatAttribute.MagicRes].Total;
        double resPierce = (skill.GetSkillDamageType() == DamageType.Physical) ? source.Stats[StatAttribute.PhysicalAtk].Total : source.Stats[StatAttribute.MagicAtk].Total;
        // TODO: Fix damage multiplier (add pet?)
        double numerator = skillDamage * (1 + source.Stats[StatAttribute.BonusAtk].Total) * (1500 - (targetRes - resPierce * 15));

        double pierceCoeff = 1 - source.Stats[StatAttribute.Pierce].Total;
        // TODO: Find correct enemy defense stats
        double denominator = target.Stats[StatAttribute.CritEvasion].Total * pierceCoeff * 15;

        return new(source, target, numerator / denominator, isCrit, HitType.Normal);
    }

    public static bool RollCrit(int critRate = 0)
    {
        return Random.Shared.Next(1000) < Math.Clamp(50 + critRate, 0, 400);
    }
}
