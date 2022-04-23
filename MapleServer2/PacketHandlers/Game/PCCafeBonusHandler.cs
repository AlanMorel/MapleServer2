using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Enums;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class PCCafeBonusHandler : GamePacketHandler<PCCafeBonusHandler>
{
    public override RecvOp OpCode => RecvOp.PCCafeBonus;

    private enum PCCafeBonusMode : byte
    {
        ClaimLoginTimeReward = 0x1,
        ClaimPCCafeItem = 0x2
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        PCCafeBonusMode mode = (PCCafeBonusMode) packet.ReadByte();

        switch (mode)
        {
            case PCCafeBonusMode.ClaimLoginTimeReward:
                HandleClaimLoginTimeReward(session, packet);
                break;
            case PCCafeBonusMode.ClaimPCCafeItem:
                HandleClaimPCCafeItem(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleClaimLoginTimeReward(GameSession session, PacketReader packet)
    {
        byte index = packet.ReadByte();
        // Needs to send the item via mail
        session.Send(PCCafeBonusPacket.ClaimLoginTimeReward(index));
    }

    private static void HandleClaimPCCafeItem(GameSession session, PacketReader packet)
    {
        int index = packet.ReadInt();
        session.Send(PCCafeBonusPacket.ClaimPCCafeItem(index));
    }
}
