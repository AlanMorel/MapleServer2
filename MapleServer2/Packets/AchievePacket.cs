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

        // call this function 4 times in succession with index from 0 to 3
        public static Packet WriteTableContent(List<Achieve> achieves, int index)
        {
            int achieveCount = achieves.Count();
            PacketWriter pWriter = PacketWriter.Of(SendOp.ACHIEVE);
            pWriter.WriteByte((byte) AchievePacketMode.TableContent);
            pWriter.WriteInt(60);

            foreach (Achieve achieve in achieves.GetRange(index*(achieveCount/4), (index+1)*(achieveCount/4)))
            {
                Packet newPacket = WriteUpdate(achieve);
                pWriter.Write(newPacket.Buffer);
            }

            return pWriter;
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
