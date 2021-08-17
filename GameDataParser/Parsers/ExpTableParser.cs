using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    internal class ExpParser : Exporter<List<ExpMetadata>>
    {
        public ExpParser(MetadataResources resources) : base(resources, "exp") { }

        protected override List<ExpMetadata> Parse()
        {
            List<ExpMetadata> expList = new List<ExpMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {

                if (!entry.Name.StartsWith("table/nextexp"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    ExpMetadata expTable = new ExpMetadata();

                    if (node.Name == "exp")
                    {
                        byte level = byte.Parse(node.Attributes["level"].Value);
                        if (level != 0)
                        {
                            expTable.Level = level;
                            expTable.Experience = long.Parse(node.Attributes["value"].Value);
                            expList.Add(expTable);
                        }
                    }
                }
            }

            return expList;
        }
    }
}
