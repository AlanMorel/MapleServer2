using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class InstrumentCategoryInfoParser : Exporter<List<InstrumentCategoryInfoMetadata>>
    {
        public InstrumentCategoryInfoParser(MetadataResources resources) : base(resources, "instrument-category-info") { }

        protected override List<InstrumentCategoryInfoMetadata> Parse()
        {
            List<InstrumentCategoryInfoMetadata> instrument = new List<InstrumentCategoryInfoMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.StartsWith("table/instrumentcategoryinfo"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                XmlNodeList nodes = document.SelectNodes("/ms2/category");

                foreach (XmlNode node in nodes)
                {
                    InstrumentCategoryInfoMetadata metadata = new InstrumentCategoryInfoMetadata();

                    metadata.CategoryId = byte.Parse(node.Attributes["id"].Value);
                    metadata.GMId = byte.Parse(node.Attributes["GMId"]?.Value ?? "0");
                    metadata.Octave = node.Attributes["defaultOctave"]?.Value ?? "";
                    metadata.PercussionId = byte.Parse(node.Attributes["percussionId"]?.Value ?? "0");

                    instrument.Add(metadata);
                }
            }
            return instrument;
        }
    }
}

