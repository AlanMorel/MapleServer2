﻿using Maple2Storage.Types;

namespace MapleServer2.Types
{
    public class Mob
    {
        public readonly int Id;
        public short Rotation; // In degrees * 10
        public CoordS Speed;
        public byte Animation;
        public PlayerStats Stats;

        public Mob(int id)
        {
            Id = id;
            Animation = 255;
            Stats = new PlayerStats()
            {
                Hp = new PlayerStat(10, 0, 10),
                CurrentHp = new PlayerStat(0, 10, 0),
            };
        }
    }
}
