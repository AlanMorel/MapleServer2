using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
using GameDataParser.Tools;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class FishParser : Exporter<List<FishMetadata>>
{
    public FishParser(MetadataResources resources) : base(resources, MetadataName.Fish) { }

    protected override List<FishMetadata> Parse()
    {
        Dictionary<int, List<int>> fishHabitat = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.Equals("table/fishhabitat.xml"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList? nodes = document.SelectNodes("/ms2/fish");
            if (nodes is null)
            {
                continue;
            }

            foreach (XmlNode node in nodes)
            {
                if (ParserHelper.CheckForNull(node, "id", "habitat"))
                {
                    continue;
                }

                int fishId = int.Parse(node.Attributes!["id"]!.Value);
                List<int> habitat = node.Attributes["habitat"]!.Value.SplitAndParseToInt(',').ToList();

                if (fishHabitat.ContainsKey(fishId))
                {
                    fishHabitat[fishId].AddRange(habitat);
                }
                else
                {
                    fishHabitat[fishId] = new(habitat);
                }
            }
        }

        List<FishMetadata> fishes = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.Equals("table/fish.xml"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList? childNodes = document.DocumentElement?.ChildNodes;
            if (childNodes is null)
            {
                continue;
            }

            foreach (XmlNode fishnode in childNodes)
            {
                if (fishnode.Name != "fish")
                {
                    continue;
                }

                if (ParserHelper.CheckForNull(fishnode, "id", "habitat"))
                {
                    continue;
                }

                FishMetadata metadata = new()
                {
                    Id = int.Parse(fishnode.Attributes!["id"]!.Value),
                    Habitat = fishnode.Attributes["habitat"]!.Value,
                    CompanionId = int.Parse(fishnode.Attributes["companion"]?.Value ?? "0"),
                    Mastery = short.Parse(fishnode.Attributes["fishMastery"]?.Value ?? "0"),
                    Rarity = byte.Parse(fishnode.Attributes["rank"]!.Value),
                    SmallSize = fishnode.Attributes["smallSize"]!.Value.SplitAndParseToShort('-').ToArray(),
                    BigSize = fishnode.Attributes["bigSize"]!.Value.SplitAndParseToShort('-').ToArray()
                };

                if (fishnode.Attributes["ignoreSpotMastery"]?.Value != null)
                {
                    byte ignoreSpotMastery = byte.Parse(fishnode.Attributes["ignoreSpotMastery"]!.Value);
                    if (ignoreSpotMastery == 1)
                    {
                        metadata.IgnoreMastery = true;
                    }
                }

                if (fishHabitat.ContainsKey(metadata.Id))
                {
                    metadata.HabitatMapId = fishHabitat[metadata.Id];
                }

                fishes.Add(metadata);
            }
        }

        return fishes;
    }
}
