using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class MasteryUgcHousingParser : Exporter<List<MasteryUgcHousingMetadata>>
{
    public MasteryUgcHousingParser(MetadataResources resources) : base(resources, MetadataName.MasteryUGCHousing) { }

    protected override List<MasteryUgcHousingMetadata> Parse()
    {
        List<MasteryUgcHousingMetadata> metadataList = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/masteryugchousing"))
            {
                continue;
            }

            XmlNodeList document = Resources.XmlReader.GetXmlDocument(entry).GetElementsByTagName("v");
            foreach (XmlNode node in document)
            {
                MasteryUgcHousingMetadata metadata = new()
                {
                    Grade = byte.Parse(node.Attributes["grade"].Value),
                    MasteryRequired = short.Parse(node.Attributes["value"].Value)
                };

                _ = int.TryParse(node.Attributes["rewardJobItemID"]?.Value ?? "0", out metadata.ItemId);

                metadataList.Add(metadata);
            }
        }

        return metadataList;
    }
}
