namespace Maple2Storage.Enums {
    public enum JobType {
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

    public enum JobCode
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