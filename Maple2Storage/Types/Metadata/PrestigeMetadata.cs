using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class PrestigeMetadata
{
    [XmlElement(Order = 1)]
    public readonly List<PrestigeReward> Rewards = new();

    public override string ToString()
    {
        return $"PrestigeMetadata(Rewards:{string.Join(",", Rewards)})";
    }
}

[XmlType]
public class PrestigeReward
{
    [XmlElement(Order = 1)]
    public readonly int Level;
    [XmlElement(Order = 2)]
    public readonly string Type;
    [XmlElement(Order = 3)]
    public readonly int Id;
    [XmlElement(Order = 4)]
    public readonly int Value;

    public PrestigeReward() { }

    public PrestigeReward(int level, string type, int id, int value)
    {
        Level = level;
        Type = type;
        Id = id;
        Value = value;
    }

    public override string ToString()
    {
        return $"PrestigeReward(Level:{Level},Type:{Type},Id:{Id},Value:{Value})";
    }
}
