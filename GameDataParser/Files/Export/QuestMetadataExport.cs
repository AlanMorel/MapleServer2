using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using GameDataParser.Crypto.Common;
using GameDataParser.Parsers;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Files.Export
{
    public class QuestMetadataExport
    {
        public static void Export(List<PackFileEntry> files, MemoryMappedFile memFile)
        {
            if (Hash.CheckHash(VariableDefines.OUTPUT + "ms2-quest-metadata"))
            {
                Console.WriteLine("\rSkipping quest metadata!");
                return;
            }
            // Parse quest metadata
            List<QuestMetadata> entities = QuestParser.Parse(memFile, files);
            QuestParser.Write(entities);
            Hash.WriteHash(VariableDefines.OUTPUT + "ms2-quest-metadata");
        }
    }
}
