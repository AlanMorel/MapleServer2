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
        public static Packet WriteTableContent(List<Achieve> achieves)
        {
            int achieveCount = achieves.Count();
            PacketWriter pWriter = PacketWriter.Of(SendOp.ACHIEVE);
            pWriter.WriteEnum(AchievePacketMode.TableContent);
            pWriter.WriteInt(achieveCount);

            foreach (Achieve achieve in achieves)
            {
                pWriter.WriteInt(achieve.Id);
                pWriter.WriteInt(1); // unknown 
                pWriter.WriteByte(3); // unknown: 0x3 - check if grade completed
                pWriter.WriteInt(1); // unknown
                pWriter.WriteInt(1/*achieve.Grade*/);
                pWriter.WriteInt(2); // unknown
                pWriter.WriteByte(0); // unknown
                pWriter.WriteLong(achieve.Counter);
                int tCount = achieve.Timestamps.Count;
                pWriter.WriteInt(tCount);
                for (int t = 0; t < tCount; t++)
                {
                    pWriter.WriteInt(t+1);
                    pWriter.WriteLong(achieve.Timestamps.ElementAt(t));
                }
            }

            return pWriter;
        }

        public static Packet WriteUpdate(Achieve achieve)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ACHIEVE);
            pWriter.WriteEnum(AchievePacketMode.Update);
            pWriter.WriteInt(achieve.Id);
            pWriter.WriteByte(3); // unknown: 3 - finished final tier, else - earned a trophy
            pWriter.WriteInt(1); // unknown
            pWriter.WriteInt(achieve.Grade);
            pWriter.WriteInt(2); // unknown
            pWriter.WriteByte(0); // unknown
            pWriter.WriteLong(achieve.Counter);
            int tCount = achieve.Timestamps.Count;
            pWriter.WriteInt(tCount);
            for (int t = 0; t < tCount; t++)
            {
                pWriter.WriteInt(t+1);
                pWriter.WriteLong(achieve.Timestamps.ElementAt(t));
            }

            return pWriter;
        }
    }
}
