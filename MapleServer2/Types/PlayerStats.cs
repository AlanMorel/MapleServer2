using System.Runtime.InteropServices;

namespace MapleServer2.Types
{
    [StructLayout(LayoutKind.Sequential, Size = 432)]
    public struct PlayerStats
    {
        // Total 36 Stats MUST be in this order (Struct)
        public PlayerStat Str;
        public PlayerStat Dex;
        public PlayerStat Int;
        public PlayerStat Luk;
        public PlayerStat Hp;
        public PlayerStat CurrentHp;
        public PlayerStat HpRegen;
        public PlayerStat Unknown7;
        public PlayerStat Spirit;
        public PlayerStat Unknown9;
        public PlayerStat Unknown10;
        public PlayerStat Stamina;
        public PlayerStat Unknown12;
        public PlayerStat Unknown13;
        public PlayerStat AtkSpd;
        public PlayerStat MoveSpd;
        public PlayerStat Acc;
        public PlayerStat Eva;
        public PlayerStat CritRate;
        public PlayerStat CritDmg;
        public PlayerStat CritEva;
        public PlayerStat Def;
        public PlayerStat Guard;
        public PlayerStat JumpHeight;
        public PlayerStat PhysAtk;
        public PlayerStat MagAtk;
        public PlayerStat PhysRes;
        public PlayerStat MagRes;
        public PlayerStat MinAtk;
        public PlayerStat MaxAtk;
        public PlayerStat Unknown30;
        public PlayerStat Unknown31;
        public PlayerStat Pierce;
        public PlayerStat MountSpeed;
        public PlayerStat BonusAtk;
        public PlayerStat Unknown35;

        public static PlayerStats Default()
        {
            return new PlayerStats
            {
                Unknown7 = new PlayerStat(3000, 3000, 3000),
                Unknown9 = new PlayerStat(10, 10, 10),
                Unknown10 = new PlayerStat(500, 500, 500),
                Unknown12 = new PlayerStat(10, 10, 10),
                Unknown13 = new PlayerStat(500, 500, 500),
                Unknown30 = new PlayerStat(0, 0, 0),
                Unknown31 = new PlayerStat(0, 0, 0),
                MountSpeed = new PlayerStat(100, 100, 100),
                Unknown35 = new PlayerStat(0, 0, 0),
            };
        }
    }

    /* Total = Min + stat bonuses
     * Min = Base stat amount
     * Max = Cap on stat (Example: MoveSpd)
     *
     * For stuff like Spirit/Stamina, you decrease Max when it is consumed
     * For Hp, Min should always be 0. For CurrentHp, Total & Max should always be 0.
     * Decrease CurrentHp Min when you lose Hp.
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
            this.Total = total;
            this.Min = min;
            this.Max = max;
        }
    }
}