using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using GameDataParser.Crypto.Common;
using GameDataParser.Parsers;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Files.Export
{
    public static class InsigniaMetadataExport
    {
        public static void Export(List<PackFileEntry> files, MemoryMappedFile memFile)
        {
            if (Hash.CheckHash("ms2-insignia-metadata"))
            {
                Console.WriteLine("\rSkipping insignia metadata!");
                return;
            }

            // Parse and save some item data from xml file
            List<InsigniaMetadata> entities = InsigniaParser.Parse(memFile, files);
            InsigniaParser.Write(entities);
            Hash.WriteHash("ms2-insignia-metadata");
        }
    }
}
