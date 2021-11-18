using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Packets.Helpers;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

// ClientTicks/Time here are probably used for animation
// Currently I am just updating animation instantly.
public class UserSyncHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.USER_SYNC;

    public UserSyncHandler() : base() { }

    public override void Handle(GameSession session, PacketReader packet)
    {
        byte function = packet.ReadByte(); // Unknown what this is for
        session.ServerTick = packet.ReadInt();
        session.ClientTick = packet.ReadInt();

        byte segments = packet.ReadByte();
        if (segments < 1)
        {
            return;
        }

        SyncState[] syncStates = new SyncState[segments];
        for (int i = 0; i < segments; i++)
        {
            syncStates[i] = packet.ReadSyncState();

            packet.ReadInt(); // ClientTicks
            packet.ReadInt(); // ServerTicks
        }

        PacketWriter syncPacket = SyncStatePacket.UserSync(session.Player.FieldPlayer, syncStates);
        session.FieldManager.BroadcastPacket(syncPacket, session);
        UpdatePlayer(session, syncStates);
    }

    public static void UpdatePlayer(GameSession session, SyncState[] syncStates)
    {
        Player player = session.Player;
        IFieldActor<Player> fieldPlayer = player.FieldPlayer;

        CoordF coord = syncStates[0].Coord.ToFloat();
        CoordF closestBlock = Block.ClosestBlock(coord);
        closestBlock.Z -= Block.BLOCK_SIZE; // Get block under player

        if (IsCoordSafe(session, syncStates[0].Coord, closestBlock))
        {
            CoordF safeBlock = Block.ClosestBlock(coord);
            safeBlock.Z += 1; // Without this player will spawn inside the block

            player.SafeBlock = closestBlock;
        }

        fieldPlayer.Coord = coord;
        fieldPlayer.Rotation = new()
        {
            Z = syncStates[0].Rotation / 10
        };

        if (IsOutOfBounds(fieldPlayer.Coord, session.FieldManager.BoundingBox))
        {
            session.Send(UserMoveByPortalPacket.Move(fieldPlayer, player.SafeBlock, fieldPlayer.Rotation));
            player.FallDamage();
        }
        // not sure if this needs to be synced here
        fieldPlayer.Animation = syncStates[0].Animation1;
    }

    private static bool IsOutOfBounds(CoordF coord, CoordS[] boundingBox)
    {
        short higherBoundZ = Math.Max(boundingBox[0].Z, boundingBox[1].Z);
        short lowerBoundZ = Math.Min(boundingBox[1].Z, boundingBox[0].Z);
        short higherBoundY = Math.Max(boundingBox[0].Y, boundingBox[1].Y);
        short lowerBoundY = Math.Min(boundingBox[1].Y, boundingBox[0].Y);
        short higherBoundX = Math.Max(boundingBox[0].X, boundingBox[1].X);
        short lowerBoundX = Math.Min(boundingBox[1].X, boundingBox[0].X);

        if (coord.Z > higherBoundZ || coord.Z < lowerBoundZ)
        {
            return true;
        }

        if (coord.Y > higherBoundY || coord.Y < lowerBoundY)
        {
            return true;
        }

        return coord.X > higherBoundX || coord.X < lowerBoundX;
    }

    private static bool IsCoordSafe(GameSession session, CoordS currentCoord, CoordF closestCoord)
    {
        // Save last coord if player is not falling and not in a air mount
        return MapMetadataStorage.BlockExists(session.Player.MapId, closestCoord.ToShort()) &&
            !session.Player.OnAirMount &&
            (session.Player.SafeBlock - closestCoord).Length() > 350 &&
            session.Player.FieldPlayer.Coord.Z == currentCoord.Z;
    }
}
