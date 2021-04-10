using System.Collections.Generic;
using System.Linq;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class FishingSpotParser : Exporter<List<FishingSpotMetadata>>
    {
        public FishingSpotParser(MetadataResources resources) : base(resources, "fishing-spot") { }

        protected override List<FishingSpotMetadata> Parse()
        {
            List<FishingSpotMetadata> spots = new List<FishingSpotMetadata>();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {
                if (!entry.Name.StartsWith("table/fishingspot"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    if (node.Name == "spot")
                    {
                        FishingSpotMetadata metadata = new FishingSpotMetadata();

                        metadata.Id = int.Parse(node.Attributes["id"].Value);
                        metadata.MinMastery = short.Parse(node.Attributes["minMastery"].Value);
                        metadata.MaxMastery = short.Parse(node.Attributes["maxMastery"].Value);
                        metadata.LiquidType = node.Attributes["liquidType"].Value.Split(",").ToList();

                        spots.Add(metadata);
                    }
                }
            }
            return spots;
        }
    }
}
