using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Managers.Actors;

namespace MapleServer2.Types;

public struct AdditionalEffectParameters
{
    public readonly int Id;
    public readonly int Level;
    public int Stacks;
    public IFieldActor? Caster;
    public bool IsBuff;
    public int Duration = 0;
    public SkillCast? ParentSkill;

    public AdditionalEffectParameters(int id, int level)
    {
        Id = id;
        Level = level;
        Stacks = 1;
        Caster = null;
        IsBuff = false;
        ParentSkill = null;
    }
}

public class AdditionalEffects
{
    public readonly List<AdditionalEffect> Effects;
    public IFieldActor? Parent;
    public bool AlwaysCrit;
    public bool Invincible;
    public long MinimumHp;
    public Dictionary<StatAttribute, float> Resistances = new();
    public AdditionalEffect? ActiveShield = null;
    private Dictionary<EffectEvent, List<AdditionalEffect>> ListeningEvents = new();

    public AdditionalEffects(IFieldActor? parent = null)
    {
        Effects = new();
        Parent = parent;

        ResetStatus();
    }

    public void ResetStatus()
    {
        AlwaysCrit = false;
        Invincible = false;
        MinimumHp = 0;
        Resistances.Clear();
        ActiveShield = null;
    }

    public void UpdateStatsIfStale(EffectEvent effectEvent = EffectEvent.Tick)
    {
        if (Parent is null)
        {
            return;
        }

        foreach (AdditionalEffect effect in Effects)
        {
            bool recompute = effect.UpdateIfStale(Parent, effectEvent);

            if (recompute)
            {
                Parent.ComputeStats();

                return;
            }
        }
    }

    public bool CanApplyEffect(AdditionalEffect effect, out AdditionalEffect? oldEffect)
    {
        oldEffect = null;

        foreach (AdditionalEffect activeEffect in Effects)
        {
            EffectImmuneEffectMetadata? immune = activeEffect.LevelMetadata.ImmuneEffect;

            if (immune?.ImmuneEffectCodes.Contains(effect.Id) ?? false)
            {
                return false;
            }

            if (activeEffect.Id != effect.Id && effect.LevelMetadata.Basic.Group != activeEffect.Id)
            {
                continue;
            }

            if (effect.LevelMetadata.Basic.CasterIndividualEffect == CasterIndividualEffect.PerCasterStack && effect.Caster != activeEffect.Caster)
            {
                continue;
            }

            oldEffect = activeEffect;

            return true;
        }

        return true;
    }

    public bool UpdateEffect(AdditionalEffect? effect, AdditionalEffect newEffect, AdditionalEffectParameters parameters)
    {
        if (effect is null || Parent is null)
        {
            return false;
        }

        bool wasBelowMax = effect.Stacks < effect.LevelMetadata.Basic.MaxBuffCount;

        effect.Stacks = Math.Max(Math.Min(effect.LevelMetadata.Basic.MaxBuffCount, parameters.Stacks + effect.Stacks), 0);

        if (effect.LevelMetadata.Basic.ResetCondition != EffectResetCondition.DontResetTimer)
        {
            if (effect.LevelMetadata.Basic.ResetCondition != EffectResetCondition.ChangeTimer)
            {
                effect.Start = Environment.TickCount;
            }

            effect.Duration = newEffect.LevelMetadata.Basic.DurationTick;
        }

        if (effect.Stacks == 0)
        {
            effect.Stop(Parent);

            return true;
        }

        if (effect.Stacks > 1)
        {
            Parent.SkillTriggerHandler.FireEvents(Parent, null, EffectEvent.OnBuffStacksReached, effect.Id);

            if (wasBelowMax && effect.Stacks == effect.LevelMetadata.Basic.MaxBuffCount)
            {
                Parent.SkillTriggerHandler.FireEvents(Parent, null, EffectEvent.UnknownWizardEvent, effect.Id);
                Parent.SkillTriggerHandler.FireEvents(Parent, null, EffectEvent.UnknownStrikerEvent, effect.Id);
            }
        }

        if (effect.BuffId != 0)
        {
            Parent.FieldManager?.BroadcastPacket(BuffPacket.UpdateBuff(effect, Parent.ObjectId));
        }

        effect.Process(Parent);

        return true;
    }

    private void DebugPrint(string message, AdditionalEffect effect, Player? ownerPlayer, Player? casterPlayer, bool debugPrintOwner, bool debugPrintCaster)
    {
        if (debugPrintOwner)
        {
            if (effect.Caster is not null)
            {
                ownerPlayer?.Session?.SendNotice($"{message} {effect.Id} from caster {effect.Caster.ObjectId}");
            }
            else
            {
                ownerPlayer?.Session?.SendNotice($"{message} {effect.Id}");
            }
        }

        if (debugPrintCaster)
        {
            casterPlayer?.Session?.SendNotice($"{message} {effect.Id} to target {Parent.ObjectId}");
        }
    }

    public AdditionalEffect? AddEffect(AdditionalEffectParameters parameters)
    {
        if (Parent is null)
        {
            return null;
        }

        // current default behavior of remove and replace, and add stacks
        // doesnt check for shared buff categories (sigils, whetstones, etc)
        // TODO: add correct refreshing behavior based on attributes from the xmls
        if (AdditionalEffectMetadataStorage.GetMetadata(parameters.Id) is null)
        {
            return null;
        }

        AdditionalEffect effect = new(Parent, parameters.Id, parameters.Level, parameters.Stacks);
        int start = Environment.TickCount;

        effect.Caster = parameters.Caster ?? Parent;
        effect.ParentSkill = parameters.ParentSkill;

        Player? ownerPlayer = (Parent as Character)?.Value;
        Player? casterPlayer = (effect.Caster as Character)?.Value;
        bool debugPrintOwner = ownerPlayer is not null && ownerPlayer.DebugPrint.PrintOwnEffects && (effect.Caster == Parent || ownerPlayer.DebugPrint.PrintEffectsFromOthers);
        bool debugPrintCaster = casterPlayer is not null && casterPlayer.DebugPrint.PrintCastedEffects;

        if (!CanApplyEffect(effect, out AdditionalEffect? activeEffect))
        {
            DebugPrint("Failed to apply effect", effect, ownerPlayer, casterPlayer, debugPrintOwner, debugPrintCaster);

            return null;
        }

        if (activeEffect is not null && activeEffect.Level != parameters.Level)
        {
            if (activeEffect.Level > parameters.Level)
            {
                return null;
            }

            if (activeEffect.Level < parameters.Level)
            {
                activeEffect.Stop(Parent);

                activeEffect = null;
            }
        }

        if (UpdateEffect(activeEffect, effect, parameters))
        {
            DebugPrint("Updated effect", effect, ownerPlayer, casterPlayer, debugPrintOwner, debugPrintCaster);

            return activeEffect;
        }

        if (parameters.Stacks == 0)
        {
            return null;
        }

        effect.Stacks = parameters.Stacks;
        effect.Start = start;
        effect.Duration = parameters.Duration != 0 ? parameters.Duration : effect.LevelMetadata.Basic.DurationTick;

        InvokeStatValue invokeStat = effect.Caster.Stats.GetEffectStats(effect.Id, effect.LevelMetadata.Basic.Group, InvokeEffectType.IncreaseDuration);

        effect.Duration = Math.Max(0, (int) invokeStat.Value + (int) ((1 + invokeStat.Rate) * effect.Duration));

        if (effect.LevelMetadata.Basic.KeepCondition == EffectKeepCondition.UnlimitedDuration)
        {
            --effect.Start;
        }

        Effects.Add(effect);

        AddListeningEvents(effect);

        Parent.EffectAdded(effect);
        effect.Process(Parent);

        DebugPrint("Applied effect", effect, ownerPlayer, casterPlayer, debugPrintOwner, debugPrintCaster);

        return effect;
    }

    public void EffectStopped(AdditionalEffect effect)
    {
        Player? ownerPlayer = (Parent as Character)?.Value;
        Player? casterPlayer = (effect.Caster as Character)?.Value;
        bool debugPrintOwner = ownerPlayer is not null && ownerPlayer.DebugPrint.PrintOwnEffects && (effect.Caster == Parent || ownerPlayer.DebugPrint.PrintEffectsFromOthers);
        bool debugPrintCaster = casterPlayer is not null && casterPlayer.DebugPrint.PrintCastedEffects;

        foreach (SkillCondition condition in effect.LevelMetadata.ConditionSkill)
        {
            if (condition.BeginCondition.Caster is not null)
            {
                effect.Caster?.SkillTriggerHandler.RemoveListeningExternalEffect(condition.BeginCondition.Caster.EventCondition, effect);
            }
        }

        Effects.Remove(effect);
        Parent?.EffectRemoved(effect);

        DebugPrint("Removed effect", effect, ownerPlayer, casterPlayer, debugPrintOwner, debugPrintCaster);
    }

    public static bool CompareValues<T>(T target, T value, ConditionOperator comparison) where T : IComparable
    {
        return comparison switch
        {
            ConditionOperator.None => false,
            ConditionOperator.Equals => target.Equals(value),
            ConditionOperator.LessEquals => target.CompareTo(value) <= 0,
            ConditionOperator.GreaterEquals => target.CompareTo(value) >= 0,
            ConditionOperator.Less => target.CompareTo(value) < 0,
            ConditionOperator.Greater => target.CompareTo(value) > 0,
            _ => false
        };
    }

    public AdditionalEffect? GetEffect(int effectId, int stacks = 0, ConditionOperator comparison = ConditionOperator.GreaterEquals, int level = 0, IFieldActor? caster = null)
    {
        foreach (AdditionalEffect effect in Effects)
        {
            if (effect.Id != effectId)
            {
                continue;
            }

            if (caster is not null && effect.Caster != caster)
            {
                continue;
            }

            if (level != 0 && effect.Level != level)
            {
                continue;
            }

            if (!CompareValues(effect.Stacks, stacks, comparison))
            {
                continue;
            }

            return effect;
        }

        return null;
    }

    public bool HasEffect(int effectId, int stacks = 0, ConditionOperator comparison = ConditionOperator.GreaterEquals, int level = 0, IFieldActor? caster = null)
    {
        return GetEffect(effectId, stacks, comparison, level, caster) is not null;
    }

    public void AddListeningEvent(AdditionalEffect effect, BeginConditionSubject subject)
    {
        if (subject is null || subject.EventCondition == EffectEvent.Activate)
        {
            return;
        }

        effect.HasEvents = true;

        if (!ListeningEvents.TryGetValue(subject.EventCondition, out List<AdditionalEffect>? list))
        {
            list = new();

            ListeningEvents.Add(subject.EventCondition, list);
        }

        if (!list.Contains(effect))
        {
            list.Add(effect);
        }
    }

    public void AddListeningEvent(AdditionalEffect effect, BeginConditionSubject subject, IFieldActor? listeningTo, EffectEventOrigin origin)
    {
        if (subject is null || subject.EventCondition == EffectEvent.Activate || listeningTo is null)
        {
            return;
        }

        effect.HasEvents = true;

        listeningTo.SkillTriggerHandler.AddListeningExternalEffect(subject.EventCondition, effect, origin);
    }

    public void AddListeningEvents(AdditionalEffect effect)
    {
        if (effect.LevelMetadata.ConditionSkill is null)
        {
            return;
        }

        foreach (SkillCondition condition in effect.LevelMetadata.ConditionSkill)
        {
            AddListeningEvent(effect, condition.BeginCondition.Owner);
            AddListeningEvent(effect, condition.BeginCondition.Target);
            AddListeningEvent(effect, condition.BeginCondition.Caster, effect.Caster, EffectEventOrigin.Caster);
        }
    }

    public List<AdditionalEffect>? GetListeningEvents(EffectEvent effectEvent)
    {
        ListeningEvents.TryGetValue(effectEvent, out List<AdditionalEffect>? events);

        return events;
    }

    public void RefreshEffectEvents()
    {
        foreach ((EffectEvent type, List<AdditionalEffect> listeners) in ListeningEvents)
        {
            listeners.Clear();
        }

        foreach (AdditionalEffect effect in Effects)
        {
            AddListeningEvents(effect);
        }
    }
}
