using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class VibrateHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.VIBRATE;

    public override void Handle(GameSession session, PacketReader packet)
    {
        string entityId = packet.ReadString();
        long skillSN = packet.ReadLong();
        int skillId = packet.ReadInt();
        short skillLevel = packet.ReadShort();
        short unkShort = packet.ReadShort();
        int unkInt = packet.ReadInt();
        CoordF playerCoords = packet.Read<CoordF>();

        Player player = session.Player;
        if (!MapEntityMetadataStorage.IsVibrateObject(player.MapId, entityId))
        {
            return;
        }

        session.FieldManager.BroadcastPacket(VibratePacket.Vibrate(entityId, player.FieldPlayer.SkillCast));
    }
}
