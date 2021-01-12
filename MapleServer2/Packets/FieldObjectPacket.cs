using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class FieldObjectPacket
    {
        private enum ProxyGameObjMode : byte
        {
            LoadPlayer = 0x03,
            UpdateEntity = 0x05,
            LoadNpc = 0x6,
        }

        public static Packet LoadPlayer(IFieldObject<Player> fieldPlayer)
        {
            Player player = fieldPlayer.Value;
            PacketWriter pWriter = PacketWriter.Of(SendOp.PROXY_GAME_OBJ);
            pWriter.WriteMode(ProxyGameObjMode.LoadPlayer);
            pWriter.WriteInt(fieldPlayer.ObjectId);
            pWriter.WriteLong(player.AccountId);
            pWriter.WriteLong(player.CharacterId);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteUnicodeString(player.ProfileUrl);
            pWriter.WriteUnicodeString(player.Motto);
            pWriter.WriteByte();
            pWriter.Write(fieldPlayer.Coord);
            pWriter.WriteShort(player.Level);
            pWriter.WriteShort((short) player.JobGroupId);
            pWriter.WriteInt(player.JobId);
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteUnicodeString(player.HomeName);
            pWriter.WriteInt();
            pWriter.WriteShort();
            foreach (int trophyCount in player.Trophy)
            {
                pWriter.WriteInt(trophyCount);
            }

            return pWriter;
        }

        public static Packet UpdatePlayer(IFieldObject<Player> player)
        {
            FieldObjectUpdate flag = FieldObjectUpdate.Move | FieldObjectUpdate.Animate;
            PacketWriter pWriter = PacketWriter.Of(SendOp.PROXY_GAME_OBJ);
            pWriter.WriteMode(ProxyGameObjMode.UpdateEntity);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteByte((byte) flag);

            if (flag.HasFlag(FieldObjectUpdate.Type1))
            {
                pWriter.WriteByte();
            }
            if (flag.HasFlag(FieldObjectUpdate.Move))
            {
                pWriter.Write(player.Coord);
            }
            if (flag.HasFlag(FieldObjectUpdate.Type3))
            {
                pWriter.WriteShort();
            }
            if (flag.HasFlag(FieldObjectUpdate.Type4))
            {
                pWriter.WriteShort()
                    .WriteInt();
            }
            if (flag.HasFlag(FieldObjectUpdate.Type5))
            {
                pWriter.WriteUnicodeString("Unknown");
            }
            if (flag.HasFlag(FieldObjectUpdate.Type6))
            {
                pWriter.WriteInt();
            }
            if (flag.HasFlag(FieldObjectUpdate.Animate))
            {
                pWriter.WriteShort(player.Value.Animation);
            }

            return pWriter;
        }

        public static Packet LoadNpc(IFieldObject<Npc> npc)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PROXY_GAME_OBJ);
            pWriter.WriteMode(ProxyGameObjMode.LoadNpc);
            pWriter.WriteInt(npc.ObjectId);
            pWriter.WriteInt(npc.Value.Id);
            pWriter.WriteByte();
            pWriter.WriteInt(200);
            pWriter.Write(npc.Coord);
            return pWriter;
        }

        public static Packet LoadMob(IFieldObject<Mob> mob)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PROXY_GAME_OBJ);
            pWriter.WriteMode(ProxyGameObjMode.LoadNpc);
            pWriter.WriteInt(mob.ObjectId);
            pWriter.WriteInt(mob.Value.Id);
            pWriter.WriteByte();
            pWriter.WriteInt(200);
            pWriter.Write(mob.Coord);
            return pWriter;
        }

        public static Packet ControlNpc(IFieldObject<Npc> npc)
        {
            PacketWriter npcBuffer = new PacketWriter();
            npcBuffer.WriteInt(npc.ObjectId);
            npcBuffer.WriteByte();
            npcBuffer.Write(npc.Coord.ToShort());
            npcBuffer.WriteShort(npc.Value.Rotation);
            npcBuffer.Write(npc.Value.Speed); // XYZ Speed
            npcBuffer.WriteShort(100); // Unknown
            npcBuffer.WriteByte(1); // Flag ?
            npcBuffer.WriteShort(npc.Value.Animation);
            npcBuffer.WriteShort(1); // counter (increments every packet)
            // There can be more to this packet, probably dependent on Flag.

            PacketWriter pWriter = PacketWriter.Of(SendOp.NPC_CONTROL);
            pWriter.WriteShort(1); // Segments
            pWriter.WriteShort((short) npcBuffer.Length);
            pWriter.Write(npcBuffer.ToArray());
            return pWriter;
        }

        public static Packet ControlMob(IFieldObject<Mob> mob)
        {
            PacketWriter npcBuffer = new PacketWriter();
            npcBuffer.WriteInt(mob.ObjectId);
            npcBuffer.WriteByte();
            npcBuffer.Write(mob.Coord.ToShort());
            npcBuffer.WriteShort(mob.Value.Rotation);
            npcBuffer.Write(mob.Value.Speed); // XYZ Speed
            npcBuffer.WriteShort(100); // Unknown
            npcBuffer.WriteByte(1); // Flag ?
            npcBuffer.WriteShort(mob.Value.Animation);
            npcBuffer.WriteShort(1); // counter (increments every packet)
            // There can be more to this packet, probably dependent on Flag.

            PacketWriter pWriter = PacketWriter.Of(SendOp.NPC_CONTROL);
            pWriter.WriteShort(1); // Segments
            pWriter.WriteShort((short) npcBuffer.Length);
            pWriter.Write(npcBuffer.ToArray());
            return pWriter;
        }

        public static Packet MoveNpc(int objectId, CoordF coord)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PROXY_GAME_OBJ);
            pWriter.WriteMode(ProxyGameObjMode.UpdateEntity);
            pWriter.WriteInt(objectId);
            pWriter.WriteByte();
            pWriter.Write(coord);
            return pWriter;
        }
    }

    public class FieldPlayerUpdateData
    {
        public FieldObjectUpdate Flags;
        public CoordF Coord;
        public short Animation;
    }
}
