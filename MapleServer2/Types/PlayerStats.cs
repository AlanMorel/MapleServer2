using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;

namespace MapleServer2.Types
{
    // Player Stats in Packet Order - Count: 35 (0x23)
    public enum PlayerStatId : byte
    {
        Str = 0x00,
        Dex = 0x01,
        Int = 0x02,
        Luk = 0x03,
        Hp = 0x04,              // long
        HpRegen = 0x05,
        HpRegenTime = 0x06,     // base (3000ms)
        Spirit = 0x07,
        SpRegen = 0x08,
        SpRegenTime = 0x09,     // base (200ms)
        Stamina = 0x0A,         // base 120 (20 = 1 block)
        StaRegen = 0x0B,        // base 10  (10 = 0.5 block)
        StaRegenTime = 0x0C,    // base (500ms)
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
        Unknown35 = 0x22,   // base 0 (bonuses can be added)
    }

    /* Max = Base + stat allocations + bonuses.
     * Min = Base stat amount.
     * Current = Final value (e.g. capped Damage, current Hp, active CritRate, ...)
     *
     * Change PlayerStat.Current for temporary changes.
     */
    [StructLayout(LayoutKind.Sequential, Size = 12)]
    public struct PlayerStat
    {
        public int Max;
        public int Min;
        public int Current;

        public int this[int i]
        {
            get
            {
                return i switch
                {
                    1 => Min,
                    2 => Current,
                    _ => Max
                };
            }
        }

        public PlayerStat(int initial)
        {
            Max = initial;
            Min = initial;
            Current = initial;
        }

        public PlayerStat(int total, int min, int max)
        {
            Max = total;
            Min = min;
            Current = max;
        }

        public PlayerStat(PlayerStat sourceStat, int offset)
        {
            Max = sourceStat.Max + offset;
            Min = sourceStat.Min + offset;
            Current = sourceStat.Current + offset;
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
                { PlayerStatId.Str, new PlayerStat(9) },
                { PlayerStatId.Dex, new PlayerStat(9) },
                { PlayerStatId.Int, new PlayerStat(9) },
                { PlayerStatId.Luk, new PlayerStat(9) },
                { PlayerStatId.Hp, new PlayerStat(500, 500, 0) },         // Max = 0 on login
                { PlayerStatId.HpRegen, new PlayerStat(10) },
                { PlayerStatId.HpRegenTime, new PlayerStat(3000) },
                { PlayerStatId.Spirit, new PlayerStat(100, 100, 0) },     // Max = 0 on login
                { PlayerStatId.SpRegen, new PlayerStat(1) },
                { PlayerStatId.SpRegenTime, new PlayerStat(200) },
                { PlayerStatId.Stamina, new PlayerStat(120) },  // Max = 0 on login
                { PlayerStatId.StaRegen, new PlayerStat(10) },
                { PlayerStatId.StaRegenTime, new PlayerStat(500) },
                { PlayerStatId.AtkSpd, new PlayerStat(100) },
                { PlayerStatId.MoveSpd, new PlayerStat(100) },
                { PlayerStatId.Acc, new PlayerStat(82) },
                { PlayerStatId.Eva, new PlayerStat(70) },           // changes with job
                { PlayerStatId.CritRate, new PlayerStat(45) },      // changes with job
                { PlayerStatId.CritDmg, new PlayerStat(250) },
                { PlayerStatId.CritEva, new PlayerStat(50) },
                { PlayerStatId.Def, new PlayerStat(16) },           // base affected by something?
                { PlayerStatId.Guard, new PlayerStat(0) },
                { PlayerStatId.JumpHeight, new PlayerStat(100) },
                { PlayerStatId.PhysAtk, new PlayerStat(10) },       // base for mage, 74 thief
                { PlayerStatId.MagAtk, new PlayerStat(2) },           // base for thief, 216 mage
                { PlayerStatId.PhysRes, new PlayerStat(5) },
                { PlayerStatId.MagRes, new PlayerStat(4) },
                { PlayerStatId.MinAtk, new PlayerStat(0) },
                { PlayerStatId.MaxAtk, new PlayerStat(0) },
                { PlayerStatId.Unknown30, new PlayerStat(0) },
                { PlayerStatId.Unknown31, new PlayerStat(0) },
                { PlayerStatId.Pierce, new PlayerStat(0) },
                { PlayerStatId.MountSpeed, new PlayerStat(100) },
                { PlayerStatId.BonusAtk, new PlayerStat(0) },
                { PlayerStatId.Unknown35, new PlayerStat(0) }
            };
        }

        public PlayerStats(int strBase, int dexBase, int intBase, int lukBase, int hpBase, int critRateBase)
        {
            Data = new OrderedDictionary
            {
                { PlayerStatId.Str, new PlayerStat(strBase) },
                { PlayerStatId.Dex, new PlayerStat(dexBase) },
                { PlayerStatId.Int, new PlayerStat(intBase) },
                { PlayerStatId.Luk, new PlayerStat(lukBase) },
                { PlayerStatId.Hp, new PlayerStat(hpBase, hpBase, 0) },     // Max = 0 on login
                { PlayerStatId.HpRegen, new PlayerStat(10) },
                { PlayerStatId.HpRegenTime, new PlayerStat(3000) },
                { PlayerStatId.Spirit, new PlayerStat(100, 100, 0) },       // Max = 0 on login
                { PlayerStatId.SpRegen, new PlayerStat(1) },
                { PlayerStatId.SpRegenTime, new PlayerStat(200) },
                { PlayerStatId.Stamina, new PlayerStat(120) },              // Max = 0 on login
                { PlayerStatId.StaRegen, new PlayerStat(10) },
                { PlayerStatId.StaRegenTime, new PlayerStat(500) },
                { PlayerStatId.AtkSpd, new PlayerStat(100) },
                { PlayerStatId.MoveSpd, new PlayerStat(100) },
                { PlayerStatId.Acc, new PlayerStat(82) },
                { PlayerStatId.Eva, new PlayerStat(70) },                   // changes with job
                { PlayerStatId.CritRate, new PlayerStat(critRateBase) },    // changes with job
                { PlayerStatId.CritDmg, new PlayerStat(250) },
                { PlayerStatId.CritEva, new PlayerStat(50) },
                { PlayerStatId.Def, new PlayerStat(16) },                   // base affected by something?
                { PlayerStatId.Guard, new PlayerStat(0) },
                { PlayerStatId.JumpHeight, new PlayerStat(100) },
                { PlayerStatId.PhysAtk, new PlayerStat(10) },               // base for mage, 74 thief
                { PlayerStatId.MagAtk, new PlayerStat(2) },                 // base for thief, 216 mage
                { PlayerStatId.PhysRes, new PlayerStat(5) },
                { PlayerStatId.MagRes, new PlayerStat(4) },
                { PlayerStatId.MinAtk, new PlayerStat(0) },
                { PlayerStatId.MaxAtk, new PlayerStat(0) },
                { PlayerStatId.Unknown30, new PlayerStat(0) },
                { PlayerStatId.Unknown31, new PlayerStat(0) },
                { PlayerStatId.Pierce, new PlayerStat(0) },
                { PlayerStatId.MountSpeed, new PlayerStat(100) },
                { PlayerStatId.BonusAtk, new PlayerStat(0) },
                { PlayerStatId.Unknown35, new PlayerStat(0) }
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

        public void InitializePools(int hp, int spirit, int stamina)
        {
            PlayerStat hpStat = this[PlayerStatId.Hp];
            PlayerStat spiritStat = this[PlayerStatId.Spirit];
            PlayerStat staStat = this[PlayerStatId.Stamina];
            Data[PlayerStatId.Hp] = new PlayerStat(hpStat.Max, hpStat.Min, hp);
            Data[PlayerStatId.Spirit] = new PlayerStat(spiritStat.Max, spiritStat.Min, spirit);
            Data[PlayerStatId.Stamina] = new PlayerStat(staStat.Max, staStat.Min, stamina);
        }

        public void IncreaseMax(PlayerStatId statIndex, int amount)
        {
            PlayerStat stat = this[statIndex];
            Data[statIndex] = new PlayerStat(stat.Max + amount, stat.Min, stat.Current + amount);
        }

        public void IncreaseBase(PlayerStatId statIndex, int amount)
        {
            PlayerStat stat = this[statIndex];
            Data[statIndex] = new PlayerStat(stat.Max + amount, stat.Min + amount, stat.Current + amount);
        }

        public void Increase(PlayerStatId statIndex, int amount)
        {
            PlayerStat stat = this[statIndex];
            Data[statIndex] = new PlayerStat(stat.Max, stat.Min, stat.Current + amount);
        }

        public void DecreaseMax(PlayerStatId statIndex, int amount)
        {
            PlayerStat stat = this[statIndex];
            int newMax = Math.Clamp(stat.Max - amount, stat.Min, stat.Max);
            int newCurrent = stat.Current - amount;
            if (statIndex == PlayerStatId.Hp || statIndex == PlayerStatId.Spirit)
            {
                newCurrent = Math.Clamp(stat.Current, stat.Min, newMax);
            }
            Data[statIndex] = new PlayerStat(newMax, stat.Min, newCurrent);
        }

        public void DecreaseBase(PlayerStatId statIndex, int amount)
        {
            // Clamp Min to 0?
            PlayerStat stat = this[statIndex];
            Data[statIndex] = new PlayerStat(stat.Max - amount, stat.Min - amount, stat.Current - amount);
        }

        public void Decrease(PlayerStatId statIndex, int amount)
        {
            PlayerStat stat = this[statIndex];
            Data[statIndex] = new PlayerStat(stat.Max, stat.Min, stat.Current - amount);
        }

        public void ResetAllocations(StatDistribution statDist)
        {
            foreach (KeyValuePair<byte, int> entry in statDist.AllocatedStats)
            {
                DecreaseMax((PlayerStatId) entry.Key, entry.Value);
            }
            statDist.ResetPoints();
        }
    }
}
