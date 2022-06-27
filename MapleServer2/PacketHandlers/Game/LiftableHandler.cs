using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Managers.Actors;
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
        Character fieldPlayer = session.Player.FieldPlayer;
        IFieldObject<LiftableObject> fieldLiftable;
        LiftableObject liftable;
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
            fieldLiftable = session.FieldManager.State.LiftableObjects.Values.FirstOrDefault(x => x.Coord == coordB.ToFloat());
            if (fieldLiftable is null)
            {
                return;
            }

            liftable = fieldLiftable.Value;

            liftable.State = LiftableState.Removed;
            liftable.ItemCount--;

            fieldPlayer.CarryingLiftable = fieldLiftable;

            session.FieldManager.BroadcastPacket(ResponseCubePacket.RemoveCube(fieldPlayer.ObjectId, fieldPlayer.ObjectId, fieldLiftable.Coord.ToByte()));
            session.FieldManager.BroadcastPacket(LiftablePacket.RemoveCube(fieldLiftable));
            session.FieldManager.BroadcastPacket(BuildModePacket.Use(fieldPlayer, BuildModeHandler.BuildModeType.Liftables, liftable.Metadata.ItemId));
            return;
        }

        if (!session.FieldManager.State.LiftableObjects.TryGetValue(id, out fieldLiftable) || fieldLiftable is null)
        {
            return;
        }

        liftable = fieldLiftable.Value;

        liftable.State = LiftableState.Removed;
        liftable.ItemCount--;

        fieldPlayer.CarryingLiftable = fieldLiftable;

        session.FieldManager.BroadcastPacket(LiftablePacket.UpdateEntityById(liftable));
        session.FieldManager.BroadcastPacket(BuildModePacket.Use(fieldPlayer, BuildModeHandler.BuildModeType.Liftables, liftable.Metadata.ItemId));
        session.FieldManager.BroadcastPacket(LiftablePacket.UpdateEntityById(liftable));
    }
}
