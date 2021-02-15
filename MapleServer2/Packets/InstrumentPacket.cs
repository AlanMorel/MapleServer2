using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{

    public static class InstrumentPacket
    {
        private enum InstrumentPacketMode : byte
        {
            StartImprovise = 0x0, //improvise only?
            PlayNote = 0x1,
            StopImprovise = 0x2,
            PlayScore = 0x3,
            StopScore = 0x4,
            ScoreUses = 0x9,
            Fireworks = 0xE,
        }

        public static Packet StartImprovise(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAY_INSTRUMENT);
            pWriter.WriteEnum(InstrumentPacketMode.StartImprovise);
            pWriter.WriteInt(); // playId?
            pWriter.WriteInt(player.Session.FieldPlayer.ObjectId);
            pWriter.Write(player.Session.FieldPlayer.Coord);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            return pWriter;
        }

        public static Packet PlayNote(int note, Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAY_INSTRUMENT);
            pWriter.WriteEnum(InstrumentPacketMode.PlayNote);
            pWriter.WriteInt(); // playId?
            pWriter.WriteInt(player.Session.FieldPlayer.ObjectId);
            pWriter.WriteInt(note);
            return pWriter;
        }

        public static Packet StopImprovise(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAY_INSTRUMENT);
            pWriter.WriteEnum(InstrumentPacketMode.StopImprovise);
            pWriter.WriteInt(); // playId?
            pWriter.WriteInt(player.Session.FieldPlayer.ObjectId);
            return pWriter;
        }

        public static Packet PlayScore(Player player, string itemFileName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAY_INSTRUMENT);
            pWriter.WriteEnum(InstrumentPacketMode.PlayScore);
            pWriter.WriteByte();
            pWriter.WriteInt(player.Session.FieldPlayer.ObjectId);
            pWriter.WriteInt(player.Session.FieldPlayer.ObjectId);
            pWriter.Write(player.Session.FieldPlayer.Coord);
            pWriter.WriteInt(0x616F09CE); // scoreUid?
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteByte();
            pWriter.WriteUnicodeString(itemFileName);
            return pWriter;
        }

        public static Packet StopScore(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAY_INSTRUMENT);
            pWriter.WriteEnum(InstrumentPacketMode.StopScore);
            pWriter.WriteInt(); // playId ?
            pWriter.WriteInt(player.Session.FieldPlayer.ObjectId);
            return pWriter;
        }

        public static Packet ScoreUses(long scoreItemUid, int usesRemaining)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAY_INSTRUMENT);
            pWriter.WriteEnum(InstrumentPacketMode.ScoreUses);
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
