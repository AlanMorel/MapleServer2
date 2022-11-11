using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class InstrumentCategoryInfoParser : Exporter<List<InstrumentCategoryInfoMetadata>>
{
    public InstrumentCategoryInfoParser(MetadataResources resources) : base(resources, MetadataName.InstrumentCategoryInfo) { }

    protected override List<InstrumentCategoryInfoMetadata> Parse()
    {
        List<InstrumentCategoryInfoMetadata> instrument = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/instrumentcategoryinfo"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList? nodes = document.SelectNodes("/ms2/category");
            if (nodes is null)
            {
                continue;
            }

            foreach (XmlNode node in nodes)
            {
                if (ParserHelper.CheckForNull(node, "id"))
                {
                    continue;
                }

                InstrumentCategoryInfoMetadata metadata = new()
                {
                    CategoryId = byte.Parse(node.Attributes!["id"]!.Value),
                    GMId = byte.Parse(node.Attributes["GMId"]?.Value ?? "0"),
                    Octave = node.Attributes["defaultOctave"]?.Value ?? "",
                    PercussionId = byte.Parse(node.Attributes["percussionId"]?.Value ?? "0")
                };

                instrument.Add(metadata);
            }
        }

        return instrument;
    }
}
