using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;

namespace MapleServer2.Types;

public class AdditionalEffects
{
    public List<AdditionalEffect> Effects;
    public IFieldActor Parent;

    public AdditionalEffects()
    {
        Effects = new();
    }

    public AdditionalEffect AddEffect(int id, int level, string feature, int stacks = 1)
    {
        // current default behavior of remove and replace, and add stacks
        // doesnt check for shared buff categories (sigils, whetstones, etc)
        // TODO: add correct refreshing behavior based on attributes from the xmls

        int index;
        AdditionalEffect effect;
        bool found = TryGet(id, level, feature, out effect, out index);

        if (found)
        {
            stacks = Math.Min(effect.LevelMetadata.Basic.MaxBuffCount, stacks + effect.Stacks);

            RemoveAt(index);

            Parent.EffectRemoved(effect);
        }

        effect = new(id, level, feature, stacks);

        Effects.Add(effect);

        Parent.EffectAdded(effect);

        return effect;
    }

    public void RemoveEffect(int id, int level, string feature, int stacks = 1)
    {
        int index;
        AdditionalEffect effect;
        bool found = TryGet(id, level, feature, out effect, out index);

        if (found)
        {
            RemoveAt(index);

            Parent.EffectRemoved(effect);
        }
    }

    public void RemoveAt(int index)
    {
        Effects[index] = Effects[Effects.Count - 1];
        Effects.RemoveAt(Effects.Count - 1);
    }

    public bool TryGet(int id, int level, string feature, out AdditionalEffect effect, out int index)
    {
        effect = null;

        for (index = 0; index < Effects.Count; ++index)
        {
            if (Effects[index].Matches(id, level, feature))
            {
                effect = Effects[index];

                return true;
            }
        }

        return false;
    }

    public bool TryGet(int id, int level, string feature, out AdditionalEffect effect)
    {
        int index = 0;

        return TryGet(id, level, feature, out effect, out index);
    }

    public int CountEffects(AdditionalEffectLevelMetadata level)
    {
        int count = 0;

        return count;
    }
}

public class AdditionalEffect
{
    public int Id;
    public string Feature;
    public int Level;
    public AdditionalEffectMetadata Metadata;
    public AdditionalEffectLevelMetadata LevelMetadata;
    public int References = 0; // Reference counter. Only remove 
    public int Stacks = 1;

    public AdditionalEffect(int id, int level, string feature, int stacks = 1)
    {
        Id = id;
        Feature = feature;
        Level = level;
        Stacks = stacks;

        Metadata = AdditionalEffectMetadataStorage.GetMetadata(id);
        LevelMetadata = Metadata?.GetLevel(level, feature);
    }

    public bool Matches(int id, int level, string feature = "")
    {
        return Id == id && (feature == "" || Feature == feature);
    }
}
