using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;

namespace GameDataParser.Parsers;

public class MeshParser : Exporter<object>
{
    public MeshParser(MetadataResources resources) : base(resources, "mesh") { }

    protected override object Parse()
    {
        IEnumerable<PackFileEntry> packFileEntries = Resources.TokReader.Files.Where(entry => entry.Name.EndsWith(".tok"));
        foreach (PackFileEntry file in packFileEntries)
        {
            string path = $"{Paths.NAVMESH_DIR}/{file.Name.ToLower()}";
            if (File.Exists(path))
            {
                continue;
            }

            File.WriteAllBytes(path, Resources.TokReader.GetBytes(file));
        }

        return null;
    }
}
