using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class FishParser : Exporter<List<FishMetadata>>
    {
        public FishParser(MetadataResources resources) : base(resources, "fish") { }

        protected override List<FishMetadata> Parse()
        {
            Dictionary<int, List<int>> fishHabitat = new Dictionary<int, List<int>>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.Equals("table/fishhabitat.xml"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    if (node.Name == "fish")
                    {
                        List<int> habitat = new List<int>();
                        int fishId = int.Parse(node.Attributes["id"].Value);
                        if (node.Attributes["habitat"].Value.Contains(","))
                        {
                            habitat.AddRange(node.Attributes["habitat"].Value.Split(",").Select(int.Parse).ToList());
                        }
                        else
                        {
                            habitat.Add(string.IsNullOrEmpty(node.Attributes["habitat"].Value) ? 0 : int.Parse(node.Attributes["habitat"].Value));
                        }
                        if (fishHabitat.ContainsKey(fishId))
                        {
                            fishHabitat[fishId].AddRange(habitat);
                        }
                        else
                        {
                            fishHabitat[fishId] = new List<int>(habitat);
                        }
                    }
                }
            }

            List<FishMetadata> fishes = new List<FishMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.Equals("table/fish.xml"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                foreach (XmlNode fishnode in document.DocumentElement.ChildNodes)
                {
                    if (fishnode.Name == "fish")
                    {
                        FishMetadata metadata = new FishMetadata();

                        metadata.Id = int.Parse(fishnode.Attributes["id"].Value);
                        metadata.Habitat = fishnode.Attributes["habitat"].Value;
                        if (fishnode.Attributes["companion"] != null)
                        {
                            metadata.CompanionId = int.Parse(fishnode.Attributes["companion"].Value);
                        }
                        metadata.Mastery = string.IsNullOrEmpty(fishnode.Attributes["fishMastery"]?.Value) ? (short) 0 : short.Parse(fishnode.Attributes["fishMastery"].Value);
                        metadata.Rarity = byte.Parse(fishnode.Attributes["rank"].Value);
                        metadata.SmallSize = Array.ConvertAll(fishnode.Attributes["smallSize"].Value.Split("-"), short.Parse);
                        metadata.BigSize = Array.ConvertAll(fishnode.Attributes["bigSize"].Value.Split("-"), short.Parse);
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
            }
            return fishes;
        }
    }
}
