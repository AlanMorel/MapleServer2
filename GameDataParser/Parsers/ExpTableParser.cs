using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

internal class ExpParser : Exporter<List<ExpMetadata>>
{
    public ExpParser(MetadataResources resources) : base(resources, MetadataName.ExpTable) { }

    protected override List<ExpMetadata> Parse()
    {
        List<ExpMetadata> expList = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/nextexp"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList? nodes = document.SelectNodes("/ms2/exp");
            if (nodes is null)
            {
                continue;
            }

            foreach (XmlNode node in nodes)
            {
                if (ParserHelper.CheckForNull(node, "level", "value"))
                {
                    continue;
                }

                byte level = byte.Parse(node.Attributes!["level"]!.Value);
                if (level == 0)
                {
                    continue;
                }

                ExpMetadata expTable = new()
                {
                    Level = level,
                    Experience = long.Parse(node.Attributes["value"]!.Value)
                };
                expList.Add(expTable);
            }
        }

        return expList;
    }
}
