using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class LiftableHandler : GamePacketHandler<LiftableHandler>
{
    private enum LiftableMode : byte
    {
        PickUp = 1
    }

    public override RecvOp OpCode => RecvOp.Liftable;

    public override void Handle(GameSession session, PacketReader packet)
    {
        LiftableMode mode = (LiftableMode) packet.ReadByte();

        switch (mode)
        {
            case LiftableMode.PickUp:
                HandlePickUp(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandlePickUp(GameSession session, PacketReader packet)
    {
        string id = packet.ReadString();
        IFieldActor<Player> fieldPlayer = session.Player.FieldPlayer;
        if (id.Contains('_'))
        {
            string coordHexa = long.Parse(id.Split('_')[1]).ToString("X2");
            if (coordHexa.Length == 5)
            {
                coordHexa = "0" + coordHexa;
            }

            CoordB coordB = CoordB.From(
                (sbyte) Convert.ToByte(coordHexa[4..], 16),
                (sbyte) Convert.ToByte(coordHexa.Substring(2, 2), 16),
                (sbyte) Convert.ToByte(coordHexa[..2], 16));
            LiftableObject liftable = session.FieldManager.State.LiftableObjects.Values.FirstOrDefault(x => x.Position == coordB.ToFloat());
            if (liftable is null)
            {
                return;
            }

            liftable.State = LiftableState.Removed;
            liftable.Enabled = false;

            session.FieldManager.BroadcastPacket(ResponseCubePacket.RemoveCube(fieldPlayer.ObjectId, fieldPlayer.ObjectId, liftable.Position.ToByte()));
            session.FieldManager.BroadcastPacket(LiftablePacket.RemoveCube(liftable));
            session.FieldManager.BroadcastPacket(BuildModePacket.Use(fieldPlayer, BuildModeHandler.BuildModeType.Liftables, liftable.Metadata.ItemId));
            return;
        }

        if (!session.FieldManager.State.LiftableObjects.TryGetValue(id, out LiftableObject liftable2))
        {
            return;
        }

        liftable2.State = LiftableState.Removed;
        liftable2.Enabled = false;

        session.FieldManager.BroadcastPacket(LiftablePacket.UpdateEntityById(liftable2));
        session.FieldManager.BroadcastPacket(BuildModePacket.Use(fieldPlayer, BuildModeHandler.BuildModeType.Liftables, liftable2.Metadata.ItemId));
        session.FieldManager.BroadcastPacket(LiftablePacket.UpdateEntityById(liftable2));
    }
}
