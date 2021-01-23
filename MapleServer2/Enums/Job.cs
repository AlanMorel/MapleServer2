namespace MapleServer2.Enums
{
    public enum Job : int
    {
        None = 0,
        Knight = 10,
        Berserker = 20,
        Wizard = 30,
        Priest = 40,
        Archer = 50,
        HeavyGunner = 60,
        Thief = 70,
        Assassin = 80,
        Runeblade = 90,
        Striker = 100,
        SoulBinder = 110,
        GameMaster = 999
    }

    public enum JobCode : int
    {
        None = 0,
        Knight = 101,
        Berserker = 201,
        Wizard = 301,
        Priest = 401,
        Archer = 501,
        HeavyGunner = 601,
        Thief = 701,
        Assassin = 801,
        Runeblade = 901,
        Striker = 1001,
        SoulBinder = 1101,
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
