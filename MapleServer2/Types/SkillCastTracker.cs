using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using MapleServer2.Managers;
using MapleServer2.Managers.Actors;

namespace MapleServer2.Types;

public class DamageInstance
{
    public int AttackId;
    public int SourceId;
    public int TargetId;
    public short Animation;
    public int AttackPoint;
}

public class CastedSkill
{
    public long LastTick;
    public SkillCast Cast;
    public int CurrentMotion = 0;
    public List<DamageInstance> Damages = new();

    public CastedSkill(SkillCast cast)
    {
        Cast = cast;
    }
}

public class SkillCastTracker
{
    public IFieldActor Parent;

    public List<CastedSkill> SkillCasts = new();

    public SkillCastTracker(IFieldActor parent)
    {
        Parent = parent;
    }

    public void AddSkillCast(SkillCast skillCast)
    {
        lock (SkillCasts)
        {
            SkillCasts.Add(new(skillCast)
            {
                LastTick = Parent.FieldManager?.TickCount64 ?? 0
            });
        }
    }

    private static long LongestRefreshTime = 0;

    public CastedSkill? GetSkillCast(long skillSn)
    {
        CastedSkill? skillCast = null;

        lock (SkillCasts)
        {
            skillCast = SkillCasts.FirstOrDefault((skillCast) => skillCast?.Cast.SkillSn == skillSn, null);
        }

        if (skillSn != 0 && skillCast is null && Parent is Character character)
        {
            character.Value.Session?.SendNotice($"Skill cast {skillSn} not found.");
        }

        long currentTick = Parent.FieldManager?.TickCount64 ?? 0;

        if (skillCast is not null && currentTick != 0)
        {
            LongestRefreshTime = Math.Max(LongestRefreshTime, currentTick - skillCast.LastTick);

            skillCast.LastTick = currentTick;
        }

        return skillCast;
    }

    public void Update()
    {
        if (Parent.FieldManager is null)
        {
            lock (SkillCasts)
            {
                SkillCasts.Clear();
            }

            return;
        }

        long currentTick = Parent.FieldManager.TickCount64;

        int removed = 0;

        lock (SkillCasts)
        {
            for (int i = 0; i < SkillCasts.Count; ++i)
            {
                CastedSkill skillCast = SkillCasts[i];

                if (currentTick - skillCast.LastTick > 10000)
                {
                    ++removed;

                    continue;
                }

                SkillCasts[i - removed] = SkillCasts[i];
            }

            if (removed > 0)
            {
                SkillCasts.RemoveRange(SkillCasts.Count - removed, removed);
            }
        }
    }
}
