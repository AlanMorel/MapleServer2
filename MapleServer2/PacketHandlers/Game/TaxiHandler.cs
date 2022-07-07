using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using MoonSharp.Interpreter;

namespace MapleServer2.PacketHandlers.Game;

internal class TaxiHandler : GamePacketHandler<TaxiHandler>
{
    public override RecvOp OpCode => RecvOp.RequestTaxi;

    private enum Mode : byte
    {
        Car = 0x1,
        RotorsMeso = 0x3,
        RotorsMeret = 0x4,
        DiscoverTaxi = 0x5
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();

        int mapId = 0;
        long meretPrice = 15;

        if (mode != Mode.DiscoverTaxi)
        {
            mapId = packet.ReadInt();

            MapCashCall currentMapCall = MapMetadataStorage.GetMapCashCall(session.Player.MapId);
            if (currentMapCall.DisableExitWithTaxi)
            {
                session.Send(NoticePacket.Notice(SystemNotice.ErrCashTaxiCannotDeparture, NoticeType.Popup));
                return;
            }

            MapCashCall destinationMapCall = MapMetadataStorage.GetMapCashCall(mapId);
            if (destinationMapCall.DisableEnterWithTaxi)
            {
                session.Send(NoticePacket.Notice(SystemNotice.ErrCashTaxiCannotDestination, NoticeType.Popup));
                return;
            }
        }

        switch (mode)
        {
            case Mode.Car:
                HandleCarTaxi(session, mapId);
                break;
            case Mode.RotorsMeso:
                HandleRotorMeso(session, mapId);
                break;
            case Mode.RotorsMeret:
                HandleRotorMeret(session, mapId, meretPrice);
                break;
            case Mode.DiscoverTaxi:
                HandleDiscoverTaxi(session);
                break;
            default:
                LogUnknownMode(mode);
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

        Script script = ScriptLoader.GetScript("Functions/calcTaxiCost");

        DynValue result = script.RunFunction("calcTaxiCost", mapCount, session.Player.Levels.Level);
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

        Script script = ScriptLoader.GetScript("Functions/calcAirTaxiCost");

        DynValue result = script.RunFunction("calcAirTaxiCost", session.Player.Levels.Level);
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
