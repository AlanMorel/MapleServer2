namespace MapleServer2.Enums
{
    public enum Job : int
    {
        None = 0,
        Knight = 100,
        Berserker = 200,
        Wizard = 300,
        Priest = 400,
        Archer = 500,
        HeavyGunner = 600,
        Thief = 700,
        Assassin = 800,
        Runeblade = 900,
        Striker = 1000,
        SoulBinder = 1100,
        GameMaster = 999
    }

    public enum JobCode : int
    {
        None = 0,
        Knight = 101,
        Berserker = 102,
        Wizard = 103,
        Priest = 104,
        Archer = 105,
        HeavyGunner = 106,
        Thief = 107,
        Assassin = 108,
        Runeblade = 109,
        Striker = 110,
        SoulBinder = 111,
        GameMaster = 999
    }

    public enum JobSkillSplit : byte
    {
        None = 0,
        Knight = 9,
        Berserker = 15,
        Wizard = 17,
        Priest = 10,
        Archer = 14,
        HeavyGunner = 13,
        Thief = 13,
        Assassin = 8,
        Runeblade = 8,
        Striker = 12,
        SoulBinder = 16,
        GameMaster = 0
    }
}

/* SkillIds
Common 200x (Wall Climbing, Swift Swim)
Knight 101x
Berserker 102x
Wizard 103x
Priest = 104x
Archer = 105x
HeavyGunner = 106x
Thief = 107x
Assassin = 108x
Runeblade = 109x
Striker = 110x
SoulBinder = 111x

Awakening Skills (+100)
Example: ArcherSkillId = 10500X00
Basic: X = 0
Awakening: X = 1
*/
