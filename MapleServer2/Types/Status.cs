using MapleServer2.Tools;

namespace MapleServer2.Types;

public class Status
{
    public int UniqueId { get; set; }
    public int SkillId { get; set; }
    public int Target { get; set; }
    public int Source { get; set; }
    public int Stacks { get; set; }
    public short Level { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
    public int Duration { get; set; }

    public SkillCast? SkillCast;

    public Status() { }

    public Status(SkillCast skillCast, int target, int source, int stacks)
    {
        SkillId = skillCast.SkillId;
        UniqueId = GuidGenerator.Int();
        Target = target;
        Source = source;
        Level = skillCast.SkillLevel;
        Stacks = stacks > 0 && stacks <= skillCast.MaxStack() ? stacks : skillCast.MaxStack();
        SkillCast = skillCast;
        Start = Environment.TickCount;
        Duration = skillCast.DurationTick();
        End = Start + Duration;
    }

    public Status(int id, int target, int source, short level, int duration, int stacks)
    {
        SkillId = id;
        UniqueId = GuidGenerator.Int();
        Target = target;
        Source = source;
        Level = level;
        Stacks = Math.Max(1, stacks);
        Start = Environment.TickCount;
        End = Start + duration;
        Duration = duration;
    }

    public void Overlap(Status other)
    {
        Target = other.Target;
        Source = other.Source;
        Start = other.Start;
        End = other.End;
        Level = other.Level;
        Stacks = other.Stacks;
        Duration = other.Duration;
    }

    public void AddStacks(int value)
    {
        Stacks += value;
    }
}
