using System.ComponentModel;
using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Tools;
using M2dXmlGenerator;
using Maple2.File.IO.Crypto.Common;
using Maple2.File.Parser.Tools;
using Maple2.File.Parser.Xml.AdditionalEffect;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class AdditionalEffectParser : Exporter<List<AdditionalEffectMetadata>>
{
    public AdditionalEffectParser(MetadataResources resources) : base(resources, MetadataName.AdditionalEffect) { }

    protected override List<AdditionalEffectMetadata> Parse()
    {
        List<AdditionalEffectMetadata> effects = new();
        Dictionary<int, AdditionalEffectMetadata> effectsById = new();

        Filter.Load(Resources.XmlReader, "NA", "Live");

        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("additionaleffect"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);

            int effectId = int.Parse(Path.GetFileNameWithoutExtension(entry.Name));

            AdditionalEffectMetadata metadata = new()
            {
                Id = effectId,
                Levels = new()
            };

            effects.Add(metadata);
            effectsById.Add(effectId, metadata);

            foreach (XmlNode level in document.SelectNodes("/ms2/level")!)
            {
                string? feature = level.Attributes?["feature"]?.Value;

                if (feature is not null && !FeatureLocaleFilter.Features.ContainsKey(feature))
                {
                    continue;
                }

                feature = feature ?? "";

                string? locale = level.Attributes?["locale"]?.Value;

                if (locale is not null && FeatureLocaleFilter.Locale != locale)
                {
                    continue;
                }

                XmlNode? statusProperty = level.SelectSingleNode("StatusProperty");
                Dictionary<StatAttribute, EffectStatMetadata> stats = new();

                AdditionalEffectLevelMetadata levelMeta = new()
                {
                    BeginCondition = SkillParser.ParseBeginCondition(level) ?? new(),
                    Basic = ParseBasicProperty(level.SelectSingleNode("BasicProperty")),
                    CancelEffect = ParseCancelEffect(level.SelectSingleNode("CancelEffectProperty")),
                    ImmuneEffect = ParseImmuneEffect(level.SelectSingleNode("ImmuneEffectProperty")),
                    ResetCoolDownTime = ParseResetCoolDown(level.SelectSingleNode("ResetSkillCoolDownTimeProperty")),
                    ModifyEffectDuration = ParseModifyEffectDuration(level.SelectSingleNode("ModifyEffectDurationProperty")),
                    ModifyOverlapCount = ParseModifyOverlapCount(level.SelectSingleNode("ModifyOverlapCountProperty")),
                    Offensive = ParseOffensive(level.SelectSingleNode("OffensiveProperty"), stats),
                    Defesive = ParseDefensive(level.SelectSingleNode("DefensiveProperty"), stats),
                    Recovery = ParseRecovery(level.SelectSingleNode("RecoveryProperty")),
                    DotDamage = ParseDotDamage(level.SelectSingleNode("DotDamageProperty")),
                    InvokeEffect = ParseInvokeEffect(level.SelectSingleNode("InvokeEffectProperty")),
                    Status = ParseStatusProperty(level.SelectSingleNode("StatusProperty"), stats),
                    Ride = ParseRide(level.SelectSingleNode("RideeProperty")),
                    Shield = ParseShield(level.SelectSingleNode("ShieldProperty")),
                    SplashSkill = new(),
                    ConditionSkill = new()
                };

                if (!metadata.Levels.ContainsKey(levelMeta.Basic.Level))
                {
                    metadata.Levels.Add(levelMeta.Basic.Level, levelMeta);
                }
                else
                {
                    if (feature != "")
                    {
                        metadata.Levels[levelMeta.Basic.Level] = levelMeta;
                    }
                }

                SkillParser.ParseConditionSkill(level, levelMeta.ConditionSkill);
                SkillParser.ParseConditionSkill(level, levelMeta.SplashSkill, "splashSkill");

                ParseStats(statusProperty, "Stat", stats);
                ParseStats(statusProperty, "SpecialAbility", stats);

                levelMeta.HasStats = stats.Count > 0;
                levelMeta.HasStats |= levelMeta.Status.Resistances.Count > 0;
                levelMeta.HasStats |= (levelMeta.InvokeEffect?.Types?.Length ?? 0) > 0;
                levelMeta.HasStats |= levelMeta.Status.DeathResistanceHp != 0;
                levelMeta.HasStats |= levelMeta.Offensive.AlwaysCrit;
                levelMeta.HasStats |= levelMeta.Defesive.Invincible;
                levelMeta.HasStats |= levelMeta.Shield?.HpValue > 0;
                levelMeta.HasStats |= levelMeta.Basic.AllowedSkillAttacks.Length > 0 || levelMeta.Basic.AllowedDotEffectAttacks.Length > 0;
                levelMeta.HasConditionalStats = levelMeta.HasStats && !IsDefaultBeginCondition(levelMeta.BeginCondition);
            }
        }

        return effects;
    }

    private bool IsDefaultBeginCondition(SkillBeginCondition condition)
    {
        if (condition.Owner is not null || condition.Target is not null || condition.Caster is not null)
        {
            return false;
        }

        if (condition.Stat is not null)
        {
            return false;
        }

        if (condition.RequireSkillCodes is not null)
        {
            return false;
        }

        if (condition.RequireMapCodes is not null)
        {
            return false;
        }

        if (condition.RequireMapCategoryCodes is not null)
        {
            return false;
        }

        if (condition.RequireDungeonRooms is not null)
        {
            return false;
        }

        if (condition.Jobs is not null)
        {
            return false;
        }

        if (condition.MapContinents is not null)
        {
            return false;
        }

        return true;
    }

    private void AddResistance(Dictionary<StatAttribute, float> resistances, XmlAttribute? attribute, StatAttribute type)
    {
        if (attribute is null)
        {
            return;
        }

        resistances[type] = float.Parse(attribute?.Value ?? "0");
    }

    private EffectStatusMetadata ParseStatusProperty(XmlNode? statusNode, Dictionary<StatAttribute, EffectStatMetadata> stats)
    {
        if (statusNode is null)
        {
            return new()
            {
                Stats = stats
            };
        }

        Dictionary<StatAttribute, float> resistances = new();

        AddResistance(resistances, statusNode?.Attributes?["resWapR"], StatAttribute.MaxWeaponAtk);
        AddResistance(resistances, statusNode?.Attributes?["resBapR"], StatAttribute.BonusAtk);
        AddResistance(resistances, statusNode?.Attributes?["resCadR"], StatAttribute.CritDamage);
        AddResistance(resistances, statusNode?.Attributes?["resAtpR"], StatAttribute.Accuracy);
        AddResistance(resistances, statusNode?.Attributes?["resEvpR"], StatAttribute.Evasion);
        AddResistance(resistances, statusNode?.Attributes?["resPenR"], StatAttribute.Pierce);
        AddResistance(resistances, statusNode?.Attributes?["resAspR"], StatAttribute.AttackSpeed);

        return new()
        {
            Stats = stats,
            DeathResistanceHp = long.Parse(statusNode?.Attributes?["deathResistanceHP"]?.Value ?? "0"),
            Resistances = resistances
        };
    }

    private void ParseStats(XmlNode? parentNode, string nodeName, Dictionary<StatAttribute, EffectStatMetadata> stats)
    {
        if (parentNode is null)
        {
            return;
        }

        foreach (XmlNode stat in parentNode.SelectNodes(nodeName)!)
        {
            ParseStatNode(stat, stats);
        }
    }

    private EffectBasicPropertyMetadata ParseBasicProperty(XmlNode? parentNode)
    {
        return new()
        {
            Level = int.Parse(parentNode?.Attributes?["level"]?.Value ?? "1"),
            MaxBuffCount = int.Parse(parentNode?.Attributes?["maxBuffCount"]?.Value ?? "1"),
            SkillGroupType = (SkillGroupType) int.Parse(parentNode?.Attributes?["maxBuffCount"]?.Value ?? "0"),
            Group = int.Parse(parentNode?.Attributes?["groupIDs"]?.Value ?? "0"),
            DotCondition = (EffectDotCondition) int.Parse(parentNode?.Attributes?["dotCondition"]?.Value ?? "0"),
            ResetCondition = (EffectResetCondition) int.Parse(parentNode?.Attributes?["resetCondition"]?.Value ?? "0"),
            DurationTick = int.Parse(parentNode?.Attributes?["durationTick"]?.Value ?? "0"),
            KeepCondition = (EffectKeepCondition) int.Parse(parentNode?.Attributes?["keepCondition"]?.Value ?? "0"),
            IntervalTick = int.Parse(parentNode?.Attributes?["intervalTick"]?.Value ?? "0"),
            BuffType = (BuffType) int.Parse(parentNode?.Attributes?["buffType"]?.Value ?? "0"),
            BuffSubType = (BuffSubType) int.Parse(parentNode?.Attributes?["buffSubType"]?.Value ?? "0"),
            CooldownTime = float.Parse(parentNode?.Attributes?["coolDownTime"]?.Value ?? "0"),
            DelayTick = int.Parse(parentNode?.Attributes?["delayTick"]?.Value ?? "0"),
            UseInGameTime = int.Parse(parentNode?.Attributes?["useInGameTime"]?.Value ?? "0") == 1,
            InvokeEvent = int.Parse(parentNode?.Attributes?["invokeEvent"]?.Value ?? "0") == 1,
            DeadKeepEffect = int.Parse(parentNode?.Attributes?["deadKeepEffect"]?.Value ?? "0") == 1,
            LogoutClearEffect = int.Parse(parentNode?.Attributes?["logoutClearEffect"]?.Value ?? "0") == 1,
            LeaveFieldClearEffect = int.Parse(parentNode?.Attributes?["leaveFieldClearEffect"]?.Value ?? "0") == 1,
            CasterIndividualEffect = (CasterIndividualEffect) int.Parse(parentNode?.Attributes?["casterIndividualEffect"]?.Value ?? "0"),
            ClearDistanceFromCaster = float.Parse(parentNode?.Attributes?["clearDistanceFromCaster"]?.Value ?? "0"),
            ClearEffectFromPvpZone = int.Parse(parentNode?.Attributes?["clearEffectFromPVPZone"]?.Value ?? "0") == 1,
            DoNotClearEffectFromEnterPvpZone = int.Parse(parentNode?.Attributes?["doNotClearEffectFromEnterPVPZone"]?.Value ?? "0") == 1,
            ClearCooldownFromPvpZone = int.Parse(parentNode?.Attributes?["clearCooldownFromPVPZone"]?.Value ?? "0") == 1,
            AllowedSkillAttacks = parentNode?.Attributes?["attackPossibleSkillIDs"]?.Value?.SplitAndParseToInt(',')?.ToArray() ?? Array.Empty<int>(),
            AllowedDotEffectAttacks = parentNode?.Attributes?["attackPossibleDotEffectIDs"]?.Value?.SplitAndParseToInt(',')?.ToArray() ?? Array.Empty<int>()
        };
    }

    private EffectCancelEffectMetadata ParseCancelEffect(XmlNode? parentNode)
    {
        return new()
        {
            CancelCheckSameCaster = int.Parse(parentNode?.Attributes?["cancelCheckSameCaster"]?.Value ?? "0") == 1,
            CancelPassiveEffect = int.Parse(parentNode?.Attributes?["cancelPassiveEffect"]?.Value ?? "0") == 1,
            CancelEffectCodes = parentNode?.Attributes?["cancelEffectCodes"]?.Value?.SplitAndParseToInt(',')?.ToArray() ?? Array.Empty<int>(),
            CancelBuffCategories = parentNode?.Attributes?["cancelBuffCategories"]?.Value?.SplitAndParseToInt(',')?.ToArray() ?? Array.Empty<int>()
        };
    }

    private EffectImmuneEffectMetadata ParseImmuneEffect(XmlNode? parentNode)
    {
        return new()
        {
            ImmuneEffectCodes = parentNode?.Attributes?["immuneEffectCodes"]?.Value?.SplitAndParseToInt(',')?.ToArray() ?? Array.Empty<int>(),
            ImmuneBuffCategories = parentNode?.Attributes?["immuneBuffCategories"]?.Value?.SplitAndParseToInt(',')?.ToArray() ?? Array.Empty<int>()
        };
    }

    private EffectResetSkillCooldownTimeMetadata ParseResetCoolDown(XmlNode? parentNode)
    {
        return new()
        {
            SkillCodes = parentNode?.Attributes?["skillCodes"]?.Value?.SplitAndParseToLong(',')?.ToArray() ?? Array.Empty<long>()
        };
    }

    private EffectModifyDurationMetadata ParseModifyEffectDuration(XmlNode? parentNode)
    {
        return new()
        {
            EffectCodes = parentNode?.Attributes?["effectCodes"]?.Value?.SplitAndParseToInt(',')?.ToArray() ?? Array.Empty<int>(),
            DurationFactors = parentNode?.Attributes?["durationFactors"]?.Value?.SplitAndParseToFloat(',')?.ToArray() ?? Array.Empty<float>(),
            DurationValues = parentNode?.Attributes?["durationValues"]?.Value?.SplitAndParseToFloat(',')?.ToArray() ?? Array.Empty<float>()
        };
    }

    private EffectModifyOverlapCountMetadata? ParseModifyOverlapCount(XmlNode? parentNode)
    {
        if (parentNode is null)
        {
            return null;
        }

        return new()
        {
            EffectCodes = parentNode.Attributes?["effectCodes"]?.Value?.SplitAndParseToInt(',')?.ToArray() ?? Array.Empty<int>(),
            OffsetCounts = parentNode.Attributes?["offsetCounts"]?.Value?.SplitAndParseToInt(',')?.ToArray() ?? Array.Empty<int>()
        };
    }

    private EffectOffensiveMetadata ParseOffensive(XmlNode? parentNode, Dictionary<StatAttribute, EffectStatMetadata> stats)
    {
        if (parentNode is not null)
        {
            ParseStatNode(parentNode, stats, "DamageV", "DamageR");
        }

        return new()
        {
            AlwaysCrit = int.Parse(parentNode?.Attributes?["attackSuccessCritical"]?.Value ?? "0") == 1,
            ImmuneBreak = int.Parse(parentNode?.Attributes?["hitImmuneBreak"]?.Value ?? "0")
        };
    }

    private EffectDefesiveMetadata ParseDefensive(XmlNode? parentNode, Dictionary<StatAttribute, EffectStatMetadata> stats)
    {
        return new()
        {
            Invincible = int.Parse(parentNode?.Attributes?["invincible"]?.Value ?? "0") == 1
        };
    }

    private EffectRecoveryMetadata ParseRecovery(XmlNode? parentNode)
    {
        return new()
        {
            RecoveryRate = float.Parse(parentNode?.Attributes?["RecoveryRate"]?.Value ?? "0"),
            HpRate = float.Parse(parentNode?.Attributes?["hpRate"]?.Value ?? "0"),
            HpValue = long.Parse(parentNode?.Attributes?["hpValue"]?.Value ?? "0"),
            SpRate = float.Parse(parentNode?.Attributes?["spRate"]?.Value ?? "0"),
            SpValue = long.Parse(parentNode?.Attributes?["spValue"]?.Value ?? "0"),
            EpRate = float.Parse(parentNode?.Attributes?["epRate"]?.Value ?? "0"),
            EpValue = long.Parse(parentNode?.Attributes?["epValue"]?.Value ?? "0")
        };
    }

    private EffectDotDamageMetadata ParseDotDamage(XmlNode? parentNode)
    {
        return new()
        {
            DamageType = byte.Parse(parentNode?.Attributes?["type"]?.Value ?? "0"),
            Rate = float.Parse(parentNode?.Attributes?["rate"]?.Value ?? "0"),
            Value = long.Parse(parentNode?.Attributes?["value"]?.Value ?? "0"),
            Element = int.Parse(parentNode?.Attributes?["element"]?.Value ?? "0"),
            UseGrade = int.Parse(parentNode?.Attributes?["useGrade"]?.Value ?? "0") == 1,
            DamageByTargetMaxHp = double.Parse(parentNode?.Attributes?["damageByTargetMaxHP"]?.Value ?? "0")
        };
    }

    private EffectInvokeMetadata ParseInvokeEffect(XmlNode? parentNode)
    {
        return new()
        {
            Values = parentNode?.Attributes?["values"]?.Value?.SplitAndParseToFloat(',')?.ToArray() ?? Array.Empty<float>(),
            Rates = parentNode?.Attributes?["rates"]?.Value?.SplitAndParseToFloat(',')?.ToArray() ?? Array.Empty<float>(),
            Types = parentNode?.Attributes?["types"]?.Value?.SplitAndParseToInt(',')?.Select(x => (InvokeEffectType) x)?.ToArray() ?? Array.Empty<InvokeEffectType>(),
            EffectId = int.Parse(parentNode?.Attributes?["effectID"]?.Value ?? "0"),
            EffectGroupId = int.Parse(parentNode?.Attributes?["effectGroupID"]?.Value ?? "0"),
            SkillId = int.Parse(parentNode?.Attributes?["skillID"]?.Value ?? "0"),
            SkillGroupId = int.Parse(parentNode?.Attributes?["skillGroupID"]?.Value ?? "0")
        };
    }

    private EffectRideMetadata? ParseRide(XmlNode? parentNode)
    {
        if (parentNode is null)
        {
            return null;
        }

        return new()
        {
            RideId = int.Parse(parentNode?.Attributes?["rideeID"]?.Value ?? "0")
        };
    }
    private EffectShieldMetadata? ParseShield(XmlNode? parentNode)
    {
        if (parentNode is null)
        {
            return null;
        }

        return new()
        {
            HpValue = int.Parse(parentNode?.Attributes?["hpValue"]?.Value ?? "0")
        };
    }

    private Dictionary<string, bool> FoundStat = new();

    private void ParseStatNode(XmlNode statNode, Dictionary<StatAttribute, EffectStatMetadata> stats, string valueSuffix = "value", string rateSuffix = "rate", bool checkForUnknownValues = true)
    {
        if (statNode.Attributes is null)
        {
            return;
        }

        foreach (XmlAttribute attribute in statNode.Attributes)
        {
            string name = attribute.Name;

            bool isValue = false;
            bool foundSuffix = false;

            if (name.EndsWith(valueSuffix))
            {
                isValue = true;
                foundSuffix = true;
                name = name.Substring(0, name.Length - valueSuffix.Length);
            }

            if (name.EndsWith(rateSuffix))
            {
                foundSuffix = true;
                name = name.Substring(0, name.Length - rateSuffix.Length);
            }

            StatEntry? entry = StatEntry.Entries.GetValueOrDefault(name, null);

            if (entry is null)
            {
                if (foundSuffix && checkForUnknownValues && !FoundStat.ContainsKey(name))
                {
                    FoundStat.Add(name, true);
                    foundSuffix = true;
                }

                continue;
            }

            StatEntry.AddStat(stats, attribute, entry, isValue);
        }
    }
}
