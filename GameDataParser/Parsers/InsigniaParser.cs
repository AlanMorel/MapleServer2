using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class InsigniaParser : Exporter<List<InsigniaMetadata>>
    {
        public InsigniaParser(MetadataResources resources) : base(resources, "insignia") { }

        protected override List<InsigniaMetadata> Parse()
        {
            List<InsigniaMetadata> insignias = new List<InsigniaMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.StartsWith("table/nametagsymbol"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                XmlNodeList nodes = document.SelectNodes("/ms2/symbol");

                foreach (XmlNode node in nodes)
                {
                    InsigniaMetadata metadata = new InsigniaMetadata();

                    metadata.InsigniaId = short.Parse(node.Attributes["id"].Value);
                    metadata.ConditionType = node.Attributes["conditionType"].Value;
                    _ = int.TryParse(node.Attributes["code"]?.Value ?? "0", out metadata.TitleId);

                    insignias.Add(metadata);
                }
            }

            return insignias;
        }
    }
}
