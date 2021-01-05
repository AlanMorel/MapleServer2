namespace MapleServer2.Types
{
    public struct Skill
    {
        public int Id { get; private set; }
        public short Level { get; private set; }
        public byte Learned { get; private set; }
        public string Feature { get; private set; }
        public int[] Sub { get; private set; }

        public Skill(int id, short level, byte learned, string feature = "", int[] sub = null)
        {
            Id = id;
            Level = level;
            Learned = learned;
            Feature = feature;
            Sub = sub;
        }

        public override string ToString() => $"SKILL({Id:X2}, {Level:X2}, {Learned:X2}, {Feature:X2}, {Sub:X2})";
    }
}
