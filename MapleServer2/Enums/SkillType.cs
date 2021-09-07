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
        Element = 4, // Type Element, Damage_Per_Second
        Shield = 6,
        CrowlControl = 8, // Stun, Slow, Freeze, Shock
        Recovery = 16,
        Unknown2 = 64,
        Unknown3 = 1024
    }

    public enum BuffCategroy : int
    {
        Stun = 7,
        Slow = 8
    }
}
