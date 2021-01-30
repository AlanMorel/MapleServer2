using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class GuildMetadata
    {
        [XmlElement(Order = 1)]
        public readonly List<GuildContribution> Contribution;
        [XmlElement(Order = 2)]
        public readonly List<GuildBuff> Buff;

        // Required for deserialization
        public GuildMetadata()
        {
            Contribution = new List<GuildContribution>();
            Buff = new List<GuildBuff>();
        }

        public override string ToString() =>
            $"GuildMetadata(Contribution:{string.Join(",", Contribution)},Buff{string.Join(",", Buff)}";

        public override int GetHashCode()
        {
            return HashCode.Combine(Contribution);
        }
    }

    [XmlType]
    public class GuildContribution
    {
        [XmlElement(Order = 1)]
        public readonly string Type;
        [XmlElement(Order = 2)]
        public readonly int Value;

        // Required for deserialization
        public GuildContribution() { }

        public GuildContribution(string type, int value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString() =>
            $"GuildContribution(Type:{Type},Value:{Value})";

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Value);
        }
    }

    [XmlType]
    public class GuildBuff
    {
        [XmlElement(Order = 1)]
        public readonly int Id;
        [XmlElement(Order = 2)]
        public readonly byte Level;
        [XmlElement(Order = 3)]
        public readonly int EffectId;
        [XmlElement(Order = 4)]
        public readonly byte EffectLevel;
        [XmlElement(Order = 5)]
        public readonly byte LevelRequirement;
        [XmlElement(Order = 6)]
        public readonly int UpgradeCost;
        [XmlElement(Order = 7)]
        public readonly int Cost;
        [XmlElement(Order = 8)]
        public readonly short Duration;

        public GuildBuff() { }

        public GuildBuff(int id, byte level, int effectId, byte effectLevel, byte levelRequirement, int upgradeCost, int cost, short duration)
        {
            Id = id;
            Level = level;
            EffectId = effectId;
            EffectLevel = effectLevel;
            LevelRequirement = levelRequirement;
            UpgradeCost = upgradeCost;
            Cost = cost;
            Duration = duration;
        }

        public override string ToString() =>
            $"GuildContribution(Id:{Id},Level:{Level},EffectId:{EffectId},EffectLevel:{EffectLevel},LevelRequirement:{LevelRequirement},UpgradeCost:{UpgradeCost},Cost:{Cost},Duration:{Duration}";

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Level, EffectId, EffectLevel, LevelRequirement, UpgradeCost, Cost, Duration);
        }
    }
}
