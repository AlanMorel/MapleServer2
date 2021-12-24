using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class InstrumentInfoParser : Exporter<List<InstrumentInfoMetadata>>
{
    public InstrumentInfoParser(MetadataResources resources) : base(resources, "instrument-info") { }

    protected override List<InstrumentInfoMetadata> Parse()
    {
        List<InstrumentInfoMetadata> instrument = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/instrumentinfo"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList nodes = document.SelectNodes("/ms2/instrument");

            foreach (XmlNode node in nodes)
            {
                InstrumentInfoMetadata metadata = new()
                {
                    InstrumentId = byte.Parse(node.Attributes["id"].Value),
                    Category = byte.Parse(node.Attributes["category"].Value),
                    ScoreCount = byte.Parse(node.Attributes["soloRelayScoreCount"].Value)
                };

                instrument.Add(metadata);
            }
        }

        return instrument;
    }
}
