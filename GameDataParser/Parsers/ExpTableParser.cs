using System.Collections.Generic;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    class ExpTableParser : Exporter<List<ExpMetadata>>
    {
        public ExpTableParser(MetadataResources resources) : base(resources, "exptable") { }

        protected override List<ExpMetadata> parse()
        {
            List<ExpMetadata> expList = new List<ExpMetadata>();
            foreach (PackFileEntry entry in resources.xmlFiles)
            {

                if (!entry.Name.StartsWith("table/nextexp"))
                {
                    continue;
                }

                XmlReader reader = resources.xmlMemFile.GetReader(entry.FileHeader);
                while (reader.Read())
                {
                    ExpMetadata expTable = new ExpMetadata();
                    if (reader.NodeType != XmlNodeType.Element)
                    {
                        continue;
                    }

                    if (reader.Name == "exp" && reader["level"] != "0")
                    {
                        expTable.Level = byte.Parse(reader["level"]);
                        expTable.Experience = long.Parse(reader["value"]);

                        expList.Add(expTable);
                    }
                }
            }

            return expList;
        }
    }
}
