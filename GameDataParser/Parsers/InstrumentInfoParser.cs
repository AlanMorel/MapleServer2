using System.Collections.Generic;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class InstrumentInfoParser : Exporter<List<InsturmentInfoMetadata>>
    {
        public InstrumentInfoParser(MetadataResources resources) : base(resources, "instrument-info") { }

        protected override List<InsturmentInfoMetadata> Parse()
        {
            List<InsturmentInfoMetadata> instrument = new List<InsturmentInfoMetadata>();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {

                if (!entry.Name.StartsWith("table/instrumentinfo"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    InsturmentInfoMetadata metadata = new InsturmentInfoMetadata();

                    if (node.Name == "instrument")
                    {
                        metadata.InstrumentId = byte.Parse(node.Attributes["id"].Value);
                        metadata.Category = byte.Parse(node.Attributes["category"].Value);
                        metadata.ScoreCount = byte.Parse(node.Attributes["soloRelayScoreCount"].Value);
                        instrument.Add(metadata);
                    }
                }
            }
            return instrument;
        }
    }
}

