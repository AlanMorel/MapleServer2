using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class GuildBuffMetadata
{
    [XmlElement(Order = 1)]
    public int BuffId;
    [XmlElement(Order = 2)]
    public List<GuildBuffLevel> Levels = new();

    public override string ToString()
    {
        return $"GuildBuff(BuffId:{BuffId},Levels:{Levels}";
    }
}

[XmlType]
public class GuildBuffLevel
{
    [XmlElement(Order = 2)]
    public byte Level;
    [XmlElement(Order = 3)]
    public int EffectId;
    [XmlElement(Order = 4)]
    public byte EffectLevel;
    [XmlElement(Order = 5)]
    public byte LevelRequirement;
    [XmlElement(Order = 6)]
    public int UpgradeCost;
    [XmlElement(Order = 7)]
    public int Cost;
    [XmlElement(Order = 8)]
    public short Duration;

    public override string ToString()
    {
        return
            $"BuffLevel(Level:{Level},EffectId:{EffectId},EffectLevel:{EffectLevel},LevelRequirement:{LevelRequirement},UpgradeCost:{UpgradeCost},Cost:{Cost},Duration:{Duration}";
    }
}
