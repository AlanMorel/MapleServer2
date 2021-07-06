using System;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class Status
    {
        public int UniqueId { get; set; }
        public int SkillId { get; set; }
        public int Owner { get; set; }
        public int Source { get; set; }
        public int Stacks { get; set; }
        public short Level { get; set; }
        public int Start { get; set; }
        public int End { get; set; }

        public SkillCast SkillCast { get; set; }

        public Status() { }

        public Status(SkillCast skillCast, int owner, int source, int duration, int stacks)
        {
            SkillId = skillCast.SkillId;
            UniqueId = GuidGenerator.Int();
            Owner = owner;
            Source = source;
            Level = skillCast.SkillLevel;
            Stacks = Math.Max(1, stacks);
            SkillCast = skillCast;
            Start = Environment.TickCount;
            End = Start + duration;
        }

        public Status(int id, int owner, int source, short level, int duration, int stacks)
        {
            SkillId = id;
            UniqueId = GuidGenerator.Int();
            Owner = owner;
            Source = source;
            Level = level;
            Stacks = Math.Max(1, stacks);
            Start = Environment.TickCount;
            End = Start + duration;
        }

        public void Overlap(Status other)
        {
            Owner = other.Owner;
            Source = other.Source;
            Start = other.Start;
            End = other.End;
            Level = other.Level;
            Stacks = other.Stacks;
        }

        public void AddStacks(int value) => Stacks += value;
    }
}
