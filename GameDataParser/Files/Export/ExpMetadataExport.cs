using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using GameDataParser.Crypto.Common;
using GameDataParser.Parsers;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Files.Export
{
    public class ExpMetadataExport
    {

        public void Export()
        {
            string headerFile = VariableDefines.XML_PATH.Replace(".m2d", ".m2h");
            List<PackFileEntry> files = FileList.ReadFile(File.OpenRead(headerFile));
            MemoryMappedFile memFile = MemoryMappedFile.CreateFromFile(VariableDefines.XML_PATH);

            // Parse and save exp table from xml file
            List<ExpMetadata> entities = ExpParser.Parse(memFile, files);
            ExpParser.Write(entities);
        }
    }
}
