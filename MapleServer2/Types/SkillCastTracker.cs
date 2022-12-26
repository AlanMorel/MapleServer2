using MapleServer2.Data.Static;
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
    public long LastTick = 0;
    public long InitialCastEnd = 0;
    public SkillCast Cast;
    public int CurrentMotion = 0;
    public List<DamageInstance> Damages = new();
    public int DamagesProcessed = 0;
    public int QueuedSyncDamages = 0;

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

    public void AddSkillCast(SkillCast skillCast, long initialCastEnd)
    {
        lock (SkillCasts)
        {
            long currentTick = Parent.FieldManager?.TickCount64 ?? 0;

            SkillCasts.Add(new(skillCast)
            {
                LastTick = currentTick,
                InitialCastEnd = initialCastEnd
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
            LongestRefreshTime = Math.Max(LongestRefreshTime, Math.Max(0, currentTick - Math.Max(skillCast.LastTick, skillCast.InitialCastEnd)));

            skillCast.LastTick = currentTick;
        }

        return skillCast;
    }

    public void QueueSyncDamages(long skillSn, int count)
    {
        CastedSkill? skillCast = GetSkillCast(skillSn);

        if (skillCast is null)
        {
            return;
        }

        skillCast.QueuedSyncDamages += count;
    }

    public void DamageProcessed(long skillSn, int count = 1)
    {
        CastedSkill? skillCast = GetSkillCast(skillSn);

        if (skillCast is null)
        {
            return;
        }

        skillCast.DamagesProcessed += count;
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

                long timeElapsed = currentTick - Math.Max(skillCast.LastTick, skillCast.InitialCastEnd);
                long timeout = skillCast.DamagesProcessed >= skillCast.QueuedSyncDamages ? 500 : 10000;

                if (timeElapsed > timeout)
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
