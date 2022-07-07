using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class FieldWarHandler : GamePacketHandler<FieldWarHandler>
{
    public override RecvOp OpCode => RecvOp.FieldWar;

    private enum Mode : byte
    {
        LegionEnter = 0x01
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode function = (Mode) packet.ReadByte();
        switch (function)
        {
            case Mode.LegionEnter:
                HandleLegionEnter(session);
                break;
        }
    }

    private static void HandleLegionEnter(GameSession session)
    {
        FieldWar currentFieldWar = GameServer.FieldWarManager.CurrentFieldWar;

        if (currentFieldWar is null)
        {
            return;
        }
        if (currentFieldWar.EntryClosureTime < DateTimeOffset.UtcNow)
        {
            return;
        }

        int mapId = FieldWarMetadataStorage.MapId(currentFieldWar.Id);
        session.Player.Warp(mapId);
    }
}
