using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class FishingRodParser : Exporter<List<FishingRodMetadata>>
    {
        public FishingRodParser(MetadataResources resources) : base(resources, "fishing-rod") { }

        protected override List<FishingRodMetadata> Parse()
        {
            List<FishingRodMetadata> rods = new List<FishingRodMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.StartsWith("table/fishingrod"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                XmlNodeList nodes = document.SelectNodes("/ms2/rod");

                foreach (XmlNode node in nodes)
                {
                    FishingRodMetadata metadata = new FishingRodMetadata();

                    metadata.RodId = int.Parse(node.Attributes["rodCode"].Value);
                    metadata.ItemId = int.Parse(node.Attributes["itemCode"].Value);
                    metadata.MasteryLimit = short.Parse(node.Attributes["fishMasteryLimit"].Value);
                    metadata.ReduceTime = int.Parse(node.Attributes["reduceFishingTime"].Value);

                    rods.Add(metadata);
                }
            }
            return rods;
        }
    }
}
