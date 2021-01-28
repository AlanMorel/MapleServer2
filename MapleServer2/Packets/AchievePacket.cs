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
            int sliceCount = achieveCount / 4 + 1;
            List<Packet> pWriters = new List<Packet>();
            // split achievement table into 4 packets
            for (int p = 0; p < achieveCount; p += sliceCount)
            {
                PacketWriter newWriter = PacketWriter.Of(SendOp.ACHIEVE);
                newWriter.WriteByte((byte) AchievePacketMode.TableContent);
                newWriter.WriteInt(60);
                foreach (Achieve achieve in achieves.GetRange(p, p + sliceCount))
                {
                    newWriter.Write(WriteUpdate(achieve).Buffer);
                }
                pWriters.Add(newWriter);
            }

            return pWriters;
        }

        public static Packet WriteUpdate(Achieve achieve)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ACHIEVE);
            pWriter.WriteByte((byte) AchievePacketMode.Update);
            pWriter.WriteInt(achieve.Id);
            pWriter.WriteInt(); // unknown
            pWriter.WriteByte(); // unknown
            pWriter.WriteInt(achieve.Grade);
            pWriter.WriteInt(); // unknown
            pWriter.WriteByte(); // unknown
            pWriter.WriteLong(achieve.Counter);
            pWriter.WriteInt(achieve.Timestamps.Count);
            for (int index = 0; index < achieve.Timestamps.Count; index++)
            {
                pWriter.WriteInt(index + 1);
                pWriter.WriteLong(achieve.Timestamps[index]);
            }

            return pWriter;
        }
    }
}
