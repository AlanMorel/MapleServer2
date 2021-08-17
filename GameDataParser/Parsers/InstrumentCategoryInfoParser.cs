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
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    InstrumentCategoryInfoMetadata metadata = new InstrumentCategoryInfoMetadata();

                    if (node.Name == "category")
                    {
                        metadata.CategoryId = byte.Parse(node.Attributes["id"].Value);

                        if (node.Attributes["GMId"] != null)
                        {
                            metadata.GMId = byte.Parse(node.Attributes["GMId"].Value);
                        }

                        if (node.Attributes["defaultOctave"] != null)
                        {
                            metadata.Octave = node.Attributes["defaultOctave"].Value;
                        }

                        if (node.Attributes["percussionId"] != null)
                        {
                            metadata.PercussionId = byte.Parse(node.Attributes["percussionId"].Value);
                        }

                        instrument.Add(metadata);
                    }
                }
            }
            return instrument;
        }
    }
}

