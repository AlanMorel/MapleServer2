using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
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
            LeaveEnsemble = 0x6,
            Compose = 0x8,
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

        public static Packet PlayScore(IFieldObject<Instrument> instrument)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAY_INSTRUMENT);
            pWriter.WriteEnum(InstrumentPacketMode.PlayScore);
            pWriter.WriteBool(instrument.Value.IsCustomScore);
            pWriter.WriteInt(instrument.ObjectId);
            pWriter.WriteInt(instrument.Value.PlayerObjectId);
            pWriter.Write(instrument.Coord);
            pWriter.WriteInt(instrument.Value.InstrumentTick);
            pWriter.WriteInt(instrument.Value.GmId);
            pWriter.WriteInt(instrument.Value.PercussionId);
            pWriter.WriteByte(1);

            if (instrument.Value.IsCustomScore)
            {
                pWriter.WriteMapleString(instrument.Value.Score.Score.Notes);
            }
            else
            {
                pWriter.WriteUnicodeString(instrument.Value.Score.FileName);
            }
            return pWriter;
        }

        public static Packet StopScore(IFieldObject<Instrument> instrument)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAY_INSTRUMENT);
            pWriter.WriteEnum(InstrumentPacketMode.StopScore);
            pWriter.WriteInt(instrument.ObjectId);
            pWriter.WriteInt(instrument.Value.PlayerObjectId);
            return pWriter;
        }

        public static Packet LeaveEnsemble()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAY_INSTRUMENT);
            pWriter.WriteEnum(InstrumentPacketMode.LeaveEnsemble);
            return pWriter;
        }

        public static Packet Compose(Item item)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PLAY_INSTRUMENT);
            pWriter.WriteEnum(InstrumentPacketMode.Compose);
            pWriter.WriteLong(item.Uid);
            pWriter.WriteItem(item);
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
