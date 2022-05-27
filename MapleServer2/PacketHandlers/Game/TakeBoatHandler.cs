using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Managers.Actors;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class TakeBoatHandler : GamePacketHandler<TakeBoatHandler>
{
    public override RecvOp OpCode => RecvOp.TakeBoat;

    public override void Handle(GameSession session, PacketReader packet)
    {
        int npcObjectId = packet.ReadInt();
        if (!session.FieldManager.State.Npcs.TryGetValue(npcObjectId, out Npc npc))
        {
            return;
        }
        int mapId = 0;
        int mesoCost = 0;

        switch (npc.Value.Id)
        {
            case 11000585: // Seamus
                mapId = 2000124;
                mesoCost = 1000;
                break;
            case 11000994: // Lotachi
                mapId = 02000183;
                mesoCost = 4000;
                break;
            default:
                Logger.Information($"Unhandled boat npc: {npc.Value.Id}");
                return;
        }
        if (!session.Player.Wallet.Meso.Modify(-mesoCost))
        {
            return;
        }
        session.Player.Warp(mapId);
    }
}
