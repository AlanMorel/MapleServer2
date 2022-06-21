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

    public AdditionalEffect AddEffect(int id, int level, int stacks = 1)
    {
        // current default behavior of remove and replace, and add stacks
        // doesnt check for shared buff categories (sigils, whetstones, etc)
        // TODO: add correct refreshing behavior based on attributes from the xmls

        int index;
        AdditionalEffect effect;

        if (TryGet(id, level, out effect, out index))
        {
            stacks = Math.Min(effect.LevelMetadata.Basic.MaxBuffCount, stacks + effect.Stacks);

            RemoveAt(index);

            Parent.EffectRemoved(effect);
        }

        effect = new(id, level, stacks);

        Effects.Add(effect);

        Parent.EffectAdded(effect);

        return effect;
    }

    public void RemoveEffect(int id, int level, int stacks = 1)
    {
        int index;
        AdditionalEffect effect;

        if (TryGet(id, level, out effect, out index))
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

    public bool TryGet(int id, int level, out AdditionalEffect effect, out int index)
    {
        effect = null;

        for (index = 0; index < Effects.Count; ++index)
        {
            if (Effects[index].Matches(id))
            {
                effect = Effects[index];

                return true;
            }
        }

        return false;
    }

    public bool TryGet(int id, int level, out AdditionalEffect effect)
    {
        int index = 0;

        return TryGet(id, level, out effect, out index);
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
    public int Level;
    public AdditionalEffectMetadata Metadata;
    public AdditionalEffectLevelMetadata LevelMetadata;
    public int References = 0; // Reference counter. Only remove 
    public int Stacks = 1;

    public AdditionalEffect(int id, int level, int stacks = 1)
    {
        Id = id;
        Level = level;
        Stacks = stacks;

        Metadata = AdditionalEffectMetadataStorage.GetMetadata(id);
        LevelMetadata = AdditionalEffectMetadataStorage.GetLevelMetadata(id, level);
    }

    public bool Matches(int id)
    {
        return Id == id;
    }
}
