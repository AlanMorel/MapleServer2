using System;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;
using MapleServer2.Tools;
using MapleServer2.Servers.Game;
using MapleServer2.Data.Static;
using Maple2Storage.Types.Metadata;

namespace MapleServer2.Packets
{
    public static class ResponseCubePacket
    {
        public static Packet Pickup(GameSession session, byte[] coords)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);

            // Convert to signed byte array
            sbyte[] sCoords = Array.ConvertAll(coords, b => unchecked((sbyte) b));
            // Default to rainbow tree
            int weaponId = 18000004;

            // Find matching mapObject
            foreach (MapObject mapObject in MapEntityStorage.GetObjects(session.Player.MapId))
            {
                if (mapObject.Coord.X == sCoords[0] && mapObject.Coord.Y == sCoords[1] && mapObject.Coord.Z == sCoords[2])
                {
                    weaponId = mapObject.WeaponId;
                    break;
                }
            }

            pWriter.WriteShort(0x0011); // Mode
            pWriter.WriteInt(session.FieldPlayer.ObjectId);
            pWriter.Write(coords);
            pWriter.WriteZero(1);
            pWriter.WriteInt(weaponId);
            pWriter.WriteInt(GuidGenerator.Int()); // Item uid

            return pWriter;
        }

        public static Packet Drop(IFieldObject<Player> player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_CUBE);

            pWriter.WriteShort(0x0012); // Mode
            pWriter.WriteInt(player.ObjectId);

            return pWriter;
        }
    }
}
