using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Enums;
using MapleServer2.Tools;
using MapleServer2.Types;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class AdditionalEffectMetadataStorage
{
    public static readonly Dictionary<int, AdditionalEffectMetadata> AdditionalEffectMetadatas = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.AdditionalEffect);
        List<AdditionalEffectMetadata> effects = Serializer.Deserialize<List<AdditionalEffectMetadata>>(stream);
        foreach (AdditionalEffectMetadata effect in effects)
        {
            AdditionalEffectMetadatas[effect.Id] = effect;
        }
    }

    public static bool IsValid(int effectId) => AdditionalEffectMetadatas.ContainsKey(effectId);

    public static AdditionalEffectMetadata GetMetadata(int effectId) => AdditionalEffectMetadatas.GetValueOrDefault(effectId);

    public static AdditionalEffectLevelMetadata GetLevelMetadata(int effectId, int level)
    {
        AdditionalEffectMetadata meta = GetMetadata(effectId);

        if (meta == null)
        {
            return null;
        }

        AdditionalEffectLevelMetadata levelMeta;
        meta.Levels.TryGetValue(level, out levelMeta);

        return levelMeta;
    }
}
