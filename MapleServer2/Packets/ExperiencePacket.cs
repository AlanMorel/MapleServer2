using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class ExperiencePacket
    {
        public static Packet SendExpUp(int expGained, long expTotal, long restExp)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.EXP_UP);

            pWriter.WriteByte();
            pWriter.WriteInt(expGained);
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteLong(expTotal);
            pWriter.WriteLong(restExp); // rest exp
            pWriter.WriteInt(); // counter? increments after every exp_up
            pWriter.WriteByte();

            return pWriter;
        }

        public static Packet SendLevelUp(IFieldObject<Player> fieldPlayer, int level)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.LEVEL_UP);

            pWriter.WriteInt(fieldPlayer.ObjectId);
            pWriter.WriteInt(level);

            return pWriter;
        }
    }
}
