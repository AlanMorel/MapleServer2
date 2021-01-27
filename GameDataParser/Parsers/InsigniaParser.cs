using System.Collections.Generic;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class InsigniaParser : Exporter<List<InsigniaMetadata>>
    {
        public InsigniaParser(MetadataResources resources) : base(resources, "insignia") { }

        protected override List<InsigniaMetadata> Parse()
        {
            // Iterate over preset objects to later reference while iterating over exported maps
            List<InsigniaMetadata> insignias = new List<InsigniaMetadata>();
            foreach (PackFileEntry entry in resources.xmlFiles)
            {

                if (!entry.Name.StartsWith("table/nametagsymbol"))
                {
                    continue;
                }

                XmlReader reader = resources.xmlMemFile.GetReader(entry.FileHeader);
                while (reader.Read())
                {
                    InsigniaMetadata metadata = new InsigniaMetadata();
                    if (reader.NodeType != XmlNodeType.Element)
                    {
                        continue;
                    }

                    if (reader.Name == "symbol")
                    {
                        metadata.InsigniaId = short.Parse(reader["id"]);
                        metadata.ConditionType = reader["conditionType"];
                        metadata.TitleId = reader["code"] == "" ? 0 : int.Parse(reader["code"]);
                    }

                    insignias.Add(metadata);
                }
            }

            return insignias;
        }
    }
}
