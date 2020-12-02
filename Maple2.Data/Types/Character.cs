using System;
using System.Numerics;
using Maple2Storage.Enums;
using Maple2Storage.Types;

namespace Maple2.Data.Types {
    public class Character {
        public DateTime LastModified;

        /* Character */
        public long AccountId;
        public long Id;
        public string Name;

        public long CreationTime;
        public byte Gender; // Gender - 0 = male, 1 = female
        public JobType Job;
        public JobCode JobCode => Job != JobType.GameMaster ? (JobCode) ((int)Job / 10) : JobCode.GameMaster;
        public short Level;

        public long Experience;
        public long RestExperience;

        public int MapId;
        public SkinColor SkinColor;
        public AttributePoints AttributePoints;
        public int Title;
        public short Insignia;

        /* TODO: Not currently stored */
        public long GuildId = 0;
        public string DisplayPicture = ""; // profile/e2/5a/2755104031905685000/637207943431921205.png
        public string Motto = "";

        // Combat, Adventure, Lifestyle
        public int[] Trophy = new int[3];

        public string GuildName = "";
        public string GuildRank = "";
        public int GearScore;

        public int ReturnMapId;
        public Vector3 ReturnPosition;

        public long Mesos;
        public long ValorToken;
        public long Treva;
        public long Rue;
        public long HaviFruit;
        public long MesoToken;

        public int MaxSkillTabs;
        public long ActiveSkillTabId;

        public MasteryExp Mastery;

        public static Character NewCharacter(byte gender, JobType jobType, string name) {
            var player = new Character {
                CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Name = name,
                Gender = gender,
                Job = jobType,
                Level = 1,
                MapId = 2000062,
                AttributePoints = new AttributePoints(),
            };
            return player;
        }

        public static Character Default(long characterId, JobType jobType, string name) {
            Character character = NewCharacter(1, jobType, name);
            character.Id = characterId;
            character.Level = 80;
            character.Motto = "Motto";

            return character;
        }
    }
}