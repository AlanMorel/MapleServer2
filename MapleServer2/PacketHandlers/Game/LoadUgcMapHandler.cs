﻿using System;
using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
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

            bool isHouse = session.Player.MapId == (int) Map.PrivateResidence;
            List<int> plots = MapMetadataStorage.GetPlots(session.Player.MapId);
            List<Home> homes;
            if (isHouse)
            {
                Home home = DatabaseManager.GetHome(session.Player.VisitingHomeId);
                if (home != null)
                {
                    homes = new List<Home>() { home };

                    session.Send(ResponseLoadUGCMapPacket.LoadUGCMap(isHouse, home: home));
                    List<Cube> portals = home.FurnishingInventory.Values.Where(x => x.Item.Id == 50400190 || x.Item.Id == 50400158).ToList();

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
                    session.Send(ResponseLoadUGCMapPacket.LoadUGCMap(false));
                    return;
                }
            }
            else
            {
                homes = DatabaseManager.GetHomesOnMap(session.Player.MapId);
                session.Send(ResponseLoadUGCMapPacket.LoadUGCMap(isHouse));
            }

            session.Send(SendCubesPacket.LoadPlots(homes, session.Player.MapId));
            List<Cube> cubes = new List<Cube>();
            homes.ForEach(h => cubes.AddRange(h.FurnishingInventory.Values.ToList()));
            session.Send(SendCubesPacket.LoadCubes(cubes));
            session.Send(SendCubesPacket.LoadAvailablePlots(homes, plots));
            session.Send(SendCubesPacket.Expiration(homes.Where(x => x.PlotNumber != 0).ToList()));

            session.Send("6D 00 12 00 00 00 00 00 00 00 00 00 00 00 00".ToByteArray()); // send ugc
        }
    }
}
