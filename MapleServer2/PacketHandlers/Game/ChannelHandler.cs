
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class ChannelHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.CHANNEL;

    public override void Handle(GameSession session, PacketReader packet)
    {
        short channelId = packet.ReadShort();

        Player player = session.Player;
        player.InstanceId = channelId;
        player.ChannelId = channelId;

        player.WarpGameToGame(player.MapId, channelId);
    }
}
