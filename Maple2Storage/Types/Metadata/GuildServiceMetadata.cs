﻿using System;
using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class GuildServiceMetadata
    {
        [XmlElement(Order = 1)]
        public int Id;
        [XmlElement(Order = 2)]
        public string Type;
        [XmlElement(Order = 3)]
        public int Level;
        [XmlElement(Order = 4)]
        public int UpgradeCost;
        [XmlElement(Order = 5)]
        public int LevelRequirement;
        [XmlElement(Order = 6)]
        public int HouseLevelRequirement;

        public GuildServiceMetadata() { }

        public override string ToString() =>
            $"GuildBuff(Id:{Id},Type:{Type},Level:{Level},UpgradeCost:{UpgradeCost},LevelRequirement:{LevelRequirement},HouseLevelRequirement:{HouseLevelRequirement}";

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Type, Level);
        }
    }
}
