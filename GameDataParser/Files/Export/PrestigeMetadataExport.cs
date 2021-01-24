using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using GameDataParser.Crypto.Common;
using GameDataParser.Parsers;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Files.Export
{
    public static class PrestigeMetadataExport
    {
        public static void Export(List<PackFileEntry> files, MemoryMappedFile memFile)
        {
            if (Hash.CheckHash(VariableDefines.OUTPUT + "ms2-prestige-metadata"))
            {
                Console.WriteLine("\rSkipping prestige metadata!");
                return;
            }

            // Parse and save some item data from xml file
            PrestigeMetadata PrestigeMetadata = PrestigeParser.Parse(memFile, files);
            PrestigeParser.Write(PrestigeMetadata);
            Hash.WriteHash(VariableDefines.OUTPUT + "ms2-prestige-metadata");
        }
    }
}
