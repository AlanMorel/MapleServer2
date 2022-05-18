using Maple2.PathEngine;
using Maple2.PathEngine.Exception;
using Maple2.PathEngine.Types;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using Serilog;
using Path = Maple2.PathEngine.Path;

namespace MapleServer2.Types;

public class FieldNavigator : IDisposable
{
    private readonly Mesh Mesh;
    private readonly CollisionContext CollisionContext;
    private readonly Dictionary<(int, int), Shape> Shapes;
    private readonly ILogger Logger = Log.Logger.ForContext<FieldNavigator>();

    public FieldNavigator(string mapName)
    {
        byte[] meshBuffer = File.ReadAllBytes(Paths.NAVMESH_DIR + $"/{mapName}.tok");

        Mesh = MapleServer.PathEngine.loadMeshFromBuffer(FileFormat.tok, meshBuffer);
        CollisionContext = Mesh.newContext();
        Shapes = new();
    }

    /// <summary>
    /// Find a random position on the mesh around the agent with the given radius.
    /// Creates a path from the agent position to the random position.
    /// </summary>
    /// <returns>List of CoordS or null if path is not possible</returns>
    public List<CoordS> GenerateRandomPathAroundAgent(Agent agent, int radius)
    {
        Position randomPositionLocally = Mesh.generateRandomPositionLocally(agent.getPosition(), radius);

        if (!PositionIsValid(randomPositionLocally))
        {
            return null;
        }

        using Path path = agent.findShortestPathTo(CollisionContext, randomPositionLocally);
        return PathToCoordS(path);
    }

    /// <summary>
    /// Find a random position on the mesh around the centerCoord with the given radius.
    /// Creates a path from the centerCoord to the random position.
    /// </summary>
    /// <returns>List of CoordS or null if path is not possible</returns>
    public List<CoordS> GenerateRandomPathAroundCoord(Agent agent, CoordS centerCoord, int radius)
    {
        Path path;
        try
        {
            Position position = FindPositionFromCoordS(centerCoord);

            Position randomPositionLocally = Mesh.generateRandomPositionLocally(position, radius);

            Position findClosestUnobstructedPosition = agent.findClosestUnobstructedPosition(CollisionContext, 500);
            agent.moveTo(findClosestUnobstructedPosition);

            path = agent.findShortestPathTo(CollisionContext, randomPositionLocally);
        }
        catch (InvalidPositionException)
        {
            return null;
        }
        catch (Exception e)
        {
            Logger.Error("Error in GenerateRandomPathAroundCoord. Agent position: {0}. Center coord: {1}. {2}",
                agent.getPosition(), centerCoord, e.Message);
            return null;
        }

        return PathToCoordS(path);
    }

    /// <summary>
    /// Find the shortest path from the agent to the target position.
    /// </summary>
    /// <returns>List of CoordS or null if path is not possible</returns>
    public List<CoordS> FindPath(Agent agent, CoordS endCoord)
    {
        Path path;
        try
        {
            if (!FindFirstPositionBelow(endCoord, out Position position))
            {
                return null;
            }

            path = agent.findShortestPathTo(CollisionContext, position);
        }
        catch (InvalidPositionException)
        {
            return null;
        }
        catch (Exception e)
        {
            Logger.Error("Error in FindPath. Agent position: {0}. End coord: {1}. {2}",
                agent.getPosition(), endCoord, e.Message);
            return null;
        }

        return PathToCoordS(path);
    }

    /// <summary>
    /// Creates a box from the given capsule metadata. Used to calculate collision in the mesh.
    /// </summary>
    public Shape AddShape(NpcMetadataCapsule metadata)
    {
        int width = metadata.Radius; // Using radius for width
        int height = metadata.Height;

        if (Shapes.TryGetValue((width, height), out Shape cacheShape))
        {
            return cacheShape;
        }

        int halfWidth = width / 2;
        int halfHeight = height / 2;
        List<Point> rectArray = new()
        {
            new(-halfWidth, -halfHeight),
            new(-halfWidth, halfHeight),
            new(halfWidth, halfHeight),
            new(halfWidth, -halfHeight)
        };

        Shape shape = MapleServer.PathEngine.newShape(rectArray);

        Shapes.Add((width, height), shape);

        Mesh.generateUnobstructedSpaceFor(shape, true);
        Mesh.generatePathfindPreprocessFor(shape);
        return shape;
    }

    /// <summary>
    /// Creates an agent from the given sh2ape, also adds it to the mesh and to the collision context.
    /// </summary>
    /// <returns>Agent or null if position isn't valid</returns>
    public Agent AddAgent(IFieldActor actor, Shape shape)
    {
        Position position = FindPositionFromCoordS(actor.Coord.ToShort());
        if (!PositionIsValid(position))
        {
            return null;
        }

        Position unobstructedPosition = Mesh.findClosestUnobstructedPosition(shape, CollisionContext, position, 200);
        if (!PositionIsValid(unobstructedPosition))
        {
            return null;
        }

        Agent agent = Mesh.placeAgent(shape, unobstructedPosition);
        agent.setUserData(actor.ObjectId);

        CollisionContext.addAgent(agent);
        return agent;
    }

    /// <summary>
    /// Find the nearest position on the mesh to the given coordS.
    /// </summary>
    public Position FindPositionFromCoordS(CoordS coordF)
    {
        return Mesh.positionNear3DPoint(coordF.X, coordF.Y, coordF.Z, horizontalRange: 15, verticalRange: 15);
    }

    /// <summary>
    /// Check if the given position is valid.
    /// </summary>
    public bool PositionIsValid(Position position)
    {
        return Mesh.positionIsValid(position);
    }

    /// <summary>
    /// Find the first valid position below the given coordS.
    /// </summary>
    public bool FindFirstPositionBelow(CoordS coordF, out Position position)
    {
        position = default;
        CoordS tempCoord = coordF;
        // Check 10 times below the given coordS
        for (int i = 0; i < 10; i++)
        {
            position = Mesh.positionNear3DPoint(tempCoord.X, tempCoord.Y, tempCoord.Z, horizontalRange: 15, verticalRange: 15);
            if (position.Cell != -1)
            {
                return true;
            }

            tempCoord.Z -= Block.BLOCK_SIZE;
        }

        return false;
    }

    public CoordS? FindClosestUnobstructedCoordS(Shape shape, CoordS coordS, int radius)
    {
        Position position = FindPositionFromCoordS(coordS);
        return FindClosestUnobstructedCoordS(shape, position, radius);
    }

    public CoordS? FindClosestUnobstructedCoordS(Shape shape, Position position, int radius)
    {
        Position unobstructedPosition;
        try
        {
            Position randomPosition = Mesh.generateRandomPositionLocally(position, radius);

            unobstructedPosition = Mesh.findClosestUnobstructedPosition(shape, CollisionContext, randomPosition, radius);
        }
        catch (InvalidPositionException)
        {
            return null;
        }

        if (!PositionIsValid(unobstructedPosition))
        {
            return null;
        }

        return CoordS.From(unobstructedPosition.X, unobstructedPosition.Y, Mesh.heightAtPosition(unobstructedPosition));
    }

    private List<CoordS> PathToCoordS(Path path)
    {
        if (path is null)
        {
            return null;
        }

        List<CoordS> coordS = new();
        for (int i = 0; i < path.size(); i++)
        {
            Position position = path.position(i);
            if (position.Cell < 0)
            {
                continue;
            }

            coordS.Add(GetCoordSFromPosition(position));
        }

        return coordS;
    }

    private CoordS GetCoordSFromPosition(Position position)
    {
        return CoordS.From(position.X, position.Y, Mesh.heightAtPosition(position));
    }

    public void Dispose()
    {
        Mesh?.Dispose();
        GC.SuppressFinalize(this);
    }

    ~FieldNavigator()
    {
        Dispose();
    }
}
