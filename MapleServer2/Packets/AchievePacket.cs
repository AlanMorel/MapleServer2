using System.Collections.Generic;
using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class AchievePacket
    {
        private enum AchievePacketMode : byte
        {
            // figure out what these are
            TableStart = 0x0,
            TableContent = 0x1,
            Update = 0x2
        }

        public static Packet WriteTableStart()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ACHIEVE);
            pWriter.WriteByte((byte) AchievePacketMode.TableStart);

            return pWriter;
        }

        // packet from WriteTableStart() must be sent immediately before sending these packets
        public static List<Packet> WriteTableContent(List<Achieve> achieves)
        {
            int achieveCount = achieves.Count();
            int sliceSize = achieveCount / 4 + 1;
            List<Packet> pWriters = new List<Packet>();
            // split achievement table into 4 packets
            // for (int p = 0; p < achieveCount; p += sliceSize)
            // {
                PacketWriter newWriter = PacketWriter.Of(SendOp.ACHIEVE);
                newWriter.WriteByte((byte) AchievePacketMode.TableContent);
                newWriter.WriteInt(60);
                newWriter.WriteInt(0x055ed126);
                newWriter.WriteInt(1); // unknown
                newWriter.WriteByte(3); // unknown
                newWriter.WriteInt(1);
                newWriter.WriteInt(1); // unknown
                newWriter.WriteByte(2); // unknown
                newWriter.WriteInt(0); // unknown this is for 0x1 mode
                newWriter.WriteLong(1);
                newWriter.WriteInt(1);
                newWriter.WriteInt(1);
                newWriter.WriteLong(0x5D140D39);
                pWriters.Add(newWriter);
            // }

            return pWriters;
        }

        public static Packet WriteUpdate(Achieve achieve)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ACHIEVE);
            pWriter.WriteByte((byte) AchievePacketMode.Update);
            pWriter.WriteByte((byte) AchievePacketMode.TableContent);
            pWriter.WriteInt(60);
            pWriter.WriteInt(0x055ed126);
            pWriter.WriteInt(1); // unknown
            pWriter.WriteByte(3); // unknown
            pWriter.WriteInt(1);
            pWriter.WriteInt(1); // unknown
            pWriter.WriteByte(2); // unknown
            pWriter.WriteLong(1);
            pWriter.WriteInt(1);
            pWriter.WriteInt(1);
            pWriter.WriteLong(0x5D140D39);

            return pWriter;
        }
    }
}
