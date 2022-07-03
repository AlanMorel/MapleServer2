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
public class UserSyncHandler : GamePacketHandler<UserSyncHandler>
{
    public override RecvOp OpCode => RecvOp.UserSync;

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

        CoordF coordUnderneath = coord;
        coordUnderneath.Z -= 50;

        CoordF blockUnderneath = Block.ClosestBlock(coordUnderneath);
        if (IsCoordSafe(player, syncStates[0].Coord, blockUnderneath))
        {
            CoordF safeBlock = Block.ClosestBlock(coord);
            // TODO: Knowing the state of the player using the animation is probably not the correct way to do this
            // we will need to know the state of the player for other things like counting time spent on ropes/running/walking/swimming
            if (syncStates[0].Animation2 is 7 or 132) // swimming
            {
                safeBlock.Z += Block.BLOCK_SIZE; // Without this player will spawn under the water
            }

            safeBlock.Z += 10; // Without this player will spawn inside the block

            player.SafeBlock = safeBlock;
        }

        fieldPlayer.Coord = coord;
        fieldPlayer.Rotation = new()
        {
            Z = syncStates[0].Rotation / 10
        };

        if (IsOutOfBounds(fieldPlayer.Coord, session.FieldManager.BoundingBox))
        {
            player.Move(player.SafeBlock, fieldPlayer.Rotation);
            player.FallDamage();
        }

        // not sure if this needs to be synced here
        fieldPlayer.Animation = syncStates[0].BoreAnimation;
    }

    private static bool IsOutOfBounds(CoordF coord, CoordS[] boundingBox)
    {
        CoordS box1 = boundingBox[0];
        CoordS box2 = boundingBox[1];

        short higherBoundZ = Math.Max(box1.Z, box2.Z);
        short lowerBoundZ = Math.Min(box2.Z, box1.Z);

        short higherBoundY = Math.Max(box1.Y, box2.Y);
        short lowerBoundY = Math.Min(box2.Y, box1.Y);

        short higherBoundX = Math.Max(box1.X, box2.X);
        short lowerBoundX = Math.Min(box2.X, box1.X);

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

    private static bool IsCoordSafe(Player player, CoordS currentCoord, CoordF closestCoord)
    {
        // Check if current coord is safe to be used as a return point when the character falls off the map
        return MapMetadataStorage.BlockExists(player.MapId, closestCoord.ToShort())
               && !player.OnAirMount && (player.SafeBlock - closestCoord).Length() > 350 &&
               player.FieldPlayer.Coord.Z == currentCoord.Z;
    }
}
