using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Packets
{

    public static class InstrumentPacket
    {
        private enum InstrumentPacketMode : byte
        {
            StartImprovise = 0x0,
            PlayNote = 0x1,
            StopImprovise = 0x2,
            PlayScore = 0x3,
            StopScore = 0x4,
            UpdateScoreUses = 0x9,
            Fireworks = 0xE,
        }

        public static Packet StartImprovise(IFieldObject<Player> player, byte gmId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAY_INSTRUMENT);
            pWriter.WriteEnum(InstrumentPacketMode.StartImprovise);
            pWriter.WriteInt(); // playId?
            pWriter.WriteInt(player.ObjectId);
            pWriter.Write(player.Coord);
            pWriter.WriteInt(gmId);
            pWriter.WriteInt(0);
            return pWriter;
        }

        public static Packet PlayNote(int note, IFieldObject<Player> player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAY_INSTRUMENT);
            pWriter.WriteEnum(InstrumentPacketMode.PlayNote);
            pWriter.WriteInt(); // playId?
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteInt(note);
            return pWriter;
        }

        public static Packet StopImprovise(IFieldObject<Player> player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAY_INSTRUMENT);
            pWriter.WriteEnum(InstrumentPacketMode.StopImprovise);
            pWriter.WriteInt(); // playId?
            pWriter.WriteInt(player.ObjectId);
            return pWriter;
        }

        public static Packet PlayScore(GameSession session, string itemFileName, byte gmId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAY_INSTRUMENT);
            pWriter.WriteEnum(InstrumentPacketMode.PlayScore);
            pWriter.WriteByte();
            pWriter.WriteInt(session.FieldPlayer.ObjectId + 0x10); // playId?
            pWriter.WriteInt(session.FieldPlayer.ObjectId);
            pWriter.Write(session.Player.Coord);
            pWriter.WriteInt(session.ClientTick);
            pWriter.WriteInt(gmId);
            pWriter.WriteInt();
            pWriter.WriteByte();
            pWriter.WriteUnicodeString(itemFileName);
            return pWriter;
        }

        public static Packet StopScore(IFieldObject<Player> player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAY_INSTRUMENT);
            pWriter.WriteEnum(InstrumentPacketMode.StopScore);
            pWriter.WriteInt(player.ObjectId + 0x10); // playId ?
            pWriter.WriteInt(player.ObjectId);
            return pWriter;
        }

        public static Packet UpdateScoreUses(long scoreItemUid, int usesRemaining)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAY_INSTRUMENT);
            pWriter.WriteEnum(InstrumentPacketMode.UpdateScoreUses);
            pWriter.WriteLong(scoreItemUid);
            pWriter.WriteInt(usesRemaining);
            return pWriter;
        }

        public static Packet Fireworks(int objectId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAY_INSTRUMENT);
            pWriter.WriteEnum(InstrumentPacketMode.Fireworks);
            pWriter.WriteInt(objectId);
            return pWriter;
        }
    }
}
