using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Tools;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class CharacterCreateParser : Exporter<List<CharacterCreateMetadata>>
{
    public CharacterCreateParser(MetadataResources resources) : base(resources, MetadataName.CharacterCreate) { }

    protected override List<CharacterCreateMetadata> Parse()
    {
        List<CharacterCreateMetadata> items = new();

        PackFileEntry? entry = Resources.XmlReader.Files.FirstOrDefault(x => x.Name.StartsWith("table/charactercreateselect"));
        if (entry is null)
        {
            return items;
        }

        // Parse XML
        XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
        XmlNodeList? nodes = document.SelectNodes("/ms2/group");
        if (nodes is null)
        {
            return items;
        }

        foreach (XmlNode node in nodes)
        {
            if (node.Attributes?["name"]?.Value != "display" || node.Attributes["locale"]?.Value != null)
            {
                continue;
            }

            foreach (XmlNode subnode in node)
            {
                if (subnode.Attributes?["feature"]?.Value != "SoulBinder") // get the latest entry being used
                {
                    continue;
                }

                List<int>? disabledJobs = subnode.Attributes["disableJobCode"]?.Value.SplitAndParseToInt(',').ToList();
                CharacterCreateMetadata metadata = new()
                {
                    DisabledJobs = disabledJobs
                };
                items.Add(metadata);
            }

        }
        return items;
    }
}
