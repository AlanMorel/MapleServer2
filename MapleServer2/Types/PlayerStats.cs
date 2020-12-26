using System.Runtime.InteropServices;

namespace MapleServer2.Types {
    [StructLayout(LayoutKind.Sequential, Size = 432)]
    public struct PlayerStats {
        // Total 36 Stats MUST be in this order (Struct)
        public PlayerStat Str;
        public PlayerStat Dex;
        public PlayerStat Int;
        public PlayerStat Luk;
        public PlayerStat Hp;
        public PlayerStat CurrentHp;
        public PlayerStat HpRegen;
        public PlayerStat Unknown7;     // (3000, 3000, 3000)
        public PlayerStat Spirit;
        public PlayerStat Unknown9;     // (10, 10, 10)
        public PlayerStat Unknown10;    // (500, 500, 500)
        public PlayerStat Stamina;      // (10, 10, 10)
        public PlayerStat Unknown12;    // (500, 500, 500)
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

        public static PlayerStats Default() {
            return new PlayerStats
            {
                Hp = new PlayerStat(1000, 0, 1000),
                CurrentHp = new PlayerStat(0, 500, 0),
                Spirit = new PlayerStat(100, 100, 100),
                Stamina = new PlayerStat(120, 120, 120),
                AtkSpd = new PlayerStat(120, 100, 130),
                MoveSpd = new PlayerStat(110, 100, 150),
                JumpHeight = new PlayerStat(110, 100, 130),
                MountSpeed = new PlayerStat(100, 100, 100),

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
    public struct PlayerStat {
        public int Total;
        public int Min;
        public int Max;

        public int this[int i] {
            get {
                return i switch {
                    1 => Min,
                    2 => Max,
                    _ => Total
                };
            }
        }

        public PlayerStat(int total, int min, int max) {
            Total = total;
            Min = min;
            Max = max;
        }
    }
}