using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace GameDataParser.Parsers
{
    public static class InsigniaParser
    {
        public static List<InsigniaMetadata> Parse(MemoryMappedFile m2dFile, IEnumerable<PackFileEntry> entries)
        {
            // Iterate over preset objects to later reference while iterating over exported maps
            List<InsigniaMetadata> insignias = new List<InsigniaMetadata>();
            foreach (PackFileEntry entry in entries)
            {

                if (!entry.Name.StartsWith("table/nametagsymbol"))
                {
                    continue;
                }

                using XmlReader reader = m2dFile.GetReader(entry.FileHeader);
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
        public static void Write(List<InsigniaMetadata> entities)
        {
            using (FileStream writeStream = File.Create($"{Paths.OUTPUT}/ms2-insignia-metadata"))
            {
                Serializer.Serialize(writeStream, entities);
            }
            using (FileStream readStream = File.OpenRead($"{Paths.OUTPUT}/ms2-insignia-metadata"))
            {
            }
            Console.WriteLine("\rSuccessfully parsed insignia metadata!");
        }
    }
}
