using MapleServer2.Enums;

namespace MapleServer2.Types;

public class BaseStats
{
    private const int HP_SPLIT_LEVEL = 50;
    private const double RES_DIVISOR = 99.0;

    public static long Strength(JobCode jobCode, short level)
    {
        long result = jobCode switch
        {
            JobCode.Beginner => 7,
            JobCode.Knight => 8,
            JobCode.Berserker => 8,
            JobCode.Wizard => 1,
            JobCode.Priest => 1,
            JobCode.Archer => 6,
            JobCode.HeavyGunner => 2,
            JobCode.Thief => 6,
            JobCode.Assassin => 2,
            JobCode.Runeblade => 8,
            JobCode.Striker => 6,
            JobCode.SoulBinder => 1,
            _ => 1,
        };
        for (int i = 1; i < Math.Min(level, (short) 50); i++)
        {
            result += jobCode switch
            {
                JobCode.Beginner => 7,
                JobCode.Knight => 7,
                JobCode.Berserker => 7,
                JobCode.Wizard => 0,
                JobCode.Priest => 0,
                JobCode.Archer => 1,
                JobCode.HeavyGunner => 0,
                JobCode.Thief => 1,
                JobCode.Assassin => 0,
                JobCode.Runeblade => 7,
                JobCode.Striker => 1,
                JobCode.SoulBinder => 0,
                _ => 0,
            };
        }
        for (int i = 50; i < level; i++)
        {
            result += jobCode switch
            {
                JobCode.Beginner => 1,
                JobCode.Knight => 1,
                JobCode.Berserker => 1,
                JobCode.Wizard => 0,
                JobCode.Priest => 0,
                JobCode.Archer => i % 3 == 0 ? 1 : 0,
                JobCode.HeavyGunner => 0,
                JobCode.Thief => i % 3 == 0 ? 1 : 0,
                JobCode.Assassin => 0,
                JobCode.Runeblade => 1,
                JobCode.Striker => i % 3 == 0 ? 1 : 0,
                JobCode.SoulBinder => 0,
                _ => 0,
            };
        }

        return result;
    }

    public static long Dexterity(JobCode jobCode, short level)
    {
        long result = jobCode switch
        {
            JobCode.Beginner => 6,
            JobCode.Knight => 6,
            JobCode.Berserker => 6,
            JobCode.Wizard => 1,
            JobCode.Priest => 1,
            JobCode.Archer => 8,
            JobCode.HeavyGunner => 8,
            JobCode.Thief => 2,
            JobCode.Assassin => 6,
            JobCode.Runeblade => 6,
            JobCode.Striker => 8,
            JobCode.SoulBinder => 1,
            _ => 1,
        };
        for (int i = 1; i < Math.Min(level, (short) 50); i++)
        {
            result += jobCode switch
            {
                JobCode.Beginner => 0,
                JobCode.Knight => 1,
                JobCode.Berserker => 1,
                JobCode.Wizard => 0,
                JobCode.Priest => 0,
                JobCode.Archer => 7,
                JobCode.HeavyGunner => 7,
                JobCode.Thief => 0,
                JobCode.Assassin => 1,
                JobCode.Runeblade => 1,
                JobCode.Striker => 7,
                JobCode.SoulBinder => 0,
                _ => 0,
            };
        }
        for (int i = 50; i < level; i++)
        {
            result += jobCode switch
            {
                JobCode.Beginner => 0,
                JobCode.Knight => i % 3 == 0 ? 1 : 0,
                JobCode.Berserker => i % 3 == 0 ? 1 : 0,
                JobCode.Wizard => 0,
                JobCode.Priest => 0,
                JobCode.Archer => 1,
                JobCode.HeavyGunner => 1,
                JobCode.Thief => 0,
                JobCode.Assassin => i % 3 == 0 ? 1 : 0,
                JobCode.Runeblade => i % 3 == 0 ? 1 : 0,
                JobCode.Striker => 1,
                JobCode.SoulBinder => 0,
                _ => 0,
            };
        }

        return result;
    }

    public static long Intelligence(JobCode jobCode, short level)
    {
        long result = jobCode switch
        {
            JobCode.Beginner => 2,
            JobCode.Knight => 2,
            JobCode.Berserker => 1,
            JobCode.Wizard => 14,
            JobCode.Priest => 14,
            JobCode.Archer => 1,
            JobCode.HeavyGunner => 1,
            JobCode.Thief => 1,
            JobCode.Assassin => 1,
            JobCode.Runeblade => 2,
            JobCode.Striker => 1,
            JobCode.SoulBinder => 14,
            _ => 1,
        };
        for (int i = 1; i < Math.Min(level, (short) 50); i++)
        {
            result += jobCode switch
            {
                JobCode.Beginner => i % 3 != 1 ? 1 : 0,
                JobCode.Knight => i % 2 == 0 ? 1 : 0,
                JobCode.Berserker => i % 2 == 1 ? 1 : 0,
                JobCode.Wizard => 8,
                JobCode.Priest => 8,
                JobCode.Archer => i % 2 == 0 ? 1 : 0,
                JobCode.HeavyGunner => i % 2 == 0 ? 1 : 0,
                JobCode.Thief => i % 2 == 0 ? 1 : 0,
                JobCode.Assassin => i % 2 == 0 ? 1 : 0,
                JobCode.Runeblade => i % 2 == 0 ? 1 : 0,
                JobCode.Striker => i % 2 == 1 ? 1 : 0,
                JobCode.SoulBinder => 8,
                _ => 0,
            };
        }
        for (int i = 50; i < level; i++)
        {
            result += jobCode switch
            {
                JobCode.Beginner => 0,
                JobCode.Knight => 0,
                JobCode.Berserker => 0,
                JobCode.Wizard => 1,
                JobCode.Priest => 1,
                JobCode.Archer => 0,
                JobCode.HeavyGunner => 0,
                JobCode.Thief => 0,
                JobCode.Assassin => 0,
                JobCode.Runeblade => 0,
                JobCode.Striker => 0,
                JobCode.SoulBinder => 1,
                _ => 0,
            };
        }

        return result;
    }

    public static long Luck(JobCode jobCode, short level)
    {
        long result = jobCode switch
        {
            JobCode.Beginner => 2,
            JobCode.Knight => 1,
            JobCode.Berserker => 2,
            JobCode.Wizard => 1,
            JobCode.Priest => 1,
            JobCode.Archer => 2,
            JobCode.HeavyGunner => 6,
            JobCode.Thief => 8,
            JobCode.Assassin => 8,
            JobCode.Runeblade => 1,
            JobCode.Striker => 2,
            JobCode.SoulBinder => 1,
            _ => 1,
        };
        for (int i = 1; i < Math.Min(level, (short) 50); i++)
        {
            result += jobCode switch
            {
                JobCode.Beginner => i % 3 != 0 ? 1 : 0,
                JobCode.Knight => i % 2 == 1 ? 1 : 0,
                JobCode.Berserker => i % 2 == 0 ? 1 : 0,
                JobCode.Wizard => i % 3 == 2 ? 1 : 0,
                JobCode.Priest => i % 3 == 2 ? 1 : 0,
                JobCode.Archer => i % 2 == 1 ? 1 : 0,
                JobCode.HeavyGunner => 1,
                JobCode.Thief => 7,
                JobCode.Assassin => 7,
                JobCode.Runeblade => i % 2 == 1 ? 1 : 0,
                JobCode.Striker => i % 2 == 0 ? 1 : 0,
                JobCode.SoulBinder => i % 3 == 2 ? 1 : 0,
                _ => 0,
            };
        }
        for (int i = 50; i < level; i++)
        {
            result += jobCode switch
            {
                JobCode.Beginner => 0,
                JobCode.Knight => 0,
                JobCode.Berserker => 0,
                JobCode.Wizard => 0,
                JobCode.Priest => 0,
                JobCode.Archer => 0,
                JobCode.HeavyGunner => i % 3 == 0 ? 1 : 0,
                JobCode.Thief => 1,
                JobCode.Assassin => 1,
                JobCode.Runeblade => 0,
                JobCode.Striker => 0,
                JobCode.SoulBinder => 0,
                _ => 0,
            };
        }

        return result;
    }

    public static long Health(JobCode jobCode, short level)
    {
        double result = 50;
        int max = Math.Min((int) level, HP_SPLIT_LEVEL);
        for (int i = 0; i < max; i++)
        {
            result += jobCode switch
            {
                JobCode.Beginner => 66,
                JobCode.Knight => 72,
                JobCode.Berserker => 85,
                JobCode.Wizard => 60,
                JobCode.Priest => 66,
                JobCode.Archer => 61,
                JobCode.HeavyGunner => 67.5,
                JobCode.Thief => 65,
                JobCode.Assassin => 60,
                JobCode.Runeblade => 69,
                JobCode.Striker => 69,
                JobCode.SoulBinder => 65,
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

    public static long Accuracy(JobCode jobCode, short level) => 82;

    public static long Evasion(JobCode jobCode, short level)
    {
        return jobCode switch
        {
            JobCode.Beginner => 70,
            JobCode.Knight => 70,
            JobCode.Berserker => 72,
            JobCode.Wizard => 70,
            JobCode.Priest => 70,
            JobCode.Archer => 77,
            JobCode.HeavyGunner => 77,
            JobCode.Thief => 80,
            JobCode.Assassin => 77,
            JobCode.Runeblade => 77,
            JobCode.Striker => 76,
            JobCode.SoulBinder => 76,
            _ => 70,
        };
    }

    public static long CriticalRate(JobCode jobCode, short level)
    {
        return jobCode switch
        {
            JobCode.Beginner => 35,
            JobCode.Knight => 45,
            JobCode.Berserker => 47,
            JobCode.Wizard => 40,
            JobCode.Priest => 45,
            JobCode.Archer => 55,
            JobCode.HeavyGunner => 52,
            JobCode.Thief => 50,
            JobCode.Assassin => 53,
            JobCode.Runeblade => 46,
            JobCode.Striker => 48,
            JobCode.SoulBinder => 48,
            _ => 35,
        };
    }

    private const float Primary = 19.0f / 30.0f;
    private const float Secondary = 1.0f / 6.0f;

    public static long PhysicalAttack(JobCode jobCode, long baseStr, long baseDex, long baseLuck)
    {
        (float strCoeff, float dexCoeff, float luckCoeff) = jobCode switch
        {
            JobCode.Beginner => (0.0f, 0.0f, 0.0f),
            JobCode.Knight => (Primary, Secondary, 0.0f),
            JobCode.Berserker => (Primary, Secondary, 0.0f),
            JobCode.Wizard => (0.5666f, Secondary, 0.0f),
            JobCode.Priest => (0.4721f, Secondary, 0.0f),
            JobCode.Archer => (Secondary, Primary, 0.0f),
            JobCode.HeavyGunner => (0.0f, Primary, Secondary),
            JobCode.Thief => (Secondary, 0.0f, Primary),
            JobCode.Assassin => (0.0f, Secondary, Primary),
            JobCode.Runeblade => (Primary, Secondary, 0.0f),
            JobCode.Striker => (Secondary, Primary, 0.0f),
            JobCode.SoulBinder => (0.5666f, Secondary, 0.0f),
            _ => (0.0f, 0.0f, 0.0f)
        };

        return (long) (baseStr * strCoeff + baseDex * dexCoeff + baseLuck * luckCoeff);
    }

    public static long MagicAttack(JobCode jobCode, long baseInt)
    {
        float intCoeff = jobCode switch
        {
            JobCode.Beginner => 0.0f,
            JobCode.Knight => Primary,
            JobCode.Berserker => Primary,
            JobCode.Wizard => 0.5666f,
            JobCode.Priest => 0.4721f,
            JobCode.Archer => Primary,
            JobCode.HeavyGunner => Primary,
            JobCode.Thief => Primary,
            JobCode.Assassin => Primary,
            JobCode.Runeblade => 0.5666f,
            JobCode.Striker => Primary,
            JobCode.SoulBinder => 0.5666f,
            _ => 0.0f
        };

        return (long) (baseInt * intCoeff);
    }
}
