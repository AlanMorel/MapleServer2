using System.Diagnostics;
using Maple2Storage.Types;
using SharpNav;
using SharpNav.Geometry;
using SharpNav.IO.Json;
using SharpNav.Pathfinding;

namespace GameDataParser.Tools;

public class NavmeshGenerator
{
    private readonly NavMeshGenerationSettings Settings;

    public NavmeshGenerator()
    {
        Settings = NavMeshGenerationSettings.Default;
        // settings.AgentHeight = 100;
        // settings.AgentRadius = 30;
        // settings.MaxClimb = 150;
        Settings.CellSize = 5;
        Settings.CellHeight = 5;
    }

    /// <summary>
    /// Function to generate a navmesh from a list of triangles. From SharpNav example.
    /// </summary>
    public void GenerateNavMesh(Triangle3[] tris, string mapId)
    {
        Console.WriteLine("Generating Navmesh...");

        Stopwatch sw = new();
        sw.Start();

        //level.SetBoundingBoxOffset(new SVector3(settings.CellSize * 0.5f, settings.CellHeight * 0.5f, settings.CellSize * 0.5f));
        IEnumerable<Triangle3> triEnumerable = TriangleEnumerable.FromTriangle(tris, 0, tris.Length);
        BBox3 bounds = GetBoundingBoxCustom(triEnumerable);

        Heightfield heightField = new(bounds, Settings);
        Console.WriteLine("Heightfield");
        Console.WriteLine(" + Ctor\t\t\t\t" + sw.ElapsedMilliseconds.ToString("D3") + " ms");
        long prevMs = sw.ElapsedMilliseconds;

        /*Area[] areas = AreaGenerator.From(triEnumerable, Area.Default)
            .MarkAboveHeight(areaSettings.MaxLevelHeight, Area.Null)
            .MarkBelowHeight(areaSettings.MinLevelHeight, Area.Null)
            .MarkBelowSlope(areaSettings.MaxTriSlope, Area.Null)
            .ToArray();
        heightfield.RasterizeTrianglesWithAreas(levelTris, areas);*/
        heightField.RasterizeTriangles(tris, Area.Default);
        Console.WriteLine(" + Rasterization\t\t" + (sw.ElapsedMilliseconds - prevMs).ToString("D3") + " ms");
        Console.WriteLine(" + Filtering");
        prevMs = sw.ElapsedMilliseconds;

        // // Separate spans (polys) by height (max climb distance)
        // heightfield.FilterLedgeSpans(settings.VoxelAgentHeight, settings.VoxelMaxClimb);
        // Console.WriteLine("   + Ledge Spans\t\t" + (sw.ElapsedMilliseconds - prevMs).ToString("D3") + " ms");
        // prevMs = sw.ElapsedMilliseconds;
        //
        // heightfield.FilterLowHangingWalkableObstacles(settings.VoxelMaxClimb);
        // Console.WriteLine("   + Low Hanging Obstacles\t" + (sw.ElapsedMilliseconds - prevMs).ToString("D3") + " ms");
        // prevMs = sw.ElapsedMilliseconds;
        //
        // // Filter spans that overlay eachother and have no height to allow agent to walk
        // heightfield.FilterWalkableLowHeightSpans(settings.VoxelAgentHeight);
        // Console.WriteLine("   + Low Height Spans\t" + (sw.ElapsedMilliseconds - prevMs).ToString("D3") + " ms");
        // prevMs = sw.ElapsedMilliseconds;

        CompactHeightfield compactHeightfield = new(heightField, Settings);
        Console.WriteLine("CompactHeightfield");
        Console.WriteLine(" + Ctor\t\t\t\t" + (sw.ElapsedMilliseconds - prevMs).ToString("D3") + " ms");
        prevMs = sw.ElapsedMilliseconds;

        compactHeightfield.Erode(Settings.VoxelAgentRadius);
        Console.WriteLine(" + Erosion\t\t\t" + (sw.ElapsedMilliseconds - prevMs).ToString("D3") + " ms");
        prevMs = sw.ElapsedMilliseconds;

        compactHeightfield.BuildDistanceField();
        Console.WriteLine(" + Distance Field\t" + (sw.ElapsedMilliseconds - prevMs).ToString("D3") + " ms");
        prevMs = sw.ElapsedMilliseconds;

        // Connect spans
        compactHeightfield.BuildRegions(0, Settings.MinRegionSize, Settings.MergedRegionSize);
        Console.WriteLine(" + Regions\t\t\t" + (sw.ElapsedMilliseconds - prevMs).ToString("D3") + " ms");
        prevMs = sw.ElapsedMilliseconds;

        ContourSet contourSet = compactHeightfield.BuildContourSet(Settings);
        Console.WriteLine("ContourSet");
        Console.WriteLine(" + Ctor\t\t\t\t" + (sw.ElapsedMilliseconds - prevMs).ToString("D3") + " ms");
        prevMs = sw.ElapsedMilliseconds;

        PolyMesh polyMesh = new(contourSet, Settings);
        Console.WriteLine("PolyMesh");
        Console.WriteLine(" + Ctor\t\t\t\t" + (sw.ElapsedMilliseconds - prevMs).ToString("D3") + " ms");
        prevMs = sw.ElapsedMilliseconds;

        PolyMeshDetail polyMeshDetail = new(polyMesh, compactHeightfield, Settings);
        Console.WriteLine("PolyMeshDetail");
        Console.WriteLine(" + Ctor\t\t\t\t" + (sw.ElapsedMilliseconds - prevMs).ToString("D3") + " ms");

        // Build navmesh
        NavMeshBuilder buildData = new(polyMesh, polyMeshDetail, Array.Empty<OffMeshConnection>(), Settings);
        TiledNavMesh tiledNavMesh = new(buildData);

        new NavMeshJsonSerializer().Serialize(Paths.NAVMESH_DIR + $"/{mapId}.snj", tiledNavMesh);
    }

    private static BBox3 GetBoundingBoxCustom(IEnumerable<Triangle3> tris)
    {
        BBox3 b1 = new();

        b1.Min.X = 99999;
        b1.Max.X = -99999;

        b1.Min.Y = 99999;
        b1.Max.Z = -99999;

        b1.Min.Z = 99999;
        b1.Max.Z = -99999;
        foreach (Triangle3 tri in tris)
        {
            Vector3 a = tri.A;
            Vector3 b2 = tri.B;
            Vector3 c = tri.C;
            ApplyVertexToBounds(ref a, ref b1);
            ApplyVertexToBounds(ref b2, ref b1);
            ApplyVertexToBounds(ref c, ref b1);
        }

        return b1;
    }

    private static void ApplyVertexToBounds(ref Vector3 v, ref BBox3 b)
    {
        if (v.X < b.Min.X)
        {
            b.Min.X = v.X;
        }
        else if (v.X > b.Max.X)
        {
            b.Max.X = v.X;
        }

        if (v.Y < b.Min.Y)
        {
            b.Min.Y = v.Y;
        }
        else if (v.Y > b.Max.Y)
        {
            b.Max.Y = v.Y;
        }

        if (v.Z < b.Min.Z)
        {
            b.Min.Z = v.Z;
        }
        else if (v.Z > b.Max.Z)
        {
            b.Max.Z = v.Z;
        }
    }
}
