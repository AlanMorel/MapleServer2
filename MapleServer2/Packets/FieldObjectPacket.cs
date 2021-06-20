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
            RemovePlayer = 0x04,
            UpdateEntity = 0x05,
            LoadNpc = 0x06,
            RemoveNpc = 0x07,
            Unk1 = 0x08,
            Unk2 = 0x0B,
        }

        public static Packet LoadPlayer(IFieldObject<Player> fieldPlayer)
        {
            Player player = fieldPlayer.Value;
            PacketWriter pWriter = PacketWriter.Of(SendOp.PROXY_GAME_OBJ);
            pWriter.WriteEnum(ProxyGameObjMode.LoadPlayer);
            pWriter.WriteInt(fieldPlayer.ObjectId);
            pWriter.WriteLong(player.CharacterId);
            pWriter.WriteLong(player.AccountId);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteUnicodeString(player.ProfileUrl);
            pWriter.WriteUnicodeString(player.Motto);
            pWriter.WriteByte();
            pWriter.Write(fieldPlayer.Coord);
            pWriter.WriteShort(player.Levels.Level);
            pWriter.WriteShort((short) player.Job);
            pWriter.WriteShort((short) player.JobCode);
            pWriter.WriteShort();
            pWriter.WriteInt(player.MapId);
            pWriter.WriteLong(1); // unk
            pWriter.WriteUnicodeString(player.HomeName);
            pWriter.WriteInt();
            pWriter.WriteShort();
            foreach (int trophyCount in player.TrophyCount)
            {
                pWriter.WriteInt(trophyCount);
            }

            return pWriter;
        }

        public static Packet RemovePlayer(IFieldObject<Player> fieldPlayer)
        {
            Player player = fieldPlayer.Value;
            PacketWriter pWriter = PacketWriter.Of(SendOp.PROXY_GAME_OBJ);
            pWriter.WriteEnum(ProxyGameObjMode.RemovePlayer);
            pWriter.WriteInt(fieldPlayer.ObjectId);

            return pWriter;
        }

        public static Packet UpdatePlayer(IFieldObject<Player> player)
        {
            FieldObjectUpdate flag = FieldObjectUpdate.Move | FieldObjectUpdate.Animate;
            PacketWriter pWriter = PacketWriter.Of(SendOp.PROXY_GAME_OBJ);
            pWriter.WriteEnum(ProxyGameObjMode.UpdateEntity);
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
                pWriter.WriteShort();
                pWriter.WriteInt();
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
            pWriter.WriteEnum(ProxyGameObjMode.LoadNpc);
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
            pWriter.WriteEnum(ProxyGameObjMode.LoadNpc);
            pWriter.WriteInt(mob.ObjectId);
            pWriter.WriteInt(mob.Value.Id);
            pWriter.WriteByte();
            pWriter.WriteInt(200); // also 99 for boss
            pWriter.Write(mob.Coord);
            return pWriter;
        }

        public static Packet ControlNpc(IFieldObject<Npc> npc)
        {
            PacketWriter npcBuffer = new PacketWriter();
            npcBuffer.WriteInt(npc.ObjectId);
            npcBuffer.WriteByte();
            npcBuffer.Write(npc.Coord.ToShort());
            npcBuffer.WriteShort(npc.Value.ZRotation);
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
            npcBuffer.WriteShort(mob.Value.ZRotation);
            npcBuffer.Write(mob.Value.Velocity.ToShort()); // Displacement
            npcBuffer.WriteShort(100); // Unknown
            //npcBuffer.WriteInt(); // Unknown but is required for Boss, but not for normal mobs.
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
            pWriter.WriteEnum(ProxyGameObjMode.UpdateEntity);
            pWriter.WriteInt(objectId);
            pWriter.WriteByte();
            pWriter.Write(coord);
            return pWriter;
        }

        public static Packet RemoveNpc(IFieldObject<Npc> npc)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PROXY_GAME_OBJ);
            pWriter.WriteEnum(ProxyGameObjMode.RemoveNpc);
            pWriter.WriteInt(npc.ObjectId);
            return pWriter;
        }

        public static Packet RemoveMob(IFieldObject<Mob> mob)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PROXY_GAME_OBJ);
            pWriter.WriteEnum(ProxyGameObjMode.RemoveNpc);
            pWriter.WriteInt(mob.ObjectId);
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
