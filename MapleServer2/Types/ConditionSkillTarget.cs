using Maple2Storage.Enums;

namespace MapleServer2.Types;

public struct ConditionSkillTarget
{
    public IFieldActor Owner;
    public IFieldActor? Target;
    public IFieldActor Caster;
    public IFieldActor? Attacker = null;
    public EffectEventOrigin EventOrigin;

    public ConditionSkillTarget(IFieldActor? owner, IFieldActor? target, IFieldActor? caster, IFieldActor? attacker = null, EffectEventOrigin eventOrigin = EffectEventOrigin.Owner)
    {
        Owner = owner;
        Target = target;
        Caster = caster;
        Attacker = attacker;
        EventOrigin = eventOrigin;
    }
}
