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
                    AddStat(level.Status, StatAttribute.Str, stat.strvalue);
                    AddStat(level.Status, StatAttribute.CritDamage, stat.cadvalue);
                    AddStat(level.Status, StatAttribute.Accuracy, stat.atpvalue);
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

        status.Stats.Add(stat, new()
        {
            Rate = value,
            AttributeType = StatAttributeType.Rate
        });
    }

    private void AddStat(EffectStatusMetadata status, StatAttribute stat, long value)
    {
        if (value == 0)
        {
            return;
        }

        status.Stats.Add(stat, new()
        {
            Flat = value,
            AttributeType = StatAttributeType.Flat
        });
    }
}
