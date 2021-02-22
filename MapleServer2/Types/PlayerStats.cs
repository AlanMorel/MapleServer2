using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;

namespace MapleServer2.Types
{
    public enum PlayerStatId : byte
    {
        Str = 0x00,
        Dex = 0x01,
        Int = 0x02,
        Luk = 0x03,
        Hp = 0x04,           // long
        HpRegen = 0x05,
        Unknown7 = 0x06,     // base 3000
        Spirit = 0x07,
        SpRegen = 0x08,      // (10, 10, 10)
        Unknown10 = 0x09,    // base 500
        Unknown11 = 0x0A,    // base 120
        Stamina = 0x0B,      // (10, 10, 10)
        Unknown13 = 0x0C,    // base 500
        AtkSpd = 0x0D,
        MoveSpd = 0x0E,
        Acc = 0x0F,
        Eva = 0x10,
        CritRate = 0x11,
        CritDmg = 0x12,
        CritEva = 0x13,
        Def = 0x14,
        Guard = 0x15,
        JumpHeight = 0x16,
        PhysAtk = 0x17,
        MagAtk = 0x18,
        PhysRes = 0x19,
        MagRes = 0x1A,
        MinAtk = 0x1B,
        MaxAtk = 0x1C,
        Unknown30 = 0x1D,
        Unknown31 = 0x1E,
        Pierce = 0x1F,
        MountSpeed = 0x20,
        BonusAtk = 0x21,
        Unknown35 = 0x22,   // can be increased, base stays 0
    }

    /* Total = Base + stat allocations + bonuses.
     * Min = Base stat amount.
     * Max = Final value, capped. (Example: MoveSpd)
     *
     * For stuff like Spirit/Stamina, you decrease Max when it is consumed.
     * Decrease Hp.Total and Hp.Max when you lose Hp.
     */
    [StructLayout(LayoutKind.Sequential, Size = 12)]
    public struct PlayerStat
    {
        public int Total;
        public int Min;
        public int Max;

        public int this[int i]
        {
            get
            {
                return i switch
                {
                    1 => Min,
                    2 => Max,
                    _ => Total
                };
            }
        }

        public PlayerStat(int total, int min, int max)
        {
            Total = total;
            Min = min;
            Max = max;
        }

        public PlayerStat(PlayerStat copy, int increase)
        {
            Total = copy.Total + increase;
            Min = copy.Min + increase;
            Max = copy.Max + increase;
        }
    }

    public class PlayerStats
    {
        // TOOD: Handle stat allocation in here?
        public readonly OrderedDictionary Data;

        public PlayerStats()
        {
            Data = new OrderedDictionary
            {
                { PlayerStatId.Str, new PlayerStat(9, 9, 9) },
                { PlayerStatId.Dex, new PlayerStat(9, 9, 9) },
                { PlayerStatId.Int, new PlayerStat(9, 9, 9) },
                { PlayerStatId.Luk, new PlayerStat(9, 9, 9) },
                { PlayerStatId.Hp, new PlayerStat(500, 500, 500) },         // Max = 0 on login
                { PlayerStatId.HpRegen, new PlayerStat(10, 10, 10) },
                { PlayerStatId.Unknown7, new PlayerStat(3000, 3000, 3000) },
                { PlayerStatId.Spirit, new PlayerStat(100, 100, 100) },     // Max = 0 on login
                { PlayerStatId.SpRegen, new PlayerStat(1, 1, 1) },
                { PlayerStatId.Unknown10, new PlayerStat(200, 200, 200) },
                { PlayerStatId.Unknown11, new PlayerStat(120, 120, 120) },  // Max = 0 on login
                { PlayerStatId.Stamina, new PlayerStat(10, 10, 10) },       // (10, 10, 10)
                { PlayerStatId.Unknown13, new PlayerStat(500, 500, 500) },  // (500, 500, 500)
                { PlayerStatId.AtkSpd, new PlayerStat(100, 100, 100) },
                { PlayerStatId.MoveSpd, new PlayerStat(100, 100, 100) },
                { PlayerStatId.Acc, new PlayerStat(82, 82, 82) },
                { PlayerStatId.Eva, new PlayerStat(70, 70, 70) },           // changes with job
                { PlayerStatId.CritRate, new PlayerStat(45, 45, 45) },      // changes with job
                { PlayerStatId.CritDmg, new PlayerStat(250, 250, 250) },
                { PlayerStatId.CritEva, new PlayerStat(50, 50, 50) },
                { PlayerStatId.Def, new PlayerStat(16, 16, 16) },           // base affected by something?
                { PlayerStatId.Guard, new PlayerStat(0, 0, 0) },
                { PlayerStatId.JumpHeight, new PlayerStat(100, 100, 100) },
                { PlayerStatId.PhysAtk, new PlayerStat(10, 10, 10) },       // base for mage, 74 thief
                { PlayerStatId.MagAtk, new PlayerStat(2, 2, 2) },           // base for thief, 216 mage
                { PlayerStatId.PhysRes, new PlayerStat(5, 5, 5) },
                { PlayerStatId.MagRes, new PlayerStat(4, 4, 4) },
                { PlayerStatId.MinAtk, new PlayerStat(0, 0, 0) },
                { PlayerStatId.MaxAtk, new PlayerStat(0, 0, 0) },
                { PlayerStatId.Unknown30, new PlayerStat(0, 0, 0) },
                { PlayerStatId.Unknown31, new PlayerStat(0, 0, 0) },
                { PlayerStatId.Pierce, new PlayerStat(0, 0, 0) },
                { PlayerStatId.MountSpeed, new PlayerStat(100, 100, 100) },
                { PlayerStatId.BonusAtk, new PlayerStat(0, 0, 0) },
                { PlayerStatId.Unknown35, new PlayerStat(0, 0, 0) }
            };
        }

        public PlayerStat this[PlayerStatId statIndex]
        {
            get
            {
                return (PlayerStat) Data[statIndex];
            }

            set
            {
                Data[statIndex] = value;
            }
        }

        public void IncreaseBase(PlayerStatId statIndex, int amount)
        {
            PlayerStat stat = this[statIndex];
            Data[statIndex] = new PlayerStat(stat, amount);
        }

        public void Increase(PlayerStatId statIndex, int amount)
        {
            PlayerStat stat = this[statIndex];
            Data[statIndex] = new PlayerStat(stat.Total + amount, stat.Min, stat.Max + amount);
        }

        public void DecreaseBase(PlayerStatId statIndex, int amount)
        {
            PlayerStat stat = this[statIndex];
            Data[statIndex] = new PlayerStat(stat, -amount);
        }

        public void Decrease(PlayerStatId statIndex, int amount)
        {
            PlayerStat stat = this[statIndex];
            Data[statIndex] = new PlayerStat(stat.Total - amount, stat.Min, stat.Max - amount);
        }

        public void ResetAllocations(StatDistribution statDist)
        {
            foreach (KeyValuePair<byte, int> entry in statDist.AllocatedStats)
            {
                Decrease((PlayerStatId) entry.Key, entry.Value);
            }
            statDist.ResetPoints();
        }
    }
}
