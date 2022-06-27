using System.Web;
using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2.File.Parser.Tools;
using Maple2.File.Parser.Xml.AdditionalEffect;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Enums;

namespace GameDataParser.Parsers;

public class AdditionalEffectParser : Exporter<List<AdditionalEffectMetadata>>
{
    public AdditionalEffectParser(MetadataResources resources) : base(resources, MetadataName.AdditionalEffect) { }

    protected override List<AdditionalEffectMetadata> Parse()
    {
        List<AdditionalEffectMetadata> effects = new();

        Filter.Load(Resources.XmlReader, "NA", "Live");
        Maple2.File.Parser.AdditionalEffectParser parser = new(Resources.XmlReader);
        foreach ((int id, IList<AdditionalEffectData> data) in parser.Parse())
        {
            AdditionalEffectMetadata metadata = new()
            {
                Id = id,
                Levels = new()
            };

            for (int i = 0; i < data.Count; ++i)
            {
                int levelIndex = data[i].BasicProperty.level;

                AdditionalEffectLevelMetadata level = new()
                {
                    Basic = new()
                    {
                        MaxBuffCount = data[i].BasicProperty.maxBuffCount,
                    },
                    Status = new()
                    {
                        Stats = new()
                    }
                };

                Stat stat = data[i].StatusProperty.Stat;

                if (stat != null)
                {
                    AddStat(level.Status, StatAttribute.Str, stat.strrate);
                    AddStat(level.Status, StatAttribute.Str, stat.strvalue);
                    AddStat(level.Status, StatAttribute.Int, stat.intrate);
                    AddStat(level.Status, StatAttribute.Int, stat.intvalue);
                    AddStat(level.Status, StatAttribute.Luk, stat.lukrate);
                    AddStat(level.Status, StatAttribute.Luk, stat.lukvalue);
                    AddStat(level.Status, StatAttribute.Dex, stat.dexrate);
                    AddStat(level.Status, StatAttribute.Dex, stat.dexvalue);
                    AddStat(level.Status, StatAttribute.Hp, stat.hprate);
                    AddStat(level.Status, StatAttribute.Hp, stat.hpvalue);
                    AddStat(level.Status, StatAttribute.HpRegen, stat.hp_rgprate);               // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.HpRegen, stat.hp_rgpvalue);              // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.HpRegenInterval, stat.hp_invrate);       // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.HpRegenInterval, stat.hp_invvalue);      // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.Spirit, stat.sprate);                    // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.Spirit, stat.spvalue);                   // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.SpRegen, stat.sp_rgprate);               // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.SpRegen, stat.sp_rgpvalue);              // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.SpRegenInterval, stat.sp_invrate);       // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.SpRegenInterval, stat.sp_invvalue);      // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.Stamina, stat.eprate);                   // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.Stamina, stat.epvalue);                  // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.StaminaRegen, stat.ep_rgprate);          // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.StaminaRegen, stat.ep_rgpvalue);         // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.StaminaRegenInterval, stat.ep_invrate);  // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.StaminaRegenInterval, stat.ep_invvalue); // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.AttackSpeed, stat.asprate);
                    AddStat(level.Status, StatAttribute.AttackSpeed, stat.aspvalue);
                    AddStat(level.Status, StatAttribute.MovementSpeed, stat.msprate);
                    AddStat(level.Status, StatAttribute.MovementSpeed, stat.mspvalue);
                    AddStat(level.Status, StatAttribute.Accuracy, stat.atprate);
                    AddStat(level.Status, StatAttribute.Accuracy, stat.atpvalue);
                    AddStat(level.Status, StatAttribute.Evasion, stat.evprate);
                    AddStat(level.Status, StatAttribute.Evasion, stat.evpvalue);
                    AddStat(level.Status, StatAttribute.CritRate, stat.caprate);
                    AddStat(level.Status, StatAttribute.CritRate, stat.capvalue);
                    AddStat(level.Status, StatAttribute.CritDamage, stat.cadrate);
                    AddStat(level.Status, StatAttribute.CritDamage, stat.cadvalue);
                    AddStat(level.Status, StatAttribute.CritEvasion, stat.carrate);
                    AddStat(level.Status, StatAttribute.CritEvasion, stat.carvalue);
                    AddStat(level.Status, StatAttribute.BonusAtk, stat.baprate);
                    AddStat(level.Status, StatAttribute.BonusAtk, stat.bapvalue);
                    AddStat(level.Status, StatAttribute.MountMovementSpeed, stat.rmsprate);
                    AddStat(level.Status, StatAttribute.MountMovementSpeed, stat.rmspvalue);
                    AddStat(level.Status, StatAttribute.Pierce, stat.penrate);
                    AddStat(level.Status, StatAttribute.Pierce, stat.penvalue);
                    AddStat(level.Status, StatAttribute.Damage, stat.dmgrate);
                    AddStat(level.Status, StatAttribute.Damage, stat.dmgvalue);
                    AddStat(level.Status, StatAttribute.MinWeaponAtk, stat.wapminrate);
                    AddStat(level.Status, StatAttribute.MinWeaponAtk, stat.wapminvalue);
                    AddStat(level.Status, StatAttribute.MaxWeaponAtk, stat.wapmaxrate);
                    AddStat(level.Status, StatAttribute.MaxWeaponAtk, stat.wapmaxvalue);
                    AddStat(level.Status, StatAttribute.MagicPiercing, stat.marrate);  // revisit these to make sure they're right
                    AddStat(level.Status, StatAttribute.MagicPiercing, stat.marvalue); // revisit these to make sure they're right
                    AddStat(level.Status, StatAttribute.MagicAtk, stat.maprate);
                    AddStat(level.Status, StatAttribute.MagicAtk, stat.mapvalue);
                    AddStat(level.Status, StatAttribute.PhysicalPiercing, stat.parrate);  // revisit these to make sure they're right
                    AddStat(level.Status, StatAttribute.PhysicalPiercing, stat.parvalue); // revisit these to make sure they're right
                    AddStat(level.Status, StatAttribute.PhysicalAtk, stat.paprate);
                    AddStat(level.Status, StatAttribute.PhysicalAtk, stat.papvalue);
                    AddStat(level.Status, StatAttribute.JumpHeight, stat.jmprate);
                    AddStat(level.Status, StatAttribute.JumpHeight, stat.jmpvalue);
                    AddStat(level.Status, StatAttribute.PerfectGuard, stat.abprate);
                    AddStat(level.Status, StatAttribute.PerfectGuard, stat.abpvalue);
                    AddStat(level.Status, StatAttribute.PetBonusAtk, stat.bap_petrate);
                    AddStat(level.Status, StatAttribute.PetBonusAtk, stat.bap_petvalue);
                }

                OffensiveProperty offensive = data[i].OffensiveProperty;

                if (offensive != null)
                {
                    AddStat(level.Status, StatAttribute.PhysicalAtk, offensive.papDamageV);
                    AddStat(level.Status, StatAttribute.PhysicalAtk, offensive.papDamageR);
                    AddStat(level.Status, StatAttribute.MagicAtk, offensive.mapDamageR);
                    AddStat(level.Status, StatAttribute.MagicAtk, offensive.mapDamageR);
                    //AddStat(level.Status, StatAttribute., offensive.attackSuccessCritical);
                    //AddStat(level.Status, StatAttribute., offensive.hitImmuneBreak);
                }

                DefensiveProperty defensive = data[i].DefensiveProperty;

                if (defensive != null)
                {
                    AddStat(level.Status, StatAttribute.PhysicalRes, defensive.papDamageV);
                    AddStat(level.Status, StatAttribute.PhysicalRes, defensive.papDamageR);
                    AddStat(level.Status, StatAttribute.MagicRes, defensive.mapDamageR);
                    AddStat(level.Status, StatAttribute.MagicRes, defensive.mapDamageR);
                    //AddStat(level.Status, StatAttribute., defensive.invincible);
                }



                metadata.Levels.Add(levelIndex, level);
            }

            effects.Add(metadata);
        }

        return effects;
    }

    private void AddStat(EffectStatusMetadata status, StatAttribute stat, float value)
    {
        if (value == 0)
        {
            return;
        }

        EffectStatMetadata currentValue;

        if (status.Stats.TryGetValue(stat, out currentValue))
        {
            currentValue.Rate = value;
        }
        else
        {
            status.Stats.Add(stat, new()
            {
                Rate = value,
                AttributeType = StatAttributeType.Rate
            });
        }
    }

    private void AddStat(EffectStatusMetadata status, StatAttribute stat, long value)
    {
        if (value == 0)
        {
            return;
        }

        EffectStatMetadata currentValue;

        if (status.Stats.TryGetValue(stat, out currentValue))
        {
            currentValue.Flat = value;
        }
        else
        {
            status.Stats.Add(stat, new()
            {
                Flat = value,
                AttributeType = StatAttributeType.Flat
            });
        }
    }
}
