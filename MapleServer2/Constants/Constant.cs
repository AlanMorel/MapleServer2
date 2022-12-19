using MapleServer2.Enums;

namespace MapleServer2.Constants;

public static class Constant
{
    public const float UnitPerMs = 0.001f; // Velocity in which all NPCs move per ms. 150 units per second.
    public const int MountStaminaConsumption = 50; // Amount of stamina consumed each tick when sprinting with a ground mount.
    public const double PetAttackMultiplier = 0.396f; // Applied to pet attack in the damage formula when adding it to bonus attack
    public const string LocalHost = "127.0.0.1";

    public static long SpiritRegen(JobCode jobCode)
    {
        return jobCode switch
        {
            JobCode.Beginner => 1,
            JobCode.Knight => 0,
            JobCode.Berserker => 1,
            JobCode.Wizard => 1,
            JobCode.Priest => 8,
            JobCode.Archer => 7,
            JobCode.HeavyGunner => 0,
            JobCode.Thief => 1,
            JobCode.Assassin => 1,
            JobCode.Runeblade => 1,
            JobCode.Striker => 1,
            JobCode.SoulBinder => 1,
            _ => 0,
        };
    }

    public static long SpiritRegenInterval(JobCode jobCode)
    {
        return jobCode switch
        {
            JobCode.Beginner => 100,
            JobCode.Knight => 300,
            JobCode.Berserker => 100,
            JobCode.Wizard => 100,
            JobCode.Priest => 500,
            JobCode.Archer => 500,
            JobCode.HeavyGunner => 0,
            JobCode.Thief => 100,
            JobCode.Assassin => 100,
            JobCode.Runeblade => 100,
            JobCode.Striker => 100,
            JobCode.SoulBinder => 1000,
            _ => 0,
        };
    }

    public static float SpiritRegenIntervalRate(JobCode jobCode)
    {
        return jobCode switch
        {
            JobCode.Beginner => 1,
            JobCode.Knight => 1,
            JobCode.Berserker => 1,
            JobCode.Wizard => 2,
            JobCode.Priest => 1,
            JobCode.Archer => 1,
            JobCode.HeavyGunner => 1,
            JobCode.Thief => 1,
            JobCode.Assassin => 1,
            JobCode.Runeblade => 2,
            JobCode.Striker => 1,
            JobCode.SoulBinder => 1,
            _ => 35,
        };
    }
}
