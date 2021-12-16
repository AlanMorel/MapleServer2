using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class BuildModeHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.REQUEST_SET_BUILD_MODE;

    public BuildModeHandler() : base() { }

    private enum BuildModeMode : byte
    {
        Stop = 0x0,
        Start = 0x1
    }

    public enum BuildModeType : byte
    {
        Stop = 0x0,
        House = 0x1,
        Liftables = 0x2,
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        BuildModeMode mode = (BuildModeMode) packet.ReadByte();

        switch (mode)
        {
            case BuildModeMode.Stop:
                HandleStop(session);
                break;
            case BuildModeMode.Start:
                HandleStart(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleStop(GameSession session)
    {
        if (session.Player.Guide == null)
        {
            return;
        }
        session.FieldManager.BroadcastPacket(BuildModePacket.Use(session.Player.FieldPlayer, BuildModeType.Stop));
        session.FieldManager.BroadcastPacket(GuideObjectPacket.Remove(session.Player.Guide));
        session.FieldManager.RemoveGuide(session.Player.Guide);
        session.Player.Guide = null; // remove guide from player
    }

    private static void HandleStart(GameSession session, PacketReader packet)
    {
        if (session.Player.Guide != null)
        {
            return;
        }

        byte unk = packet.ReadByte();
        int furnishingItemId = packet.ReadInt();
        long furnishingItemUid = packet.ReadLong();

        // Add Guide Object
        CoordF startCoord = Block.ClosestBlock(session.Player.FieldPlayer.Coord);
        startCoord.Z += Block.BLOCK_SIZE;
        GuideObject guide = new(0, session.Player.CharacterId)
        {
        };
        IFieldObject<GuideObject> fieldGuide = session.FieldManager.RequestFieldObject(guide);
        fieldGuide.Coord = startCoord;
        session.Player.Guide = fieldGuide;
        session.FieldManager.AddGuide(fieldGuide);

        session.FieldManager.BroadcastPacket(GuideObjectPacket.Add(fieldGuide));
        session.FieldManager.BroadcastPacket(BuildModePacket.Use(session.Player.FieldPlayer, BuildModeType.House, furnishingItemId, furnishingItemUid));
    }
}
