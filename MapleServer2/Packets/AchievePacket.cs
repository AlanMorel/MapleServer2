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

        public enum GradeStatus : byte
        {
            NotFinalGrade = 0x0,
            FinalGrade = 0x3
        }

        public static Packet WriteTableStart()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ACHIEVE);
            pWriter.WriteEnum(AchievePacketMode.TableStart);

            return pWriter;
        }

        // packet from WriteTableStart() must be sent immediately before sending these packets
        public static Packet WriteTableContent(List<Achieve> achieves)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ACHIEVE);
            pWriter.WriteEnum(AchievePacketMode.TableContent);
            pWriter.WriteInt(achieves.Count);

            foreach (Achieve achieve in achieves)
            {
                pWriter.WriteInt(achieve.Id);
                pWriter.WriteInt(1);            // unknown 'SS' ?
                pWriter.Write(WriteIndividualAchieve(achieve).Buffer);
            }

            return pWriter;
        }

        public static Packet WriteUpdate(Achieve achieve)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ACHIEVE);
            pWriter.WriteEnum(AchievePacketMode.Update);
            pWriter.WriteInt(achieve.Id);
            pWriter.Write(WriteIndividualAchieve(achieve).Buffer);

            return pWriter;
        }

        private static Packet WriteIndividualAchieve(Achieve achieve)
        {
            int tCount = achieve.Timestamps.Count;
            PacketWriter pWriter = new PacketWriter();

            pWriter.WriteEnum(achieve.GetGradeStatus());
            pWriter.WriteInt(1);
            pWriter.WriteInt(achieve.CurrentGrade);
            pWriter.WriteInt(achieve.MaxGrade);
            pWriter.WriteByte(0);
            pWriter.WriteLong(achieve.Counter);
            pWriter.WriteInt(tCount);
            for (int t = 0; t < tCount; t++)
            {
                pWriter.WriteInt(t + 1);
                pWriter.WriteLong(achieve.Timestamps.ElementAt(t));
            }

            return pWriter;
        }
    }
}
