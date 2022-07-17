using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapleServer2.Types;

public struct EffectTriggers
{
    public bool IsEvent;
    public int SkillId;
    public int EffectId;
    public IFieldActor Owner;
    public IFieldActor Target;
    public IFieldActor Caster;

    public EffectTriggers()
    {
        IsEvent = false;
        SkillId = 0;
        EffectId = 0;
        Owner = null;
        Target = null;
        Caster = null;
    }

    public void CopyEvents(EffectTriggers source)
    {
        IsEvent = source.IsEvent;

        if (!IsEvent)
        {
            return;
        }

        SkillId = source.SkillId;
        EffectId = source.EffectId;
    }
}
