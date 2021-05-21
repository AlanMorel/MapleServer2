﻿using System;
using System.Xml;
using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class NpcMetadata
    {
        [XmlElement(Order = 1)]
        public int Id;
        [XmlElement(Order = 2)]
        public string Name;
        [XmlElement(Order = 3)]
        public string Model = string.Empty;
        [XmlElement(Order = 4)]
        public int TemplateId;
        [XmlElement(Order = 5)]
        public byte Friendly;
        [XmlElement(Order = 6)]
        public byte Level;
        [XmlElement(Order = 7)]
        public int[] SkillIds = Array.Empty<int>();
        [XmlElement(Order = 8)]
        public string AiInfo = string.Empty;  // This should be a deep structure, parsing the values in path to the XML referenced here.
        [XmlElement(Order = 9)]
        public int Experience;  // -1, 0, or some other number 6287481 (max)
        [XmlElement(Order = 10)]
        public int[] GlobalDropBoxIds = Array.Empty<int>();
        [XmlElement(Order = 11)]
        public CoordS Rotation; // In degrees * 10
        [XmlElement(Order = 12)]
        public CoordS Speed;  // Packet/Callers uses XYZ?
        [XmlElement(Order = 13)]
        public CoordS Coord;
        [XmlElement(Order = 14)]
        public byte Animation;
        [XmlElement(Order = 15)]
        public NpcMetadataBasic NpcMetadataBasic = new NpcMetadataBasic();
        [XmlElement(Order = 16)]
        public NpcMetadataCombat NpcMetadataCombat = new NpcMetadataCombat();
        [XmlElement(Order = 17)]
        public NpcMetadataDead NpcMetadataDead = new NpcMetadataDead();
        [XmlElement(Order = 18)]
        public NpcMetadataDistance NpcMetadataDistance = new NpcMetadataDistance();  // combat related
        [XmlElement(Order = 19)]
        public NpcMetadataInteract NpcMetadataInteract = new NpcMetadataInteract();
        [XmlElement(Order = 20)]
        public NpcStats Stats;
        [XmlElement(Order = 21)]
        public short Kind; // 13 = Shop
        [XmlElement(Order = 22)]
        public int ShopId;

        public NpcMetadata() { }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Coord, Rotation);
        }

        public override string ToString() =>
            $"Npc:(Id:{Id},Position:{Coord},Model:{Model},Friendly:{Friendly},IsShop:{Kind == 13},ShopId:{ShopId})";
    }

    [XmlType]
    public class NpcMetadataBasic
    {
        [XmlElement(Order = 1)]
        public sbyte NpcAttackGroup;
        [XmlElement(Order = 2)]
        public sbyte NpcDefenseGroup;
        [XmlElement(Order = 3)]
        public bool AttackDamage;  // Enabled or disabled
        [XmlElement(Order = 4)]
        public bool HitImmune;
        [XmlElement(Order = 5)]
        public bool AbnormalImmune;
        [XmlElement(Order = 6)]
        public byte Class;  // Most things are 0 or 1. normalman4 (11000150) is "5"
        [XmlElement(Order = 7)]
        public ushort Kind;
        [XmlElement(Order = 8)]
        public byte HpBar;
        [XmlElement(Order = 9)]
        public bool RotationDisabled;
        [XmlElement(Order = 10)]
        public bool CarePathToEnemy;  // 0 walks around objects, 1 is probably straight line.
        [XmlElement(Order = 11)]
        public byte MaxSpawnCount;  // The maximum number of this NPC that can be spawned on a given map. (0 to 6)
        [XmlElement(Order = 12)]
        public byte GroupSpawnCount;  // When spawning this NPC, how many spawn at once? (0 to 5)
        [XmlElement(Order = 13)]
        public byte RareDegree;  // 0, 50, or 100
        [XmlElement(Order = 14)]
        public ushort Difficulty;  // 0-9
        [XmlElement(Order = 15)]
        public string[] PropertyTags = Array.Empty<string>();
        [XmlElement(Order = 16)]
        public string[] MainTags = Array.Empty<string>();
        [XmlElement(Order = 17)]
        public string[] SubTags = Array.Empty<string>();
        [XmlElement(Order = 18)]
        public string[] EventTags = Array.Empty<string>();  // field_elite, 
        [XmlElement(Order = 19)]
        public string Race;  // (plane|animal|"|spirit|fairy|combine|bug|devil)

        public NpcMetadataBasic() { }

        public override string ToString() =>
            $"NpcMetadataBasic:(NpcAttackGroup:{NpcAttackGroup},NpcDefenseGroup:{NpcDefenseGroup},AttackDamage:{AttackDamage},Difficulty:{Difficulty},MaxSpawnCount:{MaxSpawnCount},GroupSpawnCount:{GroupSpawnCount})";
    }

    [XmlType]
    public class NpcMetadataCombat
    {
        [XmlElement]
        public uint CombatAbandonTick;  // 0, or 999999
        [XmlElement]
        public uint CombatAbandonImpossibleTick;  // 0, or 999999
        [XmlElement]
        public bool CanIgnoreExtendedLifetime;  // "true" or "false" in xml. Coerced to bool here.
        [XmlElement]
        public bool CanShowHiddenTarget;  // "true" or "false' in xml. Coerced to bool here.

        public NpcMetadataCombat() { }
    }

    [XmlType]
    public class NpcMetadataDead
    {
        [XmlElement]
        public float Time;
        [XmlElement]
        public string[] Actions = Array.Empty<string>();

        public NpcMetadataDead() { }
    }

    [XmlType]
    public class NpcMetadataDistance
    {
        [XmlElement]
        public short Avoid;
        [XmlElement]
        public short Sight;
        [XmlElement]
        public int SightHeightUp;
        [XmlElement]
        public int SightHeightDown;
        [XmlElement]
        public int CustomLastSightRadius;
        [XmlElement]
        public int CustomLastSightUp;
        [XmlElement]
        public int CustomLastSightDown;

        public NpcMetadataDistance() { }
    }

    [XmlType]
    public class NpcMetadataInteract
    {
        [XmlElement]
        public string InteractFunction;  // UseSkill,50100501,1
        [XmlElement]
        public ushort InteractCastingTime;  // 0, 500, 800, 1000, 2000, 4000
        [XmlElement]
        public ushort InteractCooldownTime;  // 0, 5400, 7000, 8000, 12000

        public NpcMetadataInteract() { }
    }
}
