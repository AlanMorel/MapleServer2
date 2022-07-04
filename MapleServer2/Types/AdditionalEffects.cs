using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Tools;

namespace MapleServer2.Types;

public struct AdditionalEffectParameters
{
    public int Id;
    public int Level;
    public int Stacks;
    public int Source;
    public int Duration;
    public bool IsBuff;

    public AdditionalEffectParameters(int id, int level)
    {
        Id = id;
        Level = level;
        Stacks = 1;
        Source = -1;
        Duration = 0;
        IsBuff = false;
    }
}

public class AdditionalEffects
{
    public List<AdditionalEffect> Effects;
    public List<AdditionalEffect> TimedEffects;
    public IFieldActor Parent;
    private int NextBuffExpiration = -1;
    private AdditionalEffect NextEndingBuff = null;

    public AdditionalEffects()
    {
        Effects = new();
        TimedEffects = new();
    }

    public void UpdateEffects()
    {
        bool recomputedLastIteration = false;

        while (NextBuffExpiration != -1 && TimedEffects.Count > 0 && Environment.TickCount >= NextBuffExpiration)
        {
            int index = FindExpiredBuff();

            if (index == -1)
            {
                if (recomputedLastIteration)
                {
                    return;
                }

                RecomputeExpiration();

                recomputedLastIteration = true;

                continue;
            }

            recomputedLastIteration = false;

            RemoveEffect(Effects[index], index);
        }
    }

    public AdditionalEffect AddEffect(AdditionalEffectParameters parameters)
    {
        // current default behavior of remove and replace, and add stacks
        // doesnt check for shared buff categories (sigils, whetstones, etc)
        // TODO: add correct refreshing behavior based on attributes from the xmls

        AdditionalEffect effect = new(parameters.Id, parameters.Level, parameters.Stacks);

        if (effect.LevelMetadata == null)
        {
            return null;
        }

        EffectCancelEffectMetadata cancel = effect.LevelMetadata.CancelEffect;

        for (int i = 0; i < Effects.Count; i++)
        {
            AdditionalEffect oldEffect = Effects[i];
            EffectImmuneEffectMetadata immune = oldEffect.LevelMetadata.ImmuneEffect;

            if (immune?.ImmuneEffectCodes?.Contains(effect.Id) == true)
            {
                return null;
            }

            if (oldEffect.Id == effect.Id && oldEffect.Level > effect.Level)
            {
                return null;
            }

            if (oldEffect.Id == effect.Id)
            {
                parameters.Stacks = Math.Min(oldEffect.LevelMetadata.Basic.MaxBuffCount, parameters.Stacks + oldEffect.Stacks);

                RemoveEffect(oldEffect, i, true);

                break;
            }

            if (cancel?.CancelEffectCodes?.Contains(oldEffect.Id) == true)
            {
                RemoveEffect(oldEffect, i, true);

                break;
            }
        }

        effect.BuffId = GuidGenerator.Int();
        effect.SourceId = parameters.Source == -1 ? Parent.ObjectId : parameters.Source;
        effect.Start = Environment.TickCount;
        effect.Duration = parameters.Duration;

        if (parameters.Duration != 0)
        {
            TimedEffects.Add(effect);

            AddExpiringBuffTime(effect.End);
        }

        if (parameters.Duration == 0)
        {
            --effect.Start;
        }

        Effects.Add(effect);

        Parent.EffectAdded(effect);

        Parent.FieldManager.BroadcastPacket(BuffPacket.AddBuff(effect, Parent.ObjectId));

        return effect;
    }

    public void RemoveEffect(int id, int level, bool sendPacket = true)
    {
        int index;
        AdditionalEffect effect;

        if (TryGet(id, level, out effect, out index))
        {
            RemoveEffect(effect, index, sendPacket);
        }
    }

    public void RemoveEffect(AdditionalEffect effect, int index, bool sendPacket = true)
    {
        RemoveAt(index);

        Parent.EffectRemoved(effect);

        if (effect.BuffId != -1)
        {
            if (sendPacket)
            {
                Parent.FieldManager.BroadcastPacket(BuffPacket.RemoveBuff(effect, Parent.ObjectId));
            }

            int timedIndex = -1;

            if (TryGet(effect.Id, effect.Level, out effect, out timedIndex, true) && effect.LevelMetadata != null)
            {
                RemoveAt(timedIndex, true);
            }

            RecomputeExpiration();
        }
    }

    public int FindExpiredBuff()
    {
        int index = -1;

        for (int i = 0; i < TimedEffects.Count; ++i)
        {
            if (TimedEffects[i].End < Environment.TickCount)
            {
                index = i;
            }
        }

        if (index == -1)
        {
            return -1;
        }

        for (int i = 0; i < Effects.Count; ++i)
        {
            if (Effects[i] == TimedEffects[index])
            {
                return i;
            }
        }

        return -1;
    }

    public void AddExpiringBuffTime(int time)
    {
        if (NextBuffExpiration == -1)
        {
            NextBuffExpiration = time;

            return;
        }

        NextBuffExpiration = Math.Min(NextBuffExpiration, time);
    }

    public void RecomputeExpiration()
    {
        if (TimedEffects.Count > 0)
        {
            NextBuffExpiration = TimedEffects[0].End;

            for (int i = 1; i < TimedEffects.Count; ++i)
            {
                NextBuffExpiration = Math.Min(NextBuffExpiration, TimedEffects[i].End);
            }

            return;
        }

        NextBuffExpiration = -1;
    }

    public void RemoveAt(int index, bool removeTimed = false)
    {
        List<AdditionalEffect> list = removeTimed ? TimedEffects : Effects;

        list[index] = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
    }

    public bool TryGet(int id, int level, out AdditionalEffect effect, out int index, bool getTimed = false)
    {
        effect = null;

        List<AdditionalEffect> list = getTimed ? TimedEffects : Effects;

        for (index = 0; index < list.Count; ++index)
        {
            if (list[index].Matches(id))
            {
                effect = list[index];

                return true;
            }
        }

        return false;
    }

    public bool TryGet(int id, int level, out AdditionalEffect effect, bool getTimed = false)
    {
        int index = 0;

        return TryGet(id, level, out effect, out index, getTimed);
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
    public int SourceId = -1;
    public int BuffId = -1;
    public int Start = -1;
    public int Duration = 0;
    public int End { get => Start + Duration; }

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
