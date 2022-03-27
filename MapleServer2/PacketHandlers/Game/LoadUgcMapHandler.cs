using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class LoadUgcMapHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.REQUEST_LOAD_UGC_MAP;

    public override void Handle(GameSession session, PacketReader packet)
    {
        bool mapIsHome = session.Player.MapId == (int) Map.PrivateResidence;
        UgcMapMetadata ugcMapMetadata = UgcMapMetadataStorage.GetMetadata(session.Player.MapId);
        List<byte> plots = new();
        if (ugcMapMetadata != null)
        {
            plots = ugcMapMetadata.Groups.Select(x => x.Id).ToList();
        }

        List<Home> homes;
        if (mapIsHome)
        {
            Home home = GameServer.HomeManager.GetHomeById(session.Player.VisitingHomeId);
            if (home == null)
            {
                session.Send(ResponseLoadUgcMapPacket.LoadUgcMap());
                return;
            }

            homes = new()
            {
                home
            };

            session.Send(ResponseLoadUgcMapPacket.LoadUgcMap(home, session.Player.IsInDecorPlanner));

            // Find spawning coords for home
            int cubePortalId = 50400190;
            List<Cube> portals = home.FurnishingInventory.Values.Where(x => x.Item != null && x.Item.Id == cubePortalId).ToList();
            CoordF coord;
            CoordF rotation;
            if (portals.Count > 0)
            {
                Cube portal = portals.OrderBy(_ => Random.Shared.Next()).First();
                coord = portal.CoordF;
                coord.Z += 1;
                rotation = portal.Rotation;
                rotation.Z -= 90;
            }
            else
            {
                byte homeSize = (byte) (home.Size - 1);
                int x = -1 * Block.BLOCK_SIZE * homeSize;
                coord = CoordF.From(x, x, 151);
                rotation = CoordF.From(0, 0, 0);
            }
            session.Player.SavedCoord = coord;
            session.Player.SavedRotation = rotation;
            session.Player.SafeBlock = coord;
            session.Player.InstanceId = home.InstanceId;
        }
        else
        {
            homes = GameServer.HomeManager.GetPlots(session.Player.MapId);
            session.Send(ResponseLoadUgcMapPacket.LoadUgcMap());
        }

        List<Cube> cubes = new();
        if (!session.Player.IsInDecorPlanner)
        {
            homes.ForEach(h =>
            {
                int plotNumber = mapIsHome ? 1 : h.PlotNumber;
                cubes.AddRange(h.FurnishingInventory.Values.Where(x => x.Item.Id != 0 && x.PlotNumber == plotNumber).ToList());
            });
        }

        session.Send(SendCubesPacket.LoadPlots(homes, session.Player.MapId));
        session.Send(SendCubesPacket.LoadCubes(cubes));
        session.Send(SendCubesPacket.LoadAvailablePlots(homes, plots));
        session.Send(SendCubesPacket.Expiration(homes.Where(x => x.PlotNumber != 0).ToList()));

        session.Send(UgcPacket.Mode12());
    }
}
