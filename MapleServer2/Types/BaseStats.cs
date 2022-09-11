using MapleServer2.Enums;

namespace MapleServer2.Types;

public class BaseStats
{
    private const int HP_SPLIT_LEVEL = 50;
    private const double RES_DIVISOR = 99.0;

    public static long Strength(Job job, short level)
    {
        long result = job switch
        {
            Job.Beginner => 7,
            Job.Knight => 8,
            Job.Berserker => 8,
            Job.Wizard => 1,
            Job.Priest => 1,
            Job.Archer => 6,
            Job.HeavyGunner => 2,
            Job.Thief => 6,
            Job.Assassin => 2,
            Job.Runeblade => 8,
            Job.Striker => 6,
            Job.SoulBinder => 1,
            _ => 1,
        };
        for (int i = 1; i < Math.Min(level, (short) 50); i++)
        {
            result += job switch
            {
                Job.Beginner => 7,
                Job.Knight => 7,
                Job.Berserker => 7,
                Job.Wizard => 0,
                Job.Priest => 0,
                Job.Archer => 1,
                Job.HeavyGunner => 0,
                Job.Thief => 1,
                Job.Assassin => 0,
                Job.Runeblade => 7,
                Job.Striker => 1,
                Job.SoulBinder => 0,
                _ => 0,
            };
        }
        for (int i = 50; i < level; i++)
        {
            result += job switch
            {
                Job.Beginner => 1,
                Job.Knight => 1,
                Job.Berserker => 1,
                Job.Wizard => 0,
                Job.Priest => 0,
                Job.Archer => i % 3 == 0 ? 1 : 0,
                Job.HeavyGunner => 0,
                Job.Thief => i % 3 == 0 ? 1 : 0,
                Job.Assassin => 0,
                Job.Runeblade => 1,
                Job.Striker => i % 3 == 0 ? 1 : 0,
                Job.SoulBinder => 0,
                _ => 0,
            };
        }

        return result;
    }

    public static long Dexterity(Job job, short level)
    {
        long result = job switch
        {
            Job.Beginner => 6,
            Job.Knight => 6,
            Job.Berserker => 6,
            Job.Wizard => 1,
            Job.Priest => 1,
            Job.Archer => 8,
            Job.HeavyGunner => 8,
            Job.Thief => 2,
            Job.Assassin => 6,
            Job.Runeblade => 6,
            Job.Striker => 8,
            Job.SoulBinder => 1,
            _ => 1,
        };
        for (int i = 1; i < Math.Min(level, (short) 50); i++)
        {
            result += job switch
            {
                Job.Beginner => 0,
                Job.Knight => 1,
                Job.Berserker => 1,
                Job.Wizard => 0,
                Job.Priest => 0,
                Job.Archer => 7,
                Job.HeavyGunner => 7,
                Job.Thief => 0,
                Job.Assassin => 1,
                Job.Runeblade => 1,
                Job.Striker => 7,
                Job.SoulBinder => 0,
                _ => 0,
            };
        }
        for (int i = 50; i < level; i++)
        {
            result += job switch
            {
                Job.Beginner => 0,
                Job.Knight => i % 3 == 0 ? 1 : 0,
                Job.Berserker => i % 3 == 0 ? 1 : 0,
                Job.Wizard => 0,
                Job.Priest => 0,
                Job.Archer => 1,
                Job.HeavyGunner => 1,
                Job.Thief => 0,
                Job.Assassin => i % 3 == 0 ? 1 : 0,
                Job.Runeblade => i % 3 == 0 ? 1 : 0,
                Job.Striker => 1,
                Job.SoulBinder => 0,
                _ => 0,
            };
        }

        return result;
    }

    public static long Intelligence(Job job, short level)
    {
        long result = job switch
        {
            Job.Beginner => 2,
            Job.Knight => 2,
            Job.Berserker => 1,
            Job.Wizard => 14,
            Job.Priest => 14,
            Job.Archer => 1,
            Job.HeavyGunner => 1,
            Job.Thief => 1,
            Job.Assassin => 1,
            Job.Runeblade => 2,
            Job.Striker => 1,
            Job.SoulBinder => 14,
            _ => 1,
        };
        for (int i = 1; i < Math.Min(level, (short) 50); i++)
        {
            result += job switch
            {
                Job.Beginner => i % 3 != 1 ? 1 : 0,
                Job.Knight => i % 2 == 0 ? 1 : 0,
                Job.Berserker => i % 2 == 1 ? 1 : 0,
                Job.Wizard => 8,
                Job.Priest => 8,
                Job.Archer => i % 2 == 0 ? 1 : 0,
                Job.HeavyGunner => i % 2 == 0 ? 1 : 0,
                Job.Thief => i % 2 == 0 ? 1 : 0,
                Job.Assassin => i % 2 == 0 ? 1 : 0,
                Job.Runeblade => i % 2 == 0 ? 1 : 0,
                Job.Striker => i % 2 == 1 ? 1 : 0,
                Job.SoulBinder => 8,
                _ => 0,
            };
        }
        for (int i = 50; i < level; i++)
        {
            result += job switch
            {
                Job.Beginner => 0,
                Job.Knight => 0,
                Job.Berserker => 0,
                Job.Wizard => 1,
                Job.Priest => 1,
                Job.Archer => 0,
                Job.HeavyGunner => 0,
                Job.Thief => 0,
                Job.Assassin => 0,
                Job.Runeblade => 0,
                Job.Striker => 0,
                Job.SoulBinder => 1,
                _ => 0,
            };
        }

        return result;
    }

    public static long Luck(Job job, short level)
    {
        long result = job switch
        {
            Job.Beginner => 2,
            Job.Knight => 1,
            Job.Berserker => 2,
            Job.Wizard => 1,
            Job.Priest => 1,
            Job.Archer => 2,
            Job.HeavyGunner => 6,
            Job.Thief => 8,
            Job.Assassin => 8,
            Job.Runeblade => 1,
            Job.Striker => 2,
            Job.SoulBinder => 1,
            _ => 1,
        };
        for (int i = 1; i < Math.Min(level, (short) 50); i++)
        {
            result += job switch
            {
                Job.Beginner => i % 3 != 0 ? 1 : 0,
                Job.Knight => i % 2 == 1 ? 1 : 0,
                Job.Berserker => i % 2 == 0 ? 1 : 0,
                Job.Wizard => i % 3 == 2 ? 1 : 0,
                Job.Priest => i % 3 == 2 ? 1 : 0,
                Job.Archer => i % 2 == 1 ? 1 : 0,
                Job.HeavyGunner => 1,
                Job.Thief => 7,
                Job.Assassin => 7,
                Job.Runeblade => i % 2 == 1 ? 1 : 0,
                Job.Striker => i % 2 == 0 ? 1 : 0,
                Job.SoulBinder => i % 3 == 2 ? 1 : 0,
                _ => 0,
            };
        }
        for (int i = 50; i < level; i++)
        {
            result += job switch
            {
                Job.Beginner => 0,
                Job.Knight => 0,
                Job.Berserker => 0,
                Job.Wizard => 0,
                Job.Priest => 0,
                Job.Archer => 0,
                Job.HeavyGunner => i % 3 == 0 ? 1 : 0,
                Job.Thief => 1,
                Job.Assassin => 1,
                Job.Runeblade => 0,
                Job.Striker => 0,
                Job.SoulBinder => 0,
                _ => 0,
            };
        }

        return result;
    }

    public static long Health(Job job, short level)
    {
        double result = 50;
        int max = Math.Min((int) level, HP_SPLIT_LEVEL);
        for (int i = 0; i < max; i++)
        {
            result += job switch
            {
                Job.Beginner => 66,
                Job.Knight => 72,
                Job.Berserker => 85,
                Job.Wizard => 60,
                Job.Priest => 66,
                Job.Archer => 61,
                Job.HeavyGunner => 67.5,
                Job.Thief => 65,
                Job.Assassin => 60,
                Job.Runeblade => 69,
                Job.Striker => 69,
                Job.SoulBinder => 65,
                _ => 50,
            } * (Math.Atan(0.22 * i - 1.4) / Math.PI + 0.5);
        }

        for (int i = HP_SPLIT_LEVEL; i < level; i++)
        {
            // ReSharper disable once PossibleLossOfFraction
            result += 11.5 + (i - HP_SPLIT_LEVEL) / 10 * 0.5;
        }

        return (long) Math.Ceiling(result);
    }

    public static long Accuracy(Job job, short level) => 82;

    public static long Evasion(Job job, short level)
    {
        return job switch
        {
            Job.Beginner => 70,
            Job.Knight => 70,
            Job.Berserker => 72,
            Job.Wizard => 70,
            Job.Priest => 70,
            Job.Archer => 77,
            Job.HeavyGunner => 77,
            Job.Thief => 80,
            Job.Assassin => 77,
            Job.Runeblade => 77,
            Job.Striker => 76,
            Job.SoulBinder => 76,
            _ => 70,
        };
    }

    public static long CriticalRate(Job job, short level)
    {
        return job switch
        {
            Job.Beginner => 35,
            Job.Knight => 45,
            Job.Berserker => 47,
            Job.Wizard => 40,
            Job.Priest => 45,
            Job.Archer => 55,
            Job.HeavyGunner => 52,
            Job.Thief => 50,
            Job.Assassin => 53,
            Job.Runeblade => 46,
            Job.Striker => 48,
            Job.SoulBinder => 48,
            _ => 35,
        };
    }

    private const float Primary = 19.0f / 30.0f;
    private const float Secondary = 1.0f / 6.0f;

    public static long PhysicalAttack(Job job, long baseStr, long baseDex, long baseLuck)
    {
        (float strCoeff, float dexCoeff, float luckCoeff) = job switch
        {
            Job.Beginner => (0.0f, 0.0f, 0.0f),
            Job.Knight => (Primary, Secondary, 0.0f),
            Job.Berserker => (Primary, Secondary, 0.0f),
            Job.Wizard => (0.5666f, Secondary, 0.0f),
            Job.Priest => (0.4721f, Secondary, 0.0f),
            Job.Archer => (Secondary, Primary, 0.0f),
            Job.HeavyGunner => (0.0f, Primary, Secondary),
            Job.Thief => (Secondary, 0.0f, Primary),
            Job.Assassin => (0.0f, Secondary, Primary),
            Job.Runeblade => (Primary, Secondary, 0.0f),
            Job.Striker => (Secondary, Primary, 0.0f),
            Job.SoulBinder => (0.5666f, Secondary, 0.0f),
            _ => (0.0f, 0.0f, 0.0f)
        };

        return (long) (baseStr * strCoeff + baseDex * dexCoeff + baseLuck * luckCoeff);
    }

    public static long MagicAttack(Job job, long baseInt)
    {
        float intCoeff = job switch
        {
            Job.Beginner => 0.0f,
            Job.Knight => Primary,
            Job.Berserker => Primary,
            Job.Wizard => 0.5666f,
            Job.Priest => 0.4721f,
            Job.Archer => Primary,
            Job.HeavyGunner => Primary,
            Job.Thief => Primary,
            Job.Assassin => Primary,
            Job.Runeblade => 0.5666f,
            Job.Striker => Primary,
            Job.SoulBinder => 0.5666f,
            _ => 0.0f
        };

        return (long) (baseInt * intCoeff);
    }
}
