using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class GuildContributionMetadata
    {
        [XmlElement(Order = 1)]
        public string Type;
        [XmlElement(Order = 2)]
        public int Value;

        public GuildContributionMetadata() { }

        public GuildContributionMetadata(string type, int value)
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
}
