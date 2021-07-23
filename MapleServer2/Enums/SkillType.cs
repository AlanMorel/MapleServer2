namespace MapleServer2.Enums
{
    public enum SkillType : int
    {
        None = 0,
        Attack = 1
    }

    public enum SkillSubType : int
    {
        None = 0,
        Status = 2
    }

    public enum BuffType : int
    {
        None = 0,
        Buff = 1,
        Debuff = 2
    }

    public enum BuffSubType : int
    {
        None = 0,
        Owner = 1,
        Entity = 2,
        Element = 4, // Damage Per Second
        Shield = 6,
        Unknown = 8,
        Recovery = 16,
        Unknown2 = 64,
        Unknown3 = 1024
    }

    public enum BuffCategroy : int
    {
        Unknown = 7,
        Unknown2 = 8
    }
}
