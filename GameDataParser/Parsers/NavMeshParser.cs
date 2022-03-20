using GameDataParser.Files;
using GameDataParser.Tools;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Maple2.File.IO.Crypto.Common;
using Maple2.File.IO.Tok;
using Maple2.File.IO.Tok.XmlTypes;
using Maple2Storage.Types;
using SharpNav.Geometry;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace GameDataParser.Parsers;

public class NavMeshParser : Exporter<object>
{
    public NavMeshParser(MetadataResources resources) : base(resources, "navmesh") { }

    protected override object Parse()
    {
        // Extract the contents of NavMesh1.zip and NavMesh2.zip to the NavMesh directory
        ExtractNavMesh("NavMesh1.zip");
        ExtractNavMesh("NavMesh2.zip");

        // Parse all navmesh (.tok) files
        IEnumerable<PackFileEntry> packFileEntries = Resources.NavmeshReader.Files.Where(entry => entry.Name.EndsWith(".tok"));
        foreach (PackFileEntry entry in packFileEntries)
        {
            string mapId = entry.Name[..^".tok".Length].ToLower();

            if (File.Exists(Paths.NAVMESH_DIR + $"/{mapId}.snj"))
            {
                continue;
            }

            TokReader reader = new(Resources.NavmeshReader.GetBytes(entry));
            Mesh mesh = reader.Parse();

            Console.Write("Translating PathEngine data to SharpNav...");
            List<Triangle3> triangles = new();

            // Gather triangles (PathEngine -> SharpNav)
            List<Vertex> verts = mesh.Mesh3D.Vertices.Vertex;
            foreach (Triangle tri in mesh.Mesh3D.Triangles.Triangle)
            {
                Vertex v0 = verts[tri.Edge0StartVert];
                Vertex v1 = verts[tri.Edge1StartVert];
                Vertex v2 = verts[tri.Edge2StartVert];

                const int InvalidZ = short.MinValue;
                Vector3 sv0 = new(v0.X, v0.Z, v0.Y);
                if (tri.Edge0StartZ != InvalidZ)
                {
                    sv0.Y = tri.Edge0StartZ;
                }

                Vector3 sv1 = new(v1.X, v1.Z, v1.Y);
                if (tri.Edge1StartZ != InvalidZ)
                {
                    sv1.Y = tri.Edge1StartZ;
                }

                Vector3 sv2 = new(v2.X, v2.Z, v2.Y);
                if (tri.Edge2StartZ != InvalidZ)
                {
                    sv2.Y = tri.Edge2StartZ;
                }

                // One map had a triangle with a coord so big it would be impossible to parse the map.
                // This is a workaround to prevent others triangles like that from being added to the mesh.
                if (sv0.X > 20000 || sv0.Y > 20000 || sv0.Z > 20000 ||
                    sv1.X > 20000 || sv1.Y > 20000 || sv1.Z > 20000 ||
                    sv2.X > 20000 || sv2.Y > 20000 || sv2.Z > 20000)
                {
                    continue;
                }

                triangles.Add(new(sv0, sv1, sv2));
            }

            new NavmeshGenerator().GenerateNavMesh(triangles.ToArray(), mapId);
        }

        return null;
    }

    private static void ExtractNavMesh(string fileName)
    {
        Directory.CreateDirectory(Paths.NAVMESH_DIR);

        using FileStream fsInput = File.OpenRead(Paths.SOLUTION_DIR + $"/GameDataParser/{fileName}");
        using ZipFile zf = new(fsInput);
        foreach (ZipEntry zipEntry in zf)
        {
            // Ignore directories
            if (!zipEntry.IsFile)
            {
                continue;
            }

            string entryFileName = zipEntry.Name;
            string fullZipToPath = Path.Combine(Paths.NAVMESH_DIR, entryFileName);

            // Skip files that already exists
            if (File.Exists(fullZipToPath))
            {
                continue;
            }

            // 4K is optimum
            byte[] buffer = new byte[4096];

            // Unzip file in buffered chunks. This is just as fast as unpacking
            // to a buffer the full size of the file, but does not waste memory.
            // The "using" will close the stream even if an exception occurs.
            using Stream zipStream = zf.GetInputStream(zipEntry);
            using Stream fsOutput = File.Create(fullZipToPath);
            StreamUtils.Copy(source: zipStream, destination: fsOutput, buffer);
        }
    }
}
