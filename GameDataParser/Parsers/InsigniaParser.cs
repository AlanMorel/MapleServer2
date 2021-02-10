﻿using System.Collections.Generic;
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
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {

                if (!entry.Name.StartsWith("table/nametagsymbol"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    InsigniaMetadata metadata = new InsigniaMetadata();

                    if (node.Name == "symbol")
                    {
                        metadata.InsigniaId = short.Parse(node.Attributes["id"].Value);
                        metadata.ConditionType = node.Attributes["conditionType"].Value;
                        metadata.TitleId = string.IsNullOrEmpty(node.Attributes["code"]?.Value) ? 0 : int.Parse(node.Attributes["code"].Value);
                    }

                    insignias.Add(metadata);
                }
            }

            return insignias;
        }
    }
}
