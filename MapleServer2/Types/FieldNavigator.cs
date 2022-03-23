using Maple2.PathEngine;
using Maple2.PathEngine.Interface;
using Maple2.PathEngine.Types;
using Maple2Storage.Types;
using Path = Maple2.PathEngine.Path;

namespace MapleServer2.Types;

public class FieldNavigator : IDisposable
{
    private readonly Mesh Mesh;
    private readonly CollisionContext CollisionContext;

    public FieldNavigator(string mapName)
    {
        byte[] meshBuffer = File.ReadAllBytes(Paths.NAVMESH_DIR + $"/{mapName}.tok");

        Mesh = MapleServer.PathEngine.loadMeshFromBuffer(FileFormat.tok, meshBuffer, options: null);
        CollisionContext = Mesh.newContext();
    }

    /// <summary>
    /// Find a random position on the mesh around the agent with the given radius.
    /// Creates a path from the agent position to the random position.
    /// </summary>
    /// <returns>List of CoordS or null if path is not possible</returns>
    public List<CoordS> GenerateRandomPathAroundAgent(Agent agent, int radius)
    {
        Position randomPositionLocally = Mesh.generateRandomPositionLocally(agent.getPosition(), radius);

        if (!Mesh.positionIsValid(randomPositionLocally))
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
        Position position = FindPositionFromCoordS(centerCoord);

        if (!Mesh.positionIsValid(position))
        {
            return null;
        }

        Position randomPositionLocally = Mesh.generateRandomPositionLocally(position, radius);

        if (!Mesh.positionIsValid(randomPositionLocally))
        {
            return null;
        }

        using Path path = agent.findShortestPathTo(CollisionContext, randomPositionLocally);
        return PathToCoordS(path);
    }

    /// <summary>
    /// Find the shortest curved path from the agent to the target position.
    /// </summary>
    /// <returns>List of CoordS or null if path is not possible</returns>
    public List<CoordS> FindPath(Agent agent, CoordS endCoord)
    {
        Position end = FindPositionFromCoordS(endCoord);

        if (!Mesh.positionIsValid(end))
        {
            return null;
        }

        using Path shortestPath = agent.findShortestPathTo(CollisionContext, end);
        if (shortestPath is null)
        {
            Console.WriteLine("Shortest path is null");
            return null;
        }

        using Path path = agent.generateCurvedPath(shortestPath, CollisionContext, 0, 0, sectionLength: 50, turnRatio1: 0.5f, turnRatio2: 0.9f);
        if (path is null)
        {
            Console.WriteLine("Curved path is null");
        }

        return PathToCoordS(path);
    }

    /// <summary>
    /// Creates a box from the given capsule metadata. Used to calculate collision in the mesh.
    /// </summary>
    /// <returns>Shape</returns>
    public Shape AddShape( /*NpcMetadataCapsule metadata*/)
    {
        // TODO: Cache shapes??
        int width = 45;
        int height = 170;

        int halfWidth = width / 2;
        int halfHeight = height / 2;
        int negativeHalfWidth = halfWidth * -1;
        int negativeHalfHeight = halfHeight * -1;
        List<Point> rectArray = new()
        {
            new(negativeHalfWidth, negativeHalfHeight),
            new(negativeHalfWidth, halfHeight),
            new(halfWidth, halfHeight),
            new(halfWidth, negativeHalfHeight)
        };

        Shape shape = MapleServer.PathEngine.newShape(rectArray);
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
        if (!Mesh.positionIsValid(position))
        {
            return null;
        }

        Agent agent = Mesh.placeAgent(shape, position);
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

    public Position? FindClosestUnobstructedPosition(Shape shape, Position position, int radius)
    {
        if (!PositionIsValid(position))
        {
            return null;
        }

        Position randomPosition = Mesh.generateRandomPositionLocally(position, radius);
        if (!PositionIsValid(randomPosition))
        {
            return null;
        }

        Position unobstructedPosition = Mesh.findClosestUnobstructedPosition(shape, CollisionContext, randomPosition, radius);
        if (!PositionIsValid(unobstructedPosition))
        {
            return null;
        }

        return unobstructedPosition;
    }

    public CoordS? FindClosestUnobstructedCoordS(Shape shape, CoordS coordS, int radius)
    {
        Position position = FindPositionFromCoordS(coordS);
        return FindClosestUnobstructedCoordS(shape, position, radius);
    }

    public CoordS? FindClosestUnobstructedCoordS(Shape shape, Position position, int radius)
    {
        if (!PositionIsValid(position))
        {
            return null;
        }

        Position randomPosition = Mesh.generateRandomPositionLocally(position, radius);
        if (!PositionIsValid(randomPosition))
        {
            return null;
        }

        Position unobstructedPosition = Mesh.findClosestUnobstructedPosition(shape, CollisionContext, randomPosition, radius);
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
                Console.WriteLine("Invalid position: " + position);
                continue;
            }

            coordS.Add(CoordS.From(position.X, position.Y, Mesh.heightAtPosition(position)));
        }

        return coordS;
    }

    public void Dispose()
    {
        Mesh?.Dispose();
        GC.SuppressFinalize(this);
    }
}

public class ErrorHandler : IErrorHandler
{
    public override ErrorResult handle(ErrorType type, string description, IDictionary<string, string> attributes)
    {
        Console.WriteLine("Type: " + type + " Description: " + description + " Attributes: " + attributes);
        return 0;
    }
}
