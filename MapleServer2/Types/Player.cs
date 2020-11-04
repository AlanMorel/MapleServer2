using System;
using System.Numerics;
using System.Collections.Generic;
using Maple2Storage.Types;
using MapleServer2.Types.FieldObjects;
using Maple2Storage.Enums;
using Maple2.Data.Types.Items;

namespace MapleServer2.Types {
    public class Player {
        // Bypass Key is constant PER ACCOUNT, unsure how it is validated
        // Seems like as long as it's valid, it doesn't matter though
        public readonly long UnknownId = 0x01EF80C2; //0x01CC3721;

        // Constant Values
        public long AccountId { get; private set; }
        public long CharacterId { get; private set; }
        public long CreationTime { get; private set; }
        public string Name { get; private set; }
        // Gender - 0 = male, 1 = female
        public byte Gender { get; private set; }
        // Job Group, according to jobgroupname.xml
        public int JobGroupId { get; private set; }
        public bool Awakened { get; private set; }
        public int JobId => JobGroupId * 10 + (Awakened ? 1 : 0);

        // Mutable Values
        public int MapId;
        public short Level = 1;
        public long Experience;
        public long RestExperience;
        public long Mesos;
        public int PrestigeLevel = 100;
        public long PrestigeExperience;
        public byte Animation;
        public PlayerStats Stats;
        public IFieldObject<Mount> Mount;

        // Combat, Adventure, Lifestyle
        public int[] Trophy = new int[3];

        public CoordF Coord;
        public CoordF UnknownCoord;

        // Appearance
        public SkinColor SkinColor;

        public string GuildName = "Abyss Tales";
        public string ProfileUrl = ""; // profile/e2/5a/2755104031905685000/637207943431921205.png
        public string Motto = "";
        public string HomeName = "";

        public Vector3 ReturnPosition;

        public long ValorToken;
        public long Treva;
        public long Rue;
        public long HaviFruit;
        public long MesoToken;

        public int MaxSkillTabs;
        public long ActiveSkillTabId;

        public Dictionary<EquipSlot, Item> Equip = new Dictionary<EquipSlot, Item>();
        public List<Item> Badges = new List<Item>();
        public EquipSlot[] EquipSlots { get; }
        private EquipSlot DefaultEquipSlot => EquipSlots.Length > 0 ? EquipSlots[0] : EquipSlot.NONE;
        public bool IsBeauty => DefaultEquipSlot == EquipSlot.HR
                        || DefaultEquipSlot == EquipSlot.FA
                        || DefaultEquipSlot == EquipSlot.FD
                        || DefaultEquipSlot == EquipSlot.CL
                        || DefaultEquipSlot == EquipSlot.PA
                        || DefaultEquipSlot == EquipSlot.SH
                        || DefaultEquipSlot == EquipSlot.ER;


        public JobType jobType;
        public JobCode jobCode => jobType != JobType.GameMaster ? (JobCode)((int)jobType / 10) : JobCode.GameMaster;

        public GameOptions GameOptions { get; private set; }

        public static Player Default(long accountId, long characterId, int mapId ,string name = "Default") {
            PlayerStats stats = PlayerStats.Default();
            stats.Hp = new PlayerStat(1000, 0, 1000);
            stats.CurrentHp = new PlayerStat(0, 1000, 0);
            stats.Spirit = new PlayerStat(100, 100, 100);
            stats.Stamina = new PlayerStat(120, 120, 120);
            stats.AtkSpd = new PlayerStat(120, 100, 130);
            stats.MoveSpd = new PlayerStat(110, 100, 150);
            stats.JumpHeight = new PlayerStat(110, 100, 130);

            return new Player
            {
                MapId = mapId,//2000062,
                AccountId = accountId,
                CharacterId = characterId,
                Level = 80,
                Name = name,
                Gender = 1,
                Motto = "Motto",
                HomeName = "HomeName",
                Coord = CoordF.From(2850, 2550, 1800),
                JobGroupId = 50,
                SkinColor = new SkinColor()
                {
                    Primary = Color.Argb(0xFF, 0xEA, 0xBF, 0xAE)
                },
                CreationTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                Equip = new Dictionary<EquipSlot, Item> { },
                Stats = stats,
                GameOptions = new GameOptions(),
                Mesos = 10,

            };
        }

        public static Player NewCharacter(long accountId, byte gender, JobType jobType, string name, SkinColor skinColor, object equips)
        {
            var player = new Player
            {
                AccountId = accountId,
                CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Name = name,
                Gender = gender,
                jobType = jobType,
                Level = 1,
                MapId = 2000062,
                Stats = new PlayerStats(),
                SkinColor = skinColor,
                Equip = (Dictionary<EquipSlot, Item>)equips,
        };
            return player;
        }
    }
}