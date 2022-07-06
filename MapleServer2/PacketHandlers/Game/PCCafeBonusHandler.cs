using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game;

public class PCCafeBonusHandler : GamePacketHandler<PCCafeBonusHandler>
{
    public override RecvOp OpCode => RecvOp.PCCafeBonus;

    private enum Mode : byte
    {
        ClaimLoginTimeReward = 0x1,
        ClaimPCCafeItem = 0x2
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();

        switch (mode)
        {
            case Mode.ClaimLoginTimeReward:
                HandleClaimLoginTimeReward(session, packet);
                break;
            case Mode.ClaimPCCafeItem:
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
