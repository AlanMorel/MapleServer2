using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets {
    public static class FieldObjectPacket {
        public static Packet LoadPlayer(IFieldObject<Player> fieldPlayer) {
            Player player = fieldPlayer.Value;
            var pWriter = PacketWriter.Of(SendOp.PROXY_GAME_OBJ)
                .WriteByte(0x03)
                .WriteInt(fieldPlayer.ObjectId)
                .WriteLong(player.AccountId)
                .WriteLong(player.CharacterId)
                .WriteUnicodeString(player.Name)
                .WriteUnicodeString(player.ProfileUrl)
                .WriteUnicodeString(player.Motto)
                .WriteByte()
                .Write<CoordF>(fieldPlayer.Coord)
                .WriteShort(player.Level)
                .WriteShort((short) player.JobGroupId)
                .WriteInt(player.JobId)
                .WriteInt()
                .WriteInt()
                .WriteInt()
                .WriteUnicodeString(player.HomeName)
                .WriteInt()
                .WriteShort();
            foreach (int trophyCount in player.Trophy) {
                pWriter.WriteInt(trophyCount);
            }

            return pWriter;
        }

        public static Packet UpdatePlayer(IFieldObject<Player> player) {
            FieldObjectUpdate flag = FieldObjectUpdate.Move | FieldObjectUpdate.Animate;
            var pWriter = PacketWriter.Of(SendOp.PROXY_GAME_OBJ)
                .WriteByte(0x05)
                .WriteInt(player.ObjectId)
                .WriteByte((byte)flag);

            if (flag.HasFlag(FieldObjectUpdate.Type1)) {
                pWriter.WriteByte();
            }
            if (flag.HasFlag(FieldObjectUpdate.Move)) {
                pWriter.Write<CoordF>(player.Coord);
            }
            if (flag.HasFlag(FieldObjectUpdate.Type3)) {
                pWriter.WriteShort();
            }
            if (flag.HasFlag(FieldObjectUpdate.Type4)) {
                pWriter.WriteShort()
                    .WriteInt();
            }
            if (flag.HasFlag(FieldObjectUpdate.Type5)) {
                pWriter.WriteUnicodeString("Unknown");
            }
            if (flag.HasFlag(FieldObjectUpdate.Type6)) {
                pWriter.WriteInt();
            }
            if (flag.HasFlag(FieldObjectUpdate.Animate)) {
                pWriter.WriteShort(player.Value.Animation);
            }

            return pWriter;
        }

        public static Packet LoadNpc(IFieldObject<Npc> npc) {
            return PacketWriter.Of(SendOp.PROXY_GAME_OBJ)
                .WriteByte(0x06)
                .WriteInt(npc.ObjectId)
                .WriteInt(npc.Value.Id)
                .WriteByte()
                .WriteInt(200)
                .Write<CoordF>(npc.Coord);
        }

        public static Packet ControlNpc(IFieldObject<Npc> npc) {
            var npcBuffer = new PacketWriter()
                .WriteInt(npc.ObjectId)
                .WriteByte()
                .Write<CoordS>(npc.Coord.ToShort())
                .WriteShort(npc.Value.Rotation)
                .Write<CoordS>(npc.Value.Speed) // XYZ Speed
                .WriteShort(100) // Unknown
                .WriteByte(1) // Flag ?
                .WriteShort(npc.Value.Animation)
                .WriteShort(1); // counter (increments every packet)
            // There can be more to this packet, probably dependent on Flag.

            return PacketWriter.Of(SendOp.NPC_CONTROL)
                .WriteShort(1) // Segments
                .WriteShort((short) npcBuffer.Length)
                .Write(npcBuffer.ToArray());
        }

        public static Packet MoveNpc(int objectId, CoordF coord) {
            return PacketWriter.Of(SendOp.PROXY_GAME_OBJ)
                .WriteByte(0x05)
                .WriteInt(objectId)
                .WriteByte()
                .Write<CoordF>(coord);
        }

        public static Packet SetStats(IFieldObject<Player> player) {
            return PacketWriter.Of(SendOp.STAT)
                .WriteInt(player.ObjectId)
                .WriteByte()
                .WriteByte(0x23)
                .Write<PlayerStats>(player.Value.Stats);
        }
    }

    public class FieldPlayerUpdateData {
        public FieldObjectUpdate Flags;
        public CoordF Coord;
        public short Animation;
    }
}