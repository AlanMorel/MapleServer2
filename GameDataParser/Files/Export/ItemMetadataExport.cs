using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using GameDataParser.Crypto.Common;
using GameDataParser.Parsers;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Files.Export
{
    public static class ItemMetadataExport
    {
        public static void Export(List<PackFileEntry> files, MemoryMappedFile memFile)
        {
            if (Hash.CheckHash(VariableDefines.OUTPUT + "ms2-item-metadata"))
            {
                Console.WriteLine("\rSkipping item metadata!");
                return;
            }

            // Parse and save some item data from xml file
            List<ItemMetadata> items = ItemParser.Parse(memFile, files);
            ItemParser.Write(items);
            Hash.WriteHash(VariableDefines.OUTPUT + "ms2-item-metadata");
        }
    }
}
