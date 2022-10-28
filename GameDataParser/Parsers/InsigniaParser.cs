using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class InsigniaParser : Exporter<List<InsigniaMetadata>>
{
    public InsigniaParser(MetadataResources resources) : base(resources, MetadataName.Insignia) { }

    protected override List<InsigniaMetadata> Parse()
    {
        List<InsigniaMetadata> insignias = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/nametagsymbol"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList? nodes = document.SelectNodes("/ms2/symbol");
            if (nodes is null)
            {
                continue;
            }

            foreach (XmlNode node in nodes)
            {
                if (ParserHelper.CheckForNull(node, "id", "conditionType"))
                {
                    continue;
                }

                InsigniaMetadata metadata = new()
                {
                    InsigniaId = short.Parse(node.Attributes!["id"]!.Value),
                    ConditionType = node.Attributes["conditionType"]!.Value
                };

                _ = int.TryParse(node.Attributes["code"]?.Value ?? "0", out metadata.TitleId);

                insignias.Add(metadata);
            }
        }

        return insignias;
    }
}
