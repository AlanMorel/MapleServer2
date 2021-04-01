using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{

    public static class GameEventPacket
    {
        public static Packet Load()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GAME_EVENT);
            pWriter.WriteByte(0);
            pWriter.WriteInt(1); // event count (loop)
            pWriter.WriteUnicodeString("StringBoard"); // type of event
            pWriter.WriteInt(40000); // event ID
            pWriter.WriteInt(0); // 0 = use string below. else, use an id in stringboardtext.xml
            pWriter.WriteUnicodeString("Welcome! Please see our Github or Discord for updates and to report any bugs. discord.gg/fVZRdBN7Nd");
            return pWriter;
        }
    }
}
