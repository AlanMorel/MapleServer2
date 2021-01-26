using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using GameDataParser.Crypto.Common;
using GameDataParser.Parsers;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Files.Export
{
    public static class ExpMetadataExport
    {
        public static void Export(List<PackFileEntry> files, MemoryMappedFile memFile)
        {
            if (Hash.CheckHash("ms2-exptable-metadata"))
            {
                Console.WriteLine("\rSkipping exp metadata!");
                return;
            }

            // Parse and save exp table from xml file
            List<ExpMetadata> entities = ExpParser.Parse(memFile, files);
            ExpParser.Write(entities);
            Hash.WriteHash("ms2-exptable-metadata");
        }
    }
}
