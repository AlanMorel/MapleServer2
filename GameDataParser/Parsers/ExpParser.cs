using System.Collections.Generic;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    class ExpParser : Exporter<List<ExpMetadata>>
    {
        public ExpParser(MetadataResources resources) : base(resources, "exp") { }

        protected override List<ExpMetadata> Parse()
        {
            List<ExpMetadata> expList = new List<ExpMetadata>();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {

                if (!entry.Name.StartsWith("table/nextexp"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
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
