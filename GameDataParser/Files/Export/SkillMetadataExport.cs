using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using GameDataParser.Crypto.Common;
using GameDataParser.Parsers;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Files.Export
{
    public static class SkillMetadataExport
    {
        public static void Export(List<PackFileEntry> files, MemoryMappedFile memFile)
        {
            if (Hash.CheckHash("ms2-skill-metadata"))
            {
                Console.WriteLine("\rSkipping skill metadata!");
                return;
            }

            // Parse and save some item data from xml file
            List<SkillMetadata> skills = SkillParser.Parse(memFile, files);
            SkillParser.Write(skills);
            Hash.WriteHash("ms2-skill-metadata");
        }
    }
}
