namespace MapleServer2.Enums
{
    public enum Job : int
    {
        None = 0,
        Beginner = 1,
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
        Beginner = 10,
        Knight = 100,
        Knight2 = 101,
        Berserker = 200,
        Berserker2 = 201,
        Wizard = 300,
        Wizard2 = 301,
        Priest = 400,
        Priest2 = 401,
        Archer = 500,
        Archer2 = 501,
        HeavyGunner = 600,
        HeavyGunner2 = 601,
        Thief = 700,
        Thief2 = 701,
        Assassin = 800,
        Assassin2 = 801,
        Runeblade = 900,
        Runeblade2 = 901,
        Striker = 1000,
        Striker2 = 1001,
        SoulBinder = 1100,
        SoulBinder2 = 1101,
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
        Thief = HeavyGunner,
        Assassin = 8,
        Runeblade = Assassin,
        Striker = 12,
        SoulBinder = 16,
        GameMaster = 1,
        Beginner = GameMaster
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
