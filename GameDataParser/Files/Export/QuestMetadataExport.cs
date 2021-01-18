﻿using System.Collections.Generic;
using System.IO;
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
            // Parse quest metadata
            List<QuestMetadata> entities = QuestParser.Parse(memFile, files);
            QuestParser.Write(entities);
        }
    }
}
