using Maple2Storage.Enums;
using Maple2Storage.Tools;

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

    public static List<DamageHandler> CalculateDamage(SkillCast skill, IFieldActor<Player> source, IEnumerable<IFieldActor> targets, bool isCrit = false)
    {
        if (source.Value.GmFlags.Contains("oneshot"))
        {
            return targets.Select(t => new DamageHandler(t.Stats[StatId.Hp].Total, true)).ToList();
        }

        return CalculateDamage(skill, source, targets, isCrit);
    }

    public static List<DamageHandler> CalculateDamage(SkillCast skill, IFieldActor source, IEnumerable<IFieldActor> targets, bool isCrit = false)
    {
        return targets.Select(t => CalculateDamage(skill, source, t, isCrit)).ToList();
    }

    public static DamageHandler CalculateDamage(SkillCast skill, IFieldActor<Player> source, IFieldActor target, bool isCrit = false)
    {
        if (source.Value.GmFlags.Contains("oneshot"))
        {
            return new(target.Stats[StatId.Hp].Total, true);
        }

        return CalculateDamage(skill, (IFieldActor) source, target, isCrit);
    }

    public static DamageHandler CalculateDamage(SkillCast skill, IFieldActor source, IFieldActor target, bool isCrit = false)
    {
        // TODO: Calculate attack damage w/ stats
        double attackDamage = 300;
        double skillDamageRate = isCrit ? skill.GetCriticalDamage() : skill.GetDamageRate();
        double skillDamage = skillDamageRate * attackDamage;
        double targetRes = (skill.GetSkillDamageType() == DamageType.Physical) ? target.Stats[StatId.PhysicalRes].Total : target.Stats[StatId.MagicRes].Total;
        double resPierce = (skill.GetSkillDamageType() == DamageType.Physical) ? source.Stats[StatId.PhysicalAtk].Total : source.Stats[StatId.MagicAtk].Total;
        // TODO: Fix damage multiplier (add pet?)
        double numerator = skillDamage * (1 + source.Stats[StatId.BonusAtk].Total) * (1500 - (targetRes - resPierce * 15));

        double pierceCoeff = 1 - source.Stats[StatId.Pierce].Total;
        // TODO: Find correct enemy defense stats
        double denominator = target.Stats[StatId.CritEvasion].Total * pierceCoeff * 15;

        return new(source, target, numerator / denominator, isCrit);
    }

    public static bool RollCrit(int critRate = 0)
    {
        return RandomProvider.Get().Next(1000) < Math.Clamp(50 + critRate, 0, 400);
    }
}
