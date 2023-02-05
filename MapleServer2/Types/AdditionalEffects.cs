using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Managers.Actors;
using MapleServer2.Packets;

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

public class CompulsionEventRate
{
    public CompulsionEventType Type = CompulsionEventType.None;
    public float Rate = 0;
    public int[] SkillCodes = Array.Empty<int>();
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
    public List<CompulsionEventRate> CompulsionEvents = new();
    private Dictionary<EffectEvent, List<AdditionalEffect>> ListeningEvents = new();
    private Dictionary<EffectEvent, List<AdditionalEffect>> ListeningTickEvents = new();

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
        CompulsionEvents.Clear();
    }

    public float GetCompulsionRate(CompulsionEventType type, int skillCode = 0)
    {
        float rate = 0;

        foreach (CompulsionEventRate compulsion in CompulsionEvents)
        {
            if (compulsion.Type == type && (compulsion.SkillCodes.Length == 0 || compulsion.SkillCodes.Contains(skillCode)))
            {
                rate += compulsion.Rate;
            }
        }

        return rate;
    }

    public void UpdateStatsIfStale(EffectEvent effectEvent = EffectEvent.Tick)
    {
        if (Parent is null)
        {
            return;
        }

        foreach (AdditionalEffect effect in Effects)
        {
            bool recompute = effect.UpdateIfStale(Parent, false, effectEvent);

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

        int effectId = effect.Id;
        int effectGroup = effect.LevelMetadata.Basic.Group;

        foreach (AdditionalEffect activeEffect in Effects)
        {
            int activeId = activeEffect.Id;
            int activeGroup = activeEffect.LevelMetadata.Basic.Group;

            EffectImmuneEffectMetadata? immune = activeEffect.LevelMetadata.ImmuneEffect;

            if (immune?.ImmuneEffectCodes.Contains(effect.Id) ?? false)
            {
                return false;
            }

            if (effectId != activeId && effectGroup != activeId && effectId != activeGroup && (effectGroup + activeGroup == 0 || effectGroup != activeGroup))
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

        Parent.EffectUpdated(effect);
        effect.Process(Parent);

        return true;
    }

    public void DebugPrint(AdditionalEffect effect, EffectEvent effectEvent, ConditionSkillTarget effectInfo, int parameter = 0)
    {
        Player? ownerPlayer = (Parent as Character)?.Value;
        Player? casterPlayer = (effect.Caster as Character)?.Value;
        bool debugPrintOwner = false;
        bool debugPrintCaster = false;

        if (ownerPlayer is not null)
        {
            debugPrintOwner = ownerPlayer.DebugPrint.PrintOwnEffects && (effect.Caster == Parent || ownerPlayer.DebugPrint.PrintEffectsFromOthers);
            debugPrintOwner &= ownerPlayer.DebugPrint.PrintEffectEvents && (effectEvent != EffectEvent.Tick || ownerPlayer.DebugPrint.IncludeEffectTickEvent);
        }

        if (casterPlayer is not null && casterPlayer != ownerPlayer)
        {
            debugPrintCaster = casterPlayer.DebugPrint.PrintCastedEffects;
            debugPrintCaster &= casterPlayer.DebugPrint.PrintEffectEvents && (effectEvent != EffectEvent.Tick || casterPlayer.DebugPrint.IncludeEffectTickEvent);
        }

        if (debugPrintOwner)
        {
            ownerPlayer?.Session?.Send(NoticePacket.Notice($"Event {effectEvent} ({parameter}) fired on {effect.Id} [{effect.Level}] x{effect.Stacks}", NoticeType.Chat));
        }

        if (debugPrintCaster)
        {
            casterPlayer?.Session?.Send(NoticePacket.Notice($"Event {effectEvent} ({parameter}) fired on {effect.Id} [{effect.Level}] x{effect.Stacks} on target {Parent?.ObjectId}", NoticeType.Chat));
        }
    }

    private void DebugPrint(string message, AdditionalEffect effect)
    {
        Player? ownerPlayer = (Parent as Character)?.Value;
        Player? casterPlayer = (effect.Caster as Character)?.Value;
        bool debugPrintOwner = ownerPlayer is not null && ownerPlayer.DebugPrint.PrintOwnEffects && (effect.Caster == Parent || ownerPlayer.DebugPrint.PrintEffectsFromOthers);
        bool debugPrintCaster = casterPlayer is not null && casterPlayer != ownerPlayer && casterPlayer.DebugPrint.PrintCastedEffects;

        if (debugPrintOwner)
        {
            if (effect.Caster is not null)
            {
                ownerPlayer?.Session?.Send(NoticePacket.Notice($"{message} {effect.Id} [{effect.Level}] x{effect.Stacks} from caster {effect.Caster.ObjectId}", NoticeType.Chat));
            }
            else
            {
                ownerPlayer?.Session?.Send(NoticePacket.Notice($"{message} {effect.Id} [{effect.Level}] x{effect.Stacks}", NoticeType.Chat));
            }
        }

        if (debugPrintCaster)
        {
            casterPlayer?.Session?.Send(NoticePacket.Notice($"{message} {effect.Id} [{effect.Level}] x{effect.Stacks} to target {Parent?.ObjectId}", NoticeType.Chat));
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

        if (!CanApplyEffect(effect, out AdditionalEffect? activeEffect))
        {
            DebugPrint("Failed to apply effect", effect);

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

        if (activeEffect is not null && UpdateEffect(activeEffect, effect, parameters))
        {
            DebugPrint("Updated effect", activeEffect);

            return activeEffect;
        }

        if (parameters.Stacks == 0)
        {
            DebugPrint("Attempted to add effect with 0 stacks", effect);

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

        DebugPrint("Applied effect", effect);

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

        if (effect.HasEvents)
        {
            RefreshEffectEvents();
        }

        DebugPrint("Removed effect", effect);
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

    public AdditionalEffect? GetEffect(int effectId, int stacks = 0, ConditionOperator comparison = ConditionOperator.GreaterEquals, int level = 0, IFieldActor? caster = null, bool allowInactive = true)
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

            if (!allowInactive && !effect.IsActive)
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
        return GetEffect(effectId, stacks, comparison, level, caster, false) is not null;
    }

    public void AddListeningEventTick(AdditionalEffect effect, BeginConditionSubject subject)
    {
        if (subject is null || subject.EventCondition == EffectEvent.Activate)
        {
            return;
        }

        effect.HasEvents = true;

        if (!ListeningTickEvents.TryGetValue(subject.EventCondition, out List<AdditionalEffect>? list))
        {
            list = new();

            ListeningTickEvents.Add(subject.EventCondition, list);
        }

        if (!list.Contains(effect))
        {
            list.Add(effect);
        }
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
        AddListeningEventTick(effect, effect.LevelMetadata.BeginCondition.Owner);
        AddListeningEventTick(effect, effect.LevelMetadata.BeginCondition.Target);

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

    public List<AdditionalEffect>? GetListeningTickEvents(EffectEvent effectEvent)
    {
        ListeningTickEvents.TryGetValue(effectEvent, out List<AdditionalEffect>? events);

        return events;
    }

    public void RefreshEffectEvents()
    {
        foreach ((EffectEvent type, List<AdditionalEffect> listeners) in ListeningEvents)
        {
            listeners.Clear();
        }

        foreach ((EffectEvent type, List<AdditionalEffect> listeners) in ListeningTickEvents)
        {
            listeners.Clear();
        }

        foreach (AdditionalEffect effect in Effects)
        {
            AddListeningEvents(effect);
        }
    }
}
