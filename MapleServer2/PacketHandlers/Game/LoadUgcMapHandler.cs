using System;
using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class LoadUgcMapHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_LOAD_UGC_MAP;

        public LoadUgcMapHandler(ILogger<LoadUgcMapHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            Random random = new Random();

            bool isHome = session.Player.MapId == (int) Map.PrivateResidence;
            List<byte> plots = UGCMapMetadataStorage.GetMetadata(session.Player.MapId).Groups.Select(x => x.Id).ToList();
            List<Home> homes;
            if (isHome)
            {
                Home home = GameServer.HomeManager.GetHome(session.Player.VisitingHomeId);
                if (home == null)
                {
                    session.Send(ResponseLoadUGCMapPacket.LoadUGCMap(false));
                    return;
                }

                homes = new List<Home>() { home };

                session.Send(ResponseLoadUGCMapPacket.LoadUGCMap(isHome, home));

                List<Cube> portals = home.FurnishingInventory.Values.Where(x => x.Item != null && x.Item.Id == 50400190).ToList();
                CoordF coord;
                CoordF rotation;
                if (portals.Count > 0)
                {
                    Cube portal = portals.OrderBy(x => random.Next()).Take(1).First();
                    rotation = portal.Rotation;
                    coord = portal.CoordF;
                }
                else
                {
                    byte homeSize = (byte) (home.Size - 1);
                    int x = -1 * Block.BLOCK_SIZE * homeSize;
                    coord = CoordF.From(x, x, 151);
                    rotation = CoordF.From(0, 0, 0);
                }
                session.Player.Coord = coord;
                session.Player.Rotation = rotation;
                session.Player.InstanceId = session.Player.VisitingHomeId;
            }
            else
            {
                homes = GameServer.HomeManager.GetPlots(session.Player.MapId);
                session.Send(ResponseLoadUGCMapPacket.LoadUGCMap(isHome));
            }

            List<Cube> cubes = new List<Cube>();
            homes.ForEach(h =>
            {
                cubes.AddRange(h.FurnishingInventory.Values.Where(x => x.Item.Id != 0).ToList());
            });

            session.Send(SendCubesPacket.LoadPlots(homes, session.Player.MapId));
            session.Send(SendCubesPacket.LoadCubes(cubes));
            session.Send(SendCubesPacket.LoadAvailablePlots(homes, plots));
            session.Send(SendCubesPacket.Expiration(homes.Where(x => x.PlotNumber != 0).ToList()));

            session.Send("6D 00 12 00 00 00 00 00 00 00 00 00 00 00 00".ToByteArray()); // send ugc
        }
    }
}
