using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Tools;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class SetItemOptionParser : Exporter<List<SetItemOptionMetadata>>
{
    public SetItemOptionParser(MetadataResources resources) : base(resources, MetadataName.SetItemOption) { }
    private static readonly List<string> Properties = new() { "count", "additionalEffectID", "additionalEffectLevel", "animationPrefix", "setEffect", "groundAttribute" };

    protected override List<SetItemOptionMetadata> Parse()
    {
        List<SetItemOptionMetadata> options = new();

        foreach (PackFileEntry fileEntry in Resources.XmlReader.Files)
        {
            if (!fileEntry.Name.StartsWith("table/setitemoption"))
            {
                continue;
            }

            XmlDocument innerDocument = Resources.XmlReader.GetXmlDocument(fileEntry);
            XmlNodeList nodeList = innerDocument.SelectNodes("/ms2/option");

            foreach (XmlNode node in nodeList)
            {
                int id = int.Parse(node.Attributes["id"].Value);
                List<SetBonusMetadata> bonuses = new();
                SetItemOptionMetadata option = new()
                {
                    Id = id,
                    Parts = bonuses
                };

                foreach (XmlNode partNode in node.SelectNodes("part"))
                {
                    int count = int.Parse(partNode.Attributes["count"].Value);
                    int[] additionalEffectID = partNode.Attributes["additionalEffectID"]?.Value?.SplitAndParseToInt(',')?.ToArray() ?? Array.Empty<int>();
                    int[] additionalEffectLevel = partNode.Attributes["additionalEffectLevel"]?.Value?.SplitAndParseToInt(',')?.ToArray() ?? Array.Empty<int>();
                    Dictionary<StatAttribute, EffectStatMetadata> stats = new();

                    SetBonusMetadata setBonus = new()
                    {
                        Count = count,
                        AdditionalEffectIds = additionalEffectID,
                        AdditionalEffectLevels = additionalEffectLevel,
                        Stats = stats
                    };

                    foreach (XmlAttribute attribute in partNode.Attributes)
                    {
                        string name = attribute.Name;

                        if (name.EndsWith("_base"))
                        {
                            name = name.Substring(0, name.Length - 5);

                            bool isValue = false;

                            if (name.EndsWith("_value"))
                            {
                                isValue = true;
                                name = name.Substring(0, name.Length - 6);
                            }

                            if (name.EndsWith("_rate"))
                            {
                                name = name.Substring(0, name.Length - 5);
                            }

                            StatEntry? entry = StatEntry.Entries.GetValueOrDefault(name, null);

                            if (entry is null)
                            {
                                continue;
                            }

                            StatEntry.AddStat(setBonus.Stats, attribute, entry, isValue);
                        }
                    }

                    bonuses.Add(setBonus);
                }

                options.Add(option);
            }
        }

        return options;
    }

    private void AddStat(SetBonusMetadata setBonus, XmlAttribute attribute, StatEntry entry, bool isValue)
    {
        StatAttribute stat = entry.Attribute;

        if (entry.Name == "sgi")
        {
            stat = StatAttribute.EliteDamage;
        }

        if (entry.Name == "sgi_boss")
        {
            stat = StatAttribute.BossDamage;
        }

        if (entry.AttributeType == StatAttributeType.Flat)
        {
            long flat = 0;
            float rate = 0;

            if (isValue)
            {
                flat = long.Parse(attribute.Value);
            }
            else
            {
                rate = float.Parse(attribute.Value);
            }

            AddStatFlat(setBonus, stat, flat, rate);
        }

        if (entry.AttributeType == StatAttributeType.Rate)
        {
            float value = float.Parse(attribute.Value);

            AddStatRate(setBonus, stat, isValue ? value : 0, isValue ? 0 : value);
        }
    }

    private void AddStatRate(SetBonusMetadata setBonus, StatAttribute stat, float flat, float rate)
    {
        if (flat == 0 && rate == 0)
        {
            return;
        }

        EffectStatMetadata currentValue;

        if (setBonus.Stats.TryGetValue(stat, out currentValue))
        {
            currentValue.Flat += (long) (flat * 1000);
            currentValue.Rate += rate;

            return;
        }

        setBonus.Stats.Add(stat, new()
        {
            Flat = (long) (flat * 1000),
            Rate = rate,
            AttributeType = StatAttributeType.Rate
        });
    }

    private void AddStatFlat(SetBonusMetadata setBonus, StatAttribute stat, long flat, float rate)
    {
        if (flat == 0 && rate == 0)
        {
            return;
        }

        EffectStatMetadata currentValue;

        if (setBonus.Stats.TryGetValue(stat, out currentValue))
        {
            currentValue.Flat += flat;
            currentValue.Rate += rate;

            return;
        }

        setBonus.Stats.Add(stat, new()
        {
            Flat = flat,
            Rate = rate,
            AttributeType = StatAttributeType.Flat
        });
    }
}
