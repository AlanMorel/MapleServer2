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
    class ExpParser
    {
        public static List<ExpMetadata> Parse(MemoryMappedFile m2dFile, IEnumerable<PackFileEntry> entries)
        {
            List<ExpMetadata> expList = new List<ExpMetadata>();
            foreach (PackFileEntry entry in entries)
            {

                if (!entry.Name.StartsWith("table/nextexp"))
                {
                    continue;
                }

                XmlReader reader = m2dFile.GetReader(entry.FileHeader);
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

        public static void Write(List<ExpMetadata> entities)
        {
            using (FileStream writeStream = File.Create(VariableDefines.OUTPUT + "ms2-exptable-metadata"))
            {
                Serializer.Serialize(writeStream, entities);
            }
            using (FileStream readStream = File.OpenRead(VariableDefines.OUTPUT + "ms2-exptable-metadata"))
            {
            }
            Console.WriteLine("\rSuccessfully parsed exp table metadata!");
        }
    }
}
