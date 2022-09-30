using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class SetItemOptionMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public List<SetBonusMetadata> Parts;
}

[XmlType]
public class SetBonusMetadata
{
    [XmlElement(Order = 1)]
    public int Count;
    [XmlElement(Order = 2)]
    public int[] AdditionalEffectIds;
    [XmlElement(Order = 3)]
    public int[] AdditionalEffectLevels;
    [XmlElement(Order = 4)]
    public Dictionary<StatAttribute, EffectStatMetadata> Stats;
    [XmlElement(Order = 5)]
    public SgiTarget SgiTarget;
    [XmlElement(Order = 6)]
    public SgiTarget SgiBossTarget;

    public SetBonusMetadata(int[]? additionalEffectIds = null, int[]? additionalEffectLevels = null, Dictionary<StatAttribute, EffectStatMetadata>? stats = null)
    {
        Count = 0;
        AdditionalEffectIds = additionalEffectIds ?? Array.Empty<int>();
        AdditionalEffectLevels = additionalEffectLevels ?? Array.Empty<int>();
        Stats = stats ?? new();
    }
}
