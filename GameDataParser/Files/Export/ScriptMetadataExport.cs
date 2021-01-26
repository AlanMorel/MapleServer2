using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using GameDataParser.Crypto.Common;
using GameDataParser.Parsers;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Files.Export
{
    public class ScriptMetadataExport
    {
        public static void Export(List<PackFileEntry> files, MemoryMappedFile memFile)
        {
            if (Hash.CheckHash("ms2-script-metadata"))
            {
                Console.WriteLine("\rSkipping script metadata!");
                return;
            }

            // Parse script metadata
            List<ScriptMetadata> entities = ScriptParser.ParseNpc(memFile, files);
            entities.AddRange(ScriptParser.ParseQuest(memFile, files));
            ScriptParser.Write(entities);
            Hash.WriteHash("ms2-script-metadata");
        }
    }
}
