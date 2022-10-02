namespace MapleServer2.Types;

public struct ConditionSkillTarget
{
    public IFieldActor Owner;
    public IFieldActor Target;
    public IFieldActor Caster;
    public IFieldActor? Attacker = null;

    public ConditionSkillTarget(IFieldActor owner, IFieldActor target, IFieldActor caster, IFieldActor? attacker = null)
    {
        Owner = owner;
        Target = target;
        Caster = caster;
        Attacker = attacker;
    }
}
