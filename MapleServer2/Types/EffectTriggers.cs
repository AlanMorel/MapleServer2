using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapleServer2.Types;

public struct EffectTriggers
{
    public bool IsEvent = false;
    public int SkillId = 0;
    public int EffectId = 0;
    public IFieldActor Owner = null;
    public IFieldActor Target = null;
    public IFieldActor Caster = null;

    public EffectTriggers()
    {
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
