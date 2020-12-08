using System;
using System.Numerics;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using Maple2Storage.Types;
using MapleServer2.Enums;

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

        public string GuildName = "";
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
        public List<SkillTab> SkillTabs = new List<SkillTab>();

        public Dictionary<ItemSlot, Item> Equips = new Dictionary<ItemSlot, Item>();
        public List<Item> Badges = new List<Item>();
        public ItemSlot[] EquipSlots { get; }
        private ItemSlot DefaultEquipSlot => EquipSlots.Length > 0 ? EquipSlots[0] : ItemSlot.NONE;
        public bool IsBeauty => DefaultEquipSlot == ItemSlot.HR
                        || DefaultEquipSlot == ItemSlot.FA
                        || DefaultEquipSlot == ItemSlot.FD
                        || DefaultEquipSlot == ItemSlot.CL
                        || DefaultEquipSlot == ItemSlot.PA
                        || DefaultEquipSlot == ItemSlot.SH
                        || DefaultEquipSlot == ItemSlot.ER;

        public Job jobType;
        public JobCode jobCode => jobType != Job.GameMaster ? (JobCode)((int)jobType / 10) : JobCode.GameMaster;

        public GameOptions GameOptions { get; private set; }

        public static Player Default(long accountId, long characterId, string name = "SparkmodF") {
            PlayerStats stats = PlayerStats.Default();
            stats.Hp = new PlayerStat(1000, 0, 1000);
            stats.CurrentHp = new PlayerStat(0, 1000, 0);
            stats.Spirit = new PlayerStat(100, 100, 100);
            stats.Stamina = new PlayerStat(120, 120, 120);
            stats.AtkSpd = new PlayerStat(120, 100, 130);
            stats.MoveSpd = new PlayerStat(110, 100, 150);
            stats.JumpHeight = new PlayerStat(110, 100, 130);

            List<SkillTab> skillTabs = new List<SkillTab>();
            skillTabs.Add(new SkillTab("test1"));

            // Add runeblade skills set to level 1 and unlearned (normally would be loaded from database)
            XmlDocument xmlDoc= new XmlDocument();
            xmlDoc.Load("./MapleServer2/Constants/Skills/skills_10.xml");
            XmlNodeList skills = xmlDoc.SelectNodes("/ms2/key");
            IEnumerator ienum = skills.GetEnumerator();
            while (ienum.MoveNext()) {
                XmlNode currentNode = (XmlNode) ienum.Current;

                // Skill id
                int id = Int32.Parse((currentNode).Attributes["id"].Value);
                // Skill feature (awakening)
                XmlAttribute fAttr = (currentNode).Attributes["feature"];
                string feature = fAttr != null ? fAttr.Value : "";
                // Skill sub skills
                XmlAttribute subAttr = (currentNode).Attributes["sub"];
                int[] sub = subAttr != null ? Array.ConvertAll(subAttr.Value.Split(","), Int32.Parse) : null;

                skillTabs[0].AddOrUpdate(Skill.skill(id, 1, 0, feature, sub));
            }

            Player player = new Player {
                SkillTabs = skillTabs,
                MapId = 2000062,
                AccountId = accountId,
                CharacterId = characterId,
                Level = 70,
                Name = name,
                Gender = 1,
                Motto = "Motto",
                HomeName = "HomeName",
                Coord = CoordF.From(2850, 2550, 1800), //Little Harbor
                //Coord = CoordF.From(500, 500, 15000), // tria
                JobGroupId = 10,
                SkinColor = new SkinColor()
                {
                    Primary = Color.Argb(0xFF, 0xEA, 0xBF, 0xAE)
                },
                CreationTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                Equips = new Dictionary<ItemSlot, Item> {
                    { ItemSlot.ER, Item.Ear() },
                    { ItemSlot.HR, Item.Hair() },
                    { ItemSlot.FA, Item.Face() },
                    { ItemSlot.FD, Item.FaceDecoration() }
                },
                Stats = stats,
                GameOptions = new GameOptions(),
                Mesos = 10,

            };
            player.Equips.Add(ItemSlot.RH, Item.TutorialBow(player));
            return player;
        }

        public static Player MaleDefault(long accountId, long characterId, string name = "Sparkmod")
        {
            PlayerStats stats = PlayerStats.Default();
            stats.Hp = new PlayerStat(1000, 0, 1000);
            stats.CurrentHp = new PlayerStat(0, 1000, 0);
            stats.Spirit = new PlayerStat(100, 100, 100);
            stats.Stamina = new PlayerStat(120, 120, 120);
            stats.AtkSpd = new PlayerStat(120, 100, 130);
            stats.MoveSpd = new PlayerStat(110, 100, 150);
            stats.JumpHeight = new PlayerStat(110, 100, 130);

            List<SkillTab> skillTabs = new List<SkillTab>();
            skillTabs.Add(new SkillTab("test1"));

            // Add archer skills set to level 1 and unlearned (normally would be loaded from database)
            XmlDocument xmlDoc= new XmlDocument();
            xmlDoc.Load("./MapleServer2/Constants/Skills/skills_50.xml");
            XmlNodeList skills = xmlDoc.SelectNodes("/ms2/key");
            IEnumerator ienum = skills.GetEnumerator();
            while (ienum.MoveNext()) {
                XmlNode currentNode = (XmlNode) ienum.Current;

                // Skill id
                int id = Int32.Parse((currentNode).Attributes["id"].Value);
                // Skill feature (awakening)
                XmlAttribute fAttr = (currentNode).Attributes["feature"];
                string feature = fAttr != null ? fAttr.Value : "";
                // Skill sub skills
                XmlAttribute subAttr = (currentNode).Attributes["sub"];
                int[] sub = subAttr != null ? Array.ConvertAll(subAttr.Value.Split(","), Int32.Parse) : null;

                skillTabs[0].AddOrUpdate(Skill.skill(id, 1, 0, feature, sub));
            }

            return new Player {
                SkillTabs = skillTabs,
                MapId = 2000062,
                AccountId = accountId,
                CharacterId = characterId,
                Level = 70,
                Name = name,
                Gender = 0,
                Motto = "Motto",
                HomeName = "HomeName",
                Coord = CoordF.From(2850, 2550, 1800),
                JobGroupId = 50,
                SkinColor = new SkinColor()
                {
                    Primary = Color.Argb(0xFF, 0xEA, 0xBF, 0xAE)
                },
                CreationTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                Equips = new Dictionary<ItemSlot, Item> {
                    { ItemSlot.ER, Item.EarMale() },
                    { ItemSlot.HR, Item.HairMale() },
                    { ItemSlot.FA, Item.FaceMale() },
                    { ItemSlot.FD, Item.FaceDecorationMale() },
                    { ItemSlot.CL, Item.CloathMale() },
                    { ItemSlot.SH, Item.ShoesMale() },

                },
                Stats = stats,
                GameOptions = new GameOptions(),
                Mesos = 10,
            };
        }

        public static Player NewCharacter(byte gender, /*Job jobType*/ int job, string name, SkinColor skinColor, object equips) {
            PlayerStats stats = PlayerStats.Default();
            stats.Hp = new PlayerStat(1000, 0, 1000);
            stats.CurrentHp = new PlayerStat(0, 1000, 0);
            stats.Spirit = new PlayerStat(100, 100, 100);
            stats.Stamina = new PlayerStat(120, 120, 120);
            stats.AtkSpd = new PlayerStat(120, 100, 130);
            stats.MoveSpd = new PlayerStat(110, 100, 150);
            stats.JumpHeight = new PlayerStat(110, 100, 130);

            List<SkillTab> skillTabs = new List<SkillTab>();
            skillTabs.Add(new SkillTab("test1"));

            // Add job skills set to level 1 and unlearned
            XmlDocument xmlDoc= new XmlDocument();
            xmlDoc.Load($"./MapleServer2/Constants/Skills/skills_{job}.xml");
            XmlNodeList skills = xmlDoc.SelectNodes("/ms2/key");
            IEnumerator ienum = skills.GetEnumerator();
            while (ienum.MoveNext()) {
                XmlNode currentNode = (XmlNode) ienum.Current;

                // Skill id
                int id = Int32.Parse((currentNode).Attributes["id"].Value);
                // Skill feature (awakening)
                XmlAttribute fAttr = (currentNode).Attributes["feature"];
                string feature = fAttr != null ? fAttr.Value : "";
                // Skill sub skills
                XmlAttribute subAttr = (currentNode).Attributes["sub"];
                int[] sub = subAttr != null ? Array.ConvertAll(subAttr.Value.Split(","), Int32.Parse) : null;

                skillTabs[0].AddOrUpdate(Skill.skill(id, 1, 0, feature, sub));
            }

            return new Player {
                SkillTabs = skillTabs,
                AccountId = 0x1111111111111111,
                CharacterId = BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0),
                CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Name = name,
                Gender = gender,
                //jobType = jobType,
                JobGroupId = job,
                Level = 1,
                MapId = 2000062,
                Stats = stats,
                SkinColor = skinColor,
                Equips = (Dictionary<ItemSlot, Item>)equips,
                Motto = "Motto",
                HomeName = "HomeName",
                Coord = CoordF.From(2850, 2550, 1800),
                GameOptions = new GameOptions(),
                Mesos = 10,
            };
        }
    }
}