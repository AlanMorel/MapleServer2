using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{

    public static class EmotePacket
    {
        public static Packet LoadEmotes()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.EMOTION)
                .WriteByte(0x00); // Function

            // Some random hardcoded emotes for now
            List<int> emoteList = new List<int> {
                90200011, 90200004, 90200024, 90200041, 90200042,
                90200057, 90200043, 90200022, 90200031, 90200005,
                90200006, 90200003, 90200092, 90200077, 90200073,
                90200023, 90200001, 90200019, 90200020, 90200021,
                90200009, 90200027, 90200010, 90200028, 90200051,
                90200015, 90200016, 90200055, 90200060, 90200017,
                90200018, 90200093
            };
            pWriter.WriteInt(emoteList.Count);
            foreach (int emoteId in emoteList)
            {
                pWriter.WriteInt(emoteId)
                    .WriteInt(1)
                    .WriteLong();
            }

            return pWriter;
        }
    }
}