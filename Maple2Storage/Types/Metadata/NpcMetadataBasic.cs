using System;
using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class NpcMetadataBasic
    {
        [XmlElement]
        public sbyte NpcAttackGroup;
        [XmlElement]
        public sbyte NpcDefenseGroup;
        // [XmlElement]
        // public byte Kind;  // 0, or 2 (Warehouse)
        [XmlElement]
        public bool AttackDamage;  // Enabled or disabled
        [XmlElement]
        public bool HitImmune;
        [XmlElement]
        public bool AbnormalImmune;
        [XmlElement]
        public byte Class;  // Most things are 0 or 1. normalman4 (11000150) is "5"
        [XmlElement]
        public ushort Kind;
        [XmlElement]
        public byte HpBar;
        [XmlElement]
        public bool RotationDisabled;
        [XmlElement]
        public bool CarePathToEnemy;  // 0 walks around objects, 1 is probably straight line.
        // Do we care about sound? Does the server need to send sound packets to the client?
        [XmlElement]
        public byte MaxSpawnCount;  // The maximum number of this NPC that can be spawned on a given map. (0 to 6)
        [XmlElement]
        public byte GroupSpawnCount;  // When spawning this NPC, how many spawn at once? (0 to 5)
        [XmlElement]
        public byte RareDegree;  // 0, 50, or 100
        [XmlElement]
        public ushort Difficulty;  // 0-9
        [XmlElement]
        public string[] PropertyTags = Array.Empty<string>();  // "", "ai_wander_freely ", "keep_battle"
                                                               // "keep_battle,fixed_big_monster", etc
        /*
         * [XmlElement]
         * public bool BossNotify = false;  // "", field_elite, field_boss, dungeon_boss, dungeon_elite
         * [XmlElement]
         * public uint BossSoundDistance;  // 0 to 99999
         */
        [XmlElement]
        // Goes hand in hand (usually) with bossNotify
        public string[] EventTags = Array.Empty<string>();  // field_elite, 
        [XmlElement]
        public string Race;  // (plane|animal|"|spirit|fairy|combine|bug|devil)
        /*
         * [XmlElement]
         * public float DamagedVibrateDuration;  // Scale from 0 to 1
         * [XmlElement]
         * public short DamagedVibrateAmp;  // 0, 1, 5, 9
         */

        public NpcMetadataBasic()
        {

        }
    }

}
