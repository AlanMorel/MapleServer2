using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using GameDataParser.Crypto.Common;
using GameDataParser.Parsers;
using Maple2Storage.Types;

namespace GameDataParser.Files.Export
{
    public class MapMetadataExport
    {
        public void Export()
        {
            string headerFile = SetPath.EXPORTED_PATH.Replace(".m2d", ".m2h");
            List<PackFileEntry> files = FileList.ReadFile(File.OpenRead(headerFile));
            var memFile = MemoryMappedFile.CreateFromFile(SetPath.EXPORTED_PATH);
            List<MapEntityMetadata> entities = MapEntityParser.Parse(memFile, files);
            MapEntityParser.Write(entities);
        }
    }
}
