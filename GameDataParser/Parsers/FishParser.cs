using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class FishParser : Exporter<List<FishMetadata>>
{
    public FishParser(MetadataResources resources) : base(resources, "fish") { }

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
            XmlNodeList nodes = document.SelectNodes("/ms2/fish");

            foreach (XmlNode node in nodes)
            {
                int fishId = int.Parse(node.Attributes["id"].Value);

                string habitatString = node.Attributes["habitat"]?.Value;
                if (string.IsNullOrEmpty(habitatString))
                {
                    continue;
                }

                List<int> habitat = habitatString.Split(",").Select(int.Parse).ToList();

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
            foreach (XmlNode fishnode in document.DocumentElement.ChildNodes)
            {
                if (fishnode.Name != "fish")
                {
                    continue;
                }

                FishMetadata metadata = new();

                metadata.Id = int.Parse(fishnode.Attributes["id"].Value);
                metadata.Habitat = fishnode.Attributes["habitat"].Value;
                metadata.CompanionId = int.Parse(fishnode.Attributes["companion"]?.Value ?? "0");

                metadata.Mastery = short.Parse(fishnode.Attributes["fishMastery"]?.Value ?? "0");
                metadata.Rarity = byte.Parse(fishnode.Attributes["rank"].Value);
                metadata.SmallSize = fishnode.Attributes["smallSize"].Value.Split("-").Select(short.Parse).ToArray();
                metadata.BigSize = fishnode.Attributes["bigSize"].Value.Split("-").Select(short.Parse).ToArray();

                if (fishnode.Attributes["ignoreSpotMastery"] != null)
                {
                    byte ignoreSpotMastery = byte.Parse(fishnode.Attributes["ignoreSpotMastery"].Value);
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
