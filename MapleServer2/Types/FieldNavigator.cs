using Maple2Storage.Types;
using NLog;
using SharpNav;
using SharpNav.Geometry;
using SharpNav.IO.Json;
using SharpNav.Pathfinding;

namespace MapleServer2.Types
{
    public class FieldNavigator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const float SnapRange = 500;

        private TiledNavMesh TiledNavMesh;
        private NavMeshQuery NavMeshQuery;

        public void LoadNavMeshFromFile(string path)
        {
            try
            {
                TiledNavMesh = new NavMeshJsonSerializer().Deserialize(path);
                NavMeshQuery = new(TiledNavMesh, 2048);
            }
            catch (Exception e)
            {
                Logger.Error("Navmesh loading failed with exception:" + Environment.NewLine + e);
            }
        }

        public IEnumerable<CoordF> GenerateRandomPath(CoordF center, float range)
        {
            if (NavMeshQuery is null || TiledNavMesh is null)
            {
                return null;
            }

            NavQueryFilter filter = new();
            Vector3 snapRange = new(SnapRange);

            Vector3 centerV = new(center.X, center.Z, center.Y);

            NavMeshQuery.FindNearestPoly(ref centerV, ref snapRange, out NavPoint navFrom);
            NavMeshQuery.FindRandomPointAroundCircle(ref navFrom, range, out NavPoint navTo);

            return GeneratePathfinding(ref filter, ref navFrom, ref navTo).Select(c => CoordF.From(c.X, c.Z, c.Y));
        }

        public IEnumerable<CoordF> GenerateMoveToPath(CoordF from, CoordF to)
        {
            if (NavMeshQuery is null || TiledNavMesh is null)
            {
                return null;
            }

            NavQueryFilter filter = new();
            Vector3 snapRange = new(SnapRange);

            Vector3 fromV = new(from.X, from.Z, from.Y);
            Vector3 toV = new(to.X, to.Z, to.Y);

            NavMeshQuery.FindNearestPoly(ref fromV, ref snapRange, out NavPoint navFrom);
            NavMeshQuery.FindNearestPoly(ref toV, ref snapRange, out NavPoint navTo);

            return GeneratePathfinding(ref filter, ref navFrom, ref navTo).Select(c => CoordF.From(c.X, c.Z, c.Y));
        }

        private IEnumerable<CoordF> GeneratePathfinding(ref NavQueryFilter filter, ref NavPoint startPt, ref NavPoint endPt)
        {
            if (NavMeshQuery is null || TiledNavMesh is null)
            {
                return null;
            }

            SharpNav.Pathfinding.Path path = new();
            NavMeshQuery.FindPath(ref startPt, ref endPt, filter, path);

            // Find a smooth path (collection of points) over the mesh surface, on the path
            int polyCount = path.Count;
            Vector3 iterPos = new();
            Vector3 targetPos = new();
            NavMeshQuery.ClosestPointOnPoly(startPt.Polygon, startPt.Position, ref iterPos);
            NavMeshQuery.ClosestPointOnPoly(path[polyCount - 1], endPt.Position, ref targetPos);

            List<Vector3> smoothPath = new(2048)
            {
                iterPos
            };

            const float StepSize = 0.5f;
            const float Slop = 0.01f;
            while (polyCount > 0 && smoothPath.Count < smoothPath.Capacity)
            {
                //find location to steer towards
                Vector3 steerPos = new();
                StraightPathFlags steerPosFlag = 0;
                NavPolyId steerPosRef = NavPolyId.Null;

                if (!GetSteerTarget(NavMeshQuery, iterPos, targetPos, Slop, path, ref steerPos, ref steerPosFlag, ref steerPosRef))
                {
                    break;
                }

                bool endOfPath = (steerPosFlag & StraightPathFlags.End) != 0;
                bool offMeshConnection = (steerPosFlag & StraightPathFlags.OffMeshConnection) != 0;

                // Find movement delta
                Vector3 delta = steerPos - iterPos;
                float len = (float) Math.Sqrt(Vector3.Dot(delta, delta));

                //if steer target is at end of path or off-mesh link
                //don't move past location
                if ((endOfPath || offMeshConnection) && len < StepSize)
                {
                    len = 1;
                }
                else
                {
                    len = StepSize / len;
                }

                Vector3 moveTgt = new();
                VMad(ref moveTgt, iterPos, delta, len);

                //move
                List<NavPolyId> visited = new(16);
                NavPoint startPoint = new(path[0], iterPos);
                NavMeshQuery.MoveAlongSurface(ref startPoint, ref moveTgt, out Vector3 result, visited);
                path.FixupCorridor(visited);
                polyCount = path.Count;
                float h = 0;
                NavMeshQuery.GetPolyHeight(path[0], result, ref h);
                result.Y = h;
                iterPos = result;

                //handle end of path when close enough
                if (endOfPath && InRange(iterPos, steerPos, Slop, 1.0f))
                {
                    //reached end of path
                    iterPos = targetPos;
                    if (smoothPath.Count < smoothPath.Capacity)
                    {
                        smoothPath.Add(iterPos);
                    }

                    break;
                }

                //store results
                if (smoothPath.Count < smoothPath.Capacity)
                {
                    smoothPath.Add(iterPos);
                }
            }

            return smoothPath.Select(v => CoordF.From(v.X, v.Y, v.Z));
        }

        private static bool GetSteerTarget(NavMeshQuery navMeshQuery, Vector3 startPos, Vector3 endPos, float minTargetDist, SharpNav.Pathfinding.Path path,
            ref Vector3 steerPos, ref StraightPathFlags steerPosFlag, ref NavPolyId steerPosRef)
        {
            StraightPath steerPath = new();
            navMeshQuery.FindStraightPath(startPos, endPos, path, steerPath, 0);
            int steerPathCount = steerPath.Count;
            if (steerPathCount == 0)
            {
                return false;
            }

            //find vertex far enough to steer to
            int ns = 0;
            while (ns < steerPathCount)
            {
                if ((steerPath[ns].Flags & StraightPathFlags.OffMeshConnection) != 0 ||
                    !InRange(steerPath[ns].Point.Position, startPos, minTargetDist, 1000.0f))
                {
                    break;
                }

                ns++;
            }

            //failed to find good point to steer to
            if (ns >= steerPathCount)
            {
                return false;
            }

            steerPos = steerPath[ns].Point.Position;
            steerPos.Y = startPos.Y;
            steerPosFlag = steerPath[ns].Flags;
            if (steerPosFlag == StraightPathFlags.None && ns == (steerPathCount - 1))
            {
                steerPosFlag = StraightPathFlags.End; // otherwise seeks path infinitely!!!
            }

            steerPosRef = steerPath[ns].Point.Polygon;

            return true;
        }

        private static void VMad(ref Vector3 dest, Vector3 v1, Vector3 v2, float s)
        {
            dest.X = v1.X + v2.X * s;
            dest.Y = v1.Y + v2.Y * s;
            dest.Z = v1.Z + v2.Z * s;
        }

        private static bool InRange(Vector3 v1, Vector3 v2, float r, float h)
        {
            float dx = v2.X - v1.X;
            float dy = v2.Y - v1.Y;
            float dz = v2.Z - v1.Z;
            return (dx * dx + dz * dz) < (r * r) && Math.Abs(dy) < h;
        }
    }
}
