using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using MoonSharp.Interpreter;

namespace MapleServer2.PacketHandlers.Game;

internal class RequestTaxiHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.RequestTaxi;

    private enum RequestTaxiMode : byte
    {
        Car = 0x1,
        RotorsMeso = 0x3,
        RotorsMeret = 0x4,
        DiscoverTaxi = 0x5
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        RequestTaxiMode mode = (RequestTaxiMode) packet.ReadByte();

        int mapId = 0;
        long meretPrice = 15;

        if (mode != RequestTaxiMode.DiscoverTaxi)
        {
            mapId = packet.ReadInt();
        }

        switch (mode)
        {
            case RequestTaxiMode.Car:
                HandleCarTaxi(session, mapId);
                break;
            case RequestTaxiMode.RotorsMeso:
                HandleRotorMeso(session, mapId);
                break;
            case RequestTaxiMode.RotorsMeret:
                HandleRotorMeret(session, mapId, meretPrice);
                break;
            case RequestTaxiMode.DiscoverTaxi:
                HandleDiscoverTaxi(session);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleCarTaxi(GameSession session, int mapId)
    {
        if (!WorldMapGraphStorage.CanPathFind(session.Player.MapId.ToString(), mapId.ToString(), out int mapCount))
        {
            Logger.Warning("Path not found.");
            return;
        }

        ScriptLoader scriptLoader = new("Functions/calcTaxiCost");

        DynValue result = scriptLoader.Call("calcTaxiCost", mapCount, session.Player.Levels.Level);
        if (result == null)
        {
            return;
        }

        if (!session.Player.Wallet.Meso.Modify((long) -result.Number))
        {
            return;
        }
        session.Player.Warp(mapId);
    }

    private static void HandleRotorMeso(GameSession session, int mapId)
    {
        // VIP Travel
        Account account = session.Player.Account;
        if (account.IsVip())
        {
            session.Player.Warp(mapId);
            return;
        }

        ScriptLoader scriptLoader = new("Functions/calcAirTaxiCost");

        DynValue result = scriptLoader.Call("calcAirTaxiCost", session.Player.Levels.Level);
        if (result == null)
        {
            return;
        }

        if (!session.Player.Wallet.Meso.Modify((long) -result.Number))
        {
            return;
        }

        session.Player.Warp(mapId);
    }

    private static void HandleRotorMeret(GameSession session, int mapId, long meretPrice)
    {
        if (!session.Player.Account.RemoveMerets(meretPrice))
        {
            return;
        }

        session.Player.Warp(mapId);
    }

    private static void HandleDiscoverTaxi(GameSession session)
    {
        List<int> unlockedTaxis = session.Player.UnlockedTaxis;
        int mapId = session.Player.MapId;
        if (!unlockedTaxis.Contains(mapId))
        {
            unlockedTaxis.Add(mapId);
        }
        session.Send(TaxiPacket.DiscoverTaxi(mapId));
    }
}
