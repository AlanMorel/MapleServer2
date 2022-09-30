using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Tools;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class SetItemInfoParser : Exporter<List<SetItemInfoMetadata>>
{
    public SetItemInfoParser(MetadataResources resources) : base(resources, MetadataName.SetItemInfo) { }

    protected override List<SetItemInfoMetadata> Parse()
    {
        List<SetItemInfoMetadata> sets = new();

        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/setiteminfo"))
            {
                continue;
            }

            XmlDocument innerDocument = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList nodeList = innerDocument.SelectNodes("/ms2/set");
            foreach (XmlNode node in nodeList)
            {
                int id = int.Parse(node.Attributes["id"].Value);
                int[] itemIds = node.Attributes["itemIDs"]?.Value?.SplitAndParseToInt(',')?.ToArray() ?? Array.Empty<int>();
                int optionId = int.Parse(node.Attributes["optionID"].Value);

                sets.Add(new()
                {
                    Id = id,
                    ItemIds = itemIds,
                    OptionId = optionId,
                });
            }
        }

        return sets;
    }
}
